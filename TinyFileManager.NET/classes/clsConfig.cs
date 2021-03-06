﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using TinyFileManager.NET.interfaces;
using TinyFileManager.NET.classes;
using System.Web.Configuration;

namespace TinyFileManager.NET
{
    public static class clsConfig
    {
        #region Settings Properties
        /// <summary>
        ///  Max upload filesize in Mb
        /// </summary>
        public static int intMaxUploadSizeMb
        {
            get
            {
                return Convert.ToInt32(WebConfigurationManager.AppSettings["MaxUploadSizeMb"].ToString());
            }
        }

        /// <summary>
        ///  Allowed image file extensions
        /// </summary>
        public static string strAllowedImageExtensions
        {
            get
            {
                return WebConfigurationManager.AppSettings["AllowedImageExtensions"].ToString();
            }
        }

        /// <summary>
        ///  Allowed image file extensions as an array
        /// </summary>
        public static string[] arrAllowedImageExtensions
        {
            get
            {
                return getArrayFromString(clsConfig.strAllowedImageExtensions);
            }
        }

        /// <summary>
        ///  Allowed document file extensions
        /// </summary>
        public static string strAllowedFileExtensions
        {
            get
            {
                return WebConfigurationManager.AppSettings["AllowedFileExtensions"].ToString();
            }
        }

        /// <summary>
        ///  Allowed document file extensions as an array
        /// </summary>
        public static string[] arrAllowedFileExtensions
        {
            get
            {
                return getArrayFromString(clsConfig.strAllowedFileExtensions);
            }
        }

        /// <summary>
        ///  Allowed video file extensions
        /// </summary>
        public static string strAllowedVideoExtensions
        {
            get
            {
                return WebConfigurationManager.AppSettings["AllowedVideoExtensions"].ToString();
            }
        }

        /// <summary>
        ///  Allowed video file extensions as an array
        /// </summary>
        public static string[] arrAllowedVideoExtensions
        {
            get
            {
                return getArrayFromString(clsConfig.strAllowedVideoExtensions);
            }
        }

        /// <summary>
        ///  Allowed music file extensions
        /// </summary>
        public static string strAllowedMusicExtensions
        {
            get
            {
                return WebConfigurationManager.AppSettings["AllowedMusicExtensions"].ToString();
            }
        }

        /// <summary>
        ///  Allowed music file extensions as an array
        /// </summary>
        public static string[] arrAllowedMusicExtensions
        {
            get
            {
                return getArrayFromString(clsConfig.strAllowedMusicExtensions);
            }
        }

        /// <summary>
        ///  Allowed misc file extensions
        /// </summary>
        public static string strAllowedMiscExtensions
        {
            get
            {
                return WebConfigurationManager.AppSettings["AllowedMiscExtensions"].ToString();
            }
        }

        /// <summary>
        ///  Allowed misc file extensions as an array
        /// </summary>
        public static string[] arrAllowedMiscExtensions
        {
            get
            {
                return getArrayFromString(clsConfig.strAllowedMiscExtensions);
            }
        }

        /// <summary>
        ///  All allowed file extensions
        /// </summary>
        public static string strAllowedAllExtensions
        {
            get
            {
                string strRet = "";

                if (clsConfig.strAllowedImageExtensions.Length > 0)
                {
                    strRet = clsConfig.strAllowedImageExtensions;
                }
                if (clsConfig.strAllowedFileExtensions.Length > 0)
                {
                    if (strRet.Length > 0)
                    {
                        strRet += "," + clsConfig.strAllowedFileExtensions;
                    }
                    else
                    {
                        strRet = clsConfig.strAllowedFileExtensions;
                    }
                }
                if (clsConfig.strAllowedVideoExtensions.Length > 0)
                {
                    if (strRet.Length > 0)
                    {
                        strRet += "," + clsConfig.strAllowedVideoExtensions;
                    }
                    else
                    {
                        strRet = clsConfig.strAllowedVideoExtensions;
                    }
                }
                if (clsConfig.strAllowedMusicExtensions.Length > 0)
                {
                    if (strRet.Length > 0)
                    {
                        strRet += "," + clsConfig.strAllowedMusicExtensions;
                    }
                    else
                    {
                        strRet = clsConfig.strAllowedMusicExtensions;
                    }
                }
                if (clsConfig.strAllowedMiscExtensions.Length > 0)
                {
                    if (strRet.Length > 0)
                    {
                        strRet += "," + clsConfig.strAllowedMiscExtensions;
                    }
                    else
                    {
                        strRet = clsConfig.strAllowedMiscExtensions;
                    }
                }

                return strRet;
            }
        }

        /// <summary>
        /// Returns document root
        /// </summary>
        public static string strDocRoot
        {
            get
            {
                //return HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"].TrimEnd('\\');
                return HttpContext.Current.Server.MapPath("/").TrimEnd('\\');
            }
        }

        /// <summary>
        /// Returns the base url of the site
        /// </summary>
        public static string strBaseURL
        {
            get
            {
                return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority.TrimEnd('/');
            }
        }

        /// <summary>
        /// Returns the full upload drive path
        /// </summary>
        public static string strUploadPath
        {
            get
            {
                return clsConfig.strDocRoot + "\\" + WebConfigurationManager.AppSettings["UploadPath"].ToString().TrimEnd('\\') + "\\";
            }
        }

        /// <summary>
        /// Returns the full thumb drive path
        /// </summary>
        public static string strThumbPath
        {
            get
            {
                return clsConfig.strDocRoot + "\\" + WebConfigurationManager.AppSettings["ThumbPath"].ToString().TrimEnd('\\') + "\\";
            }
        }

        /// <summary>
        /// Returns the full upload url
        /// </summary>
        public static string strUploadURL
        {
            get
            {
                return clsConfig.strBaseURL + "/" + WebConfigurationManager.AppSettings["UploadPath"].ToString().Replace('\\', '/');
            }
        }

        /// <summary>
        /// Returns the full thumb url
        /// </summary>
        public static string strThumbURL
        {
            get
            {
                return clsConfig.strBaseURL + "/" + WebConfigurationManager.AppSettings["ThumbPath"].ToString().Replace('\\', '/');
            }
        }

        /// <summary>
        /// Returns the setting for allowing upload of file
        /// </summary>
        public static bool boolAllowUploadFile
        {
            get
            {
                return Convert.ToBoolean(WebConfigurationManager.AppSettings["AllowUploadFile"].ToString());
            }
        }

        /// <summary>
        /// Returns the setting for allowing delete of file
        /// </summary>
        public static bool boolAllowDeleteFile
        {
            get
            {
                return Convert.ToBoolean(WebConfigurationManager.AppSettings["AllowDeleteFile"].ToString());
            }
        }

        /// <summary>
        /// Returns the setting for allowing search
        /// </summary>
        public static bool boolAllowSearch
        {
            get
            {
                return Convert.ToBoolean(WebConfigurationManager.AppSettings["AllowSearch"].ToString());
            }
        }

        /// <summary>
        /// Returns the setting for allowing creation of folder
        /// </summary>
        public static bool boolAllowCreateFolder
        {
            get
            {
                return Convert.ToBoolean(WebConfigurationManager.AppSettings["AllowCreateFolder"].ToString());
            }
        }

        /// <summary>
        /// Returns the setting for allowing delete of folder
        /// </summary>
        public static bool boolAllowDeleteFolder
        {
            get
            {
                return Convert.ToBoolean(WebConfigurationManager.AppSettings["AllowDeleteFolder"].ToString());
            }
        }

        /// <summary>
        /// Returns the prefix for all ressources (script and css imports) so they can be located
        /// </summary>
        public static string strRessourcePrefix
        {
            get
            {
                return WebConfigurationManager.AppSettings["RessourcePrefix"].ToString();
            }
        }

        /// <summary>
        /// Returns the DirectoryResolver used to get the Thumb and UplaodDirectory
        /// </summary>
        public static IDirectoryResolver objDirectoryResolver
        {
            get
            {
                if (_objDirectoryResolver == null)
                {
                    _objDirectoryResolver = new clsDefaultDirectoryResolver();
                }

                return _objDirectoryResolver;
            }
            set
            {
                _objDirectoryResolver = value;
            }
        }

        /// <summary>
        /// Returns the LocalizationService used localize texts
        /// </summary>
        public static ILocalizationService objLocalizationService
        {
            get
            {
                if (_objLocalizationService == null)
                {
                    _objLocalizationService = new clsDefaultLocalizationService();
                }

                return _objLocalizationService;
            }
            set
            {
                _objLocalizationService = value;
            }
        }

        /// <summary>
        /// Returns the LocalizationService used localize texts
        /// </summary>
        public static IDirectorySearchProvider objDirectorySearchProvider
        {
            get
            {
                if (_objDirectorySearchProvider == null)
                {
                    _objDirectorySearchProvider = new clsDefaultDirectorySearchProvider();
                }

                return _objDirectorySearchProvider;
            }
            set
            {
                _objDirectorySearchProvider = value;
            }
        }

        /// <summary>
        /// Returns the UrlResolver used to resolve urls
        /// </summary>
        public static IUrlResolver objUrlResolver
        {
            get
            {
                if (_objUrlResolver == null)
                {
                    _objUrlResolver = new clsDefaultUrlResolver();
                }

                return _objUrlResolver;
            }
            set
            {
                _objUrlResolver = value;
            }
        }

        #endregion

        private static string[] getArrayFromString(string strInput)
        {
            string[] arrExt;
            string strTemp;

            //remove lead and trail single quotes so we can SPLIT the hell out of it
            strTemp = strInput.Trim('\'');
            arrExt = strTemp.Split(new string[] { "'", ",", "'" }, StringSplitOptions.RemoveEmptyEntries);

            return arrExt;
        }   // getArrayFromString

        #region Fields
        private static IDirectoryResolver _objDirectoryResolver;
        private static ILocalizationService _objLocalizationService;
        private static IDirectorySearchProvider _objDirectorySearchProvider;
        private static IUrlResolver _objUrlResolver;
        #endregion

    }   // class


}   // namespace

