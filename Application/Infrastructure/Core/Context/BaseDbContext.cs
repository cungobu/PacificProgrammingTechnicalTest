using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Infrastructure.Core.Context
{
    public abstract class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {

        }

        public BaseDbContext() : base()
        {
        }

        public override int SaveChanges()
        {
            var saveTask = SaveChangesAsync();
            saveTask.Wait();
            return saveTask.Result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = this.BeforeSavingChanges().ToArray();
            
            var result = await this.InternalSaveChangesAsync();

            this.AfterSavingChanges(entities);

            return result;
        }

        protected virtual async Task<int> InternalSaveChangesAsync()
        {
            var affectedItems = 0;
            
            var saveLaterCicularEntries = GetCircularReferenceChangingEntities();

            affectedItems += await base.SaveChangesAsync();

            saveLaterCicularEntries.ToList().ForEach(c => c.RollbackState());

            affectedItems += await base.SaveChangesAsync();

            return affectedItems;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), this.AcceptedEntityConfiguration);
            base.OnModelCreating(modelBuilder);
        }

        protected virtual bool AcceptedEntityConfiguration(Type entityType)
        {
            return true;
        }

        protected virtual EntityEntry[] BeforeSavingChanges()
        {
            HashSet<EntityEntry> entries = new HashSet<EntityEntry>(new EntityEntryComparer());
            HashSet<EntityEntry> changingEntries = ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToHashSet();
            while (!changingEntries.All(entry => entries.Contains(entry)))
            {
                foreach (var changingEntry in changingEntries)
                {
                    if (!entries.Contains(changingEntry))
                    {
                        //if (changingEntry.Entity is IWritableEntity entity)
                        //{
                        //    changingEntry.EnsureEntityState();
                        //    entity.BeforeSave();
                        //}

                        entries.Add(changingEntry);
                    }                    
                }
                changingEntries = ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToHashSet();
            }
            return entries.ToArray();
        }

        protected virtual void BeforeSavingChanges(params EntityEntry[] entries)
        {
            foreach(var entry in entries)
            {
                //if (entry.Entity is IWritableEntity entity)
                //{
                //    entry.EnsureEntityState();
                //    entity.BeforeSave();
                //}
            }
        }

        protected virtual void AfterSavingChanges(params EntityEntry[] entries)
        {
            foreach (var entry in entries)
            {
                //if (entry.Entity is IWritableEntity entity)
                //{
                //    entity.AfterSave();
                //}
            }
        }

        protected virtual SaveLaterEntry[] GetCircularReferenceChangingEntities()
        {
            var saveLaterEntries = new List<SaveLaterEntry>();

            foreach (var changingEntry in this.ChangeTracker.Entries())
            {
                foreach (var @ref in changingEntry.References)
                {
                    var targetEntry = @ref.TargetEntry;
                    if (targetEntry == null)
                    {
                        continue;
                    }

                    if (targetEntry.State == EntityState.Added || targetEntry.State == EntityState.Deleted)
                    {
                        var circularReference = targetEntry.References.SingleOrDefault(r => (r.Metadata as RuntimeNavigation).ForeignKey.PrincipalEntityType.Name.Equals(changingEntry.Metadata.ClrType.FullName));
                        if (circularReference != null)
                        {
                            // We can't set circular reference with 1 SaveChanges call 
                            var saveLaterEntry = new SaveLaterEntry(targetEntry);
                            saveLaterEntry.BackupState();
                            saveLaterEntry.SetAsUnchanged();
                            saveLaterEntries.Add(saveLaterEntry);
                        }
                    }
                }
            }

            return saveLaterEntries.ToArray();
        }

        private class EntityEntryComparer : IEqualityComparer<EntityEntry>
        {
            public bool Equals(EntityEntry? x, EntityEntry? y)
            {
                return x?.Entity == y?.Entity;
            }

            public int GetHashCode([DisallowNull] EntityEntry obj)
            {
                return obj.Entity.GetHashCode();
            }
        }

        protected class SaveLaterEntry
        {
            public EntityEntry Entity { get; set;}
            public EntityState BeforeSavingState { get; set;}

            public SaveLaterEntry(EntityEntry entity)
            {
                this.Entity = entity;
            }

            public void BackupState()
            {
                this.BeforeSavingState = this.Entity.State;
            }
            
            public void RollbackState()
            {
                this.Entity.State = BeforeSavingState;
            }

            public void SetAsUnchanged()
            {
                this.Entity.State = EntityState.Unchanged;
            }
        }
    }
}
