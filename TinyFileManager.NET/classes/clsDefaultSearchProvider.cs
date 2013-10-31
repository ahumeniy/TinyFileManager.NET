using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using TinyFileManager.NET.interfaces;

namespace TinyFileManager.NET.classes
{
    internal sealed class clsDefaultDirectorySearchProvider : IDirectorySearchProvider
    {
        public string[] SearchFiles(string relPath, string searchString)
        {
            return SearchFilesRegex(relPath, GetRegex(searchString)).ToArray();
        }

        private IEnumerable<string> SearchFilesRegex(string relPath, Regex expression)
        {
            IEnumerable<string> files = clsConfig.objDirectoryResolver.GetFiles(relPath, enums.DirectoryType.Upload).AsEnumerable().Where(m => expression.IsMatch(getFileName(m)));

            foreach (string subDir in clsConfig.objDirectoryResolver.GetDirectoriesRelative(relPath, enums.DirectoryType.Upload))
            {
                files = files.Concat(SearchFilesRegex(subDir, expression));
            }

            return files;
        }

        public string[] SearchFolders(string relPath, string searchString)
        {
            return SearchFoldersRegex(relPath, GetRegex(searchString)).ToArray();
        }

        private IEnumerable<string> SearchFoldersRegex(string relPath, Regex expression)
        {
            IEnumerable<string> files = clsConfig.objDirectoryResolver.GetDirectoriesRelative(relPath, enums.DirectoryType.Upload).AsEnumerable().Where(m => expression.IsMatch(getFileName(m)));

            foreach (string subDir in clsConfig.objDirectoryResolver.GetDirectoriesRelative(relPath, enums.DirectoryType.Upload))
            {
                files = files.Concat(SearchFoldersRegex(subDir, expression));
            }

            return files;
        }

        private string getFileName(string relPath)
        {
            return Path.GetFileName(clsConfig.objDirectoryResolver.GetAbsolutePath(relPath, enums.DirectoryType.Upload));
        }

        private Regex GetRegex(string input)
        {
            try
            {
                return new Regex(input, RegexOptions.IgnoreCase);
            }
            catch
            {
                return new Regex(Regex.Escape(input), RegexOptions.IgnoreCase);
            }
        }
    }
}