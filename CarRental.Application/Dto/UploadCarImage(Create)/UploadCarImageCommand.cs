using MediatR;
using Microsoft.AspNetCore.Http;

namespace CarRental.Application.Dto.UploadCarImage_Create
{
    public class UploadCarImageCommand : IRequest<string>
    {
        public int CarId { get; set; }
        public IFormFile Image { get; set; } = default!;
    }

}
