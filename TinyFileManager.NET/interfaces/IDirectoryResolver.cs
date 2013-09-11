using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TinyFileManager.NET.interfaces
{
    public interface IDirectoryResolver
    {
        /// <summary>
        /// Returns the root directory path for uploads
        /// Can differ per Request
        /// </summary>
        /// <returns>the root directory as string</returns>
        string UploadDirectory { get; }

        /// <summary>
        /// Returns the root directory path for thumbs
        /// Can differ per Request
        /// </summary>
        /// <returns>the root directory as string</returns>
        string ThumbsDirectory { get; }
    }
}
