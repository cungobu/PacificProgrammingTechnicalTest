using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mapper
{
    internal class Mapper : IMapper
    {
        private readonly AutoMapper.IMapper mapper;

        public Mapper(AutoMapper.IMapper mapper)
        {
            this.mapper = mapper;
        }

        public TargetType Map<SourceType, TargetType>(SourceType source)
        {
            return mapper.Map<TargetType>(source);
        }

        public TargetType Map<TargetType>(object source)
        {
            return mapper.Map<TargetType>(source);
        }

        public TargetType Merge<SourceType, TargetType>(SourceType source, TargetType target)
        {
            return mapper.Map(source, target);
        }
    }
}
