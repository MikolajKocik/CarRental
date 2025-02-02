using MediatR;
using Microsoft.AspNetCore.Hosting;


namespace CarRental.Application.Dto.UploadCarImage_Create;

public class UploadCarImageCommandHandler : IRequestHandler<UploadCarImageCommand, string>
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UploadCarImageCommandHandler(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> Handle(UploadCarImageCommand request, CancellationToken cancellation)
    {
        if (request.Image == null || request.Image.Length == 0)
            throw new ArgumentException("Image file is required.");

        var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(request.Image.FileName)}";
        var filePath = Path.Combine(uploadDir, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await request.Image.CopyToAsync(fileStream, cancellation);
        }

        return "/Images/" + fileName;
    }
}
