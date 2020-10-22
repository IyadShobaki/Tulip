using BlazorInputFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Tulip_BlazorUI.Contratcs
{
    public interface IFileUplaod
    {
        void UploadFile(IFileListEntry file, MemoryStream msFile, string pictureName);
    }
}
