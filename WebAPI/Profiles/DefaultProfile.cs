using Application.Features.Images.Models;
using TechnicalTest.Models;

namespace TechnicalTest.Profiles
{
    internal class DefaultProfile : AutoMapper.Profile
    {
        public DefaultProfile()
        {
            CreateMap<ImageDto, ImageViewModel>();
        }
    }
}