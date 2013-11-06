using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyFileManager.NET.interfaces
{
    public interface IDirectorySearchProvider
    {
        /// <summary>
        /// Searches for files in the given relative path
        /// </summary>
        /// <param name="relPath"></param>
        /// <param name="searchString">the search query</param>
        /// <returns></returns>
        string[] SearchFiles(string relPath, string searchString);

        /// <summary>
        /// Searches for folders in the given relative path
        /// </summary>
        /// <param name="relPath"></param>
        /// <param name="searchString">the search query</param>
        /// <returns></returns>
        string[] SearchFolders(string relPath, string searchString);
    }
}
