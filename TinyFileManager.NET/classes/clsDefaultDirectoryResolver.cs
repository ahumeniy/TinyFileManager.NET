using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TinyFileManager.NET.interfaces;

namespace TinyFileManager.NET.classes
{
    internal sealed class clsDefaultDirectoryResolver : IDirectoryResolver
    {
        /// <summary>
        /// Returns the root directory path for uploads set in the web.config
        /// Can differ per Request
        /// </summary>
        /// <returns>the root directory as string</returns>
        public string UploadDirectory
        {
            get
            {
                return clsConfig.strDocRoot + "\\" + Properties.Settings.Default.UploadPath.TrimEnd('\\') + "\\";
            }
        }

        /// <summary>
        /// Returns the root directory path for thumbs set in the web.config
        /// Can differ per Request
        /// </summary>
        /// <returns>the root directory as string</returns>
        public string ThumbsDirectory
        {
            get
            {
                return clsConfig.strDocRoot + "\\" + Properties.Settings.Default.ThumbPath.TrimEnd('\\') + "\\";
            }
        }
    }
}