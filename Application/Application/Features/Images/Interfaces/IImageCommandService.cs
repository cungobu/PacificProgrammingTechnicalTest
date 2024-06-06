using Application.Features.Images.Commands;
using Application.Features.Images.Models;

namespace Application.Features.Images.Interfaces
{
    public interface IImageCommandService
    {
        ImageDto Update(UpdateImageCommand command);
        ImageDto Create(CreateImageCommand command);
    }
}
