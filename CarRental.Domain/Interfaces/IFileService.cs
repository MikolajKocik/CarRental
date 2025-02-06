using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Interfaces
{
    public interface IFileService
    {
        Task<List<string>> SaveFilesAsync(List<IFormFile> files, string folder);
        Task DeleteFilesAsync(List<string> filePaths, string folder);
    }
}
