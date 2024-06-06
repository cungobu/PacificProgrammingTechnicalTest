using Application.Features.Images.Models;

namespace Application.Features.Images.Interfaces
{
    public interface IImageReportService
    {
        ImageDto Get(int id);
    }
}
