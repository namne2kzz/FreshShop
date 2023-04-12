using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.Common
{
    public interface IStorageService
    {
        public string GetFileUrl(string fileName);

        public Task SaveFileAsync(Stream mediaBinaryStream, string fileName);

        public Task DeleteFileAsync(string fileName);
    }
}
