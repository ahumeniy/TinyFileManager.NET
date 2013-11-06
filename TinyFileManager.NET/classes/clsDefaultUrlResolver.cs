using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TinyFileManager.NET.enums;
using TinyFileManager.NET.interfaces;

namespace TinyFileManager.NET.classes
{
    internal sealed class clsDefaultUrlResolver : IUrlResolver
    {
        public string GetUrl(string relPath, DirectoryType type)
        {
            string baseUrl = type == DirectoryType.Upload ? clsConfig.strUploadURL : clsConfig.strThumbURL;

            return baseUrl + "/" + relPath.Replace('\\', '/');
        }
    }
}