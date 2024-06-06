using Application.Features.Images.Interfaces;
using Application.Features.Images.Models;
using AutoMapper;
using Domain.Repositories;

namespace Application.Features.Images.Services
{
    public class ImageReportService : IImageReportService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Domain.Entities.Image> _imageRepository;

        public ImageReportService(IMapper mapper,
                                IRepository<Domain.Entities.Image> imageRepository)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        public ImageDto Get(int id)
        {
            var activity = _imageRepository.Queryable()
                .FirstOrDefault(c => c.Id == id);

            if (activity == null)
            {
                throw new Exception($"Cannot found image with id {id}");
            }

            var imageDto = _mapper.Map<ImageDto>(activity);

            return imageDto;
        }
    }
}