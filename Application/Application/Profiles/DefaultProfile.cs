using Application.Features.Images.Models;
using Domain.Entities;

namespace Application.Profiles
{
    internal class DefaultProfile : AutoMapper.Profile
    {
        public DefaultProfile()
        {
            CreateMap<Image, ImageDto>();
        }
    }
}