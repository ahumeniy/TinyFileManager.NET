using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TinyFileManager.NET.enums;

namespace TinyFileManager.NET.interfaces
{
    public interface IDirectoryResolver
    {
        /// <summary>
        /// Creates a directory given subfolder
        /// </summary>
        /// <param name="subFolder"></param>
        void CreateDirectory(string relSubFolder, DirectoryType type);

        /// <summary>
        /// Returns the absolute local path of a given file or folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string GetAbsolutePath(string relPath, DirectoryType type);

        /// <summary>
        /// Will delete a file
        /// </summary>
        /// <param name="relFilePath"></param>
        void DeleteFile(string relFilePath, DirectoryType type);

        /// <summary>
        /// Will delete a directory
        /// </summary>
        /// <param name="relPath"></param>
        void DeleteDirectory(string relPath, DirectoryType type);

        /// <summary>
        /// Returns wheather or not a file exists
        /// </summary>
        /// <param name="relFilePath"></param>
        /// <returns></returns>
        bool FileExists(string relFilePath, DirectoryType type);

        /// <summary>
        /// Returns an array with relative paths to all child directories
        /// </summary>
        /// <param name="relPath"></param>
        /// <returns></returns>
        string[] GetDirectoriesRelative(string relPath, DirectoryType type);

        /// <summary>
        /// Returns an array containing the names of all files in the given relative folder
        /// </summary>
        /// <param name="relPath"></param>
        /// <returns></returns>
        string[] GetFiles(string relPath, DirectoryType type);

        /// <summary>
        /// Returns the relative path of a folder's parent
        /// </summary>
        /// <param name="relPath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetParentRelative(string relPath, DirectoryType type);
    }
}
