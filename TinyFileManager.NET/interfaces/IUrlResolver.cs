using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyFileManager.NET.enums;

namespace TinyFileManager.NET.interfaces
{
    public interface IUrlResolver
    {
        //similiar to the DirectoryResolver this can be used to alter the give url

        string GetUrl(string relPath, DirectoryType type);
    }
}
