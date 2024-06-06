using Application.Features.Images.Commands;
using Application.Features.Images.Interfaces;
using Application.Features.Images.Models;
using AutoMapper;
using Domain;
using Domain.Repositories;

namespace Application.Features.Images.Services
{
    public class ImageCommandService : IImageCommandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Domain.Entities.Image> _imageRepository;

        public ImageCommandService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IRepository<Domain.Entities.Image> imageRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageRepository = imageRepository;
        }

        #region CRUD
        public ImageDto Update(UpdateImageCommand command)
        {
            Domain.Entities.Image image = _imageRepository.Find(command.Id);
            if (image == null)
            {
                throw new Exception($"Cannot found image with id {command.Id}");
            }

            image.Url = command.Url;

            _imageRepository.Update(image);

            _unitOfWork.SaveChanges();
            return _mapper.Map<ImageDto>(image);
        }

        public ImageDto Create(CreateImageCommand command)
        {
            var image = new Domain.Entities.Image();

            image.Url = command.Url;

            _imageRepository.Add(image);

            _unitOfWork.SaveChanges();
            return _mapper.Map<ImageDto>(image);
        }
        #endregion CRUD
    }
}