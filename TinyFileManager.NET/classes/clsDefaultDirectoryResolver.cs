using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TinyFileManager.NET.enums;
using TinyFileManager.NET.interfaces;

namespace TinyFileManager.NET.classes
{
    internal sealed class clsDefaultDirectoryResolver : IDirectoryResolver
    {

        public void CreateDirectory(string relSubFolder, DirectoryType type)
        {
            Directory.CreateDirectory(GetAbsolutePath(relSubFolder, type));
        }

        public string GetAbsolutePath(string relPath, DirectoryType type)
        {
            string strPathBase = type == DirectoryType.Upload ? clsConfig.strUploadPath : clsConfig.strThumbPath;

            return (strPathBase + relPath).Replace("\\\\", "\\");
        }

        public void DeleteFile(string relFilePath, DirectoryType type)
        {
            File.Delete(GetAbsolutePath(relFilePath, type));
        }

        public void DeleteDirectory(string relPath, DirectoryType type)
        {
            Directory.Delete(GetAbsolutePath(relPath, type));
        }

        public bool FileExists(string relFilePath, DirectoryType type)
        {
            return File.Exists(GetAbsolutePath(relFilePath, type));
        }

        public string[] GetDirectoriesRelative(string relPath, DirectoryType type)
        {
            try
            {
                return Directory.GetDirectories(GetAbsolutePath(relPath, type)).AsEnumerable<string>().Select(m => GetRelativePath(m, type)).ToArray();
            }
            catch
            {
                return new string[0];
            }
        }

        public string[] GetFiles(string relPath, DirectoryType type)
        {
            try
            {
                return Directory.GetFiles(GetAbsolutePath(relPath, type)).AsEnumerable<string>().Select(m => GetRelativePath(m, type)).ToArray();
            }
            catch
            {
                return new string[0];
            }
        }

        /// <summary>
        /// converts a absolute path to a relative path 
        /// </summary>
        /// <param name="absPath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetRelativePath(string absPath, DirectoryType type)
        {
            string strPathBase = type == DirectoryType.Upload ? clsConfig.strUploadPath : clsConfig.strThumbPath;

            return absPath.Replace(strPathBase, "");
        }

        public string GetParentRelative(string relPath, DirectoryType type)
        {
            return GetRelativePath(Directory.GetParent(GetAbsolutePath(relPath, type)).FullName, type);
        }

        public bool CanDeleteFile(string relPath)
        {
            return clsConfig.boolAllowDeleteFile;
        }

        public bool CanDeleteFolder(string relPath)
        {
            return clsConfig.boolAllowDeleteFolder;
        }

        public bool CanUploadInFolder(string relPath)
        {
            return true;
        }

        public bool CanCreateFolderInFolder(string relPath)
        {
            return true;
        }
    }
}