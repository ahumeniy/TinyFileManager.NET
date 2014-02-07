using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using TinyFileManager.NET.enums;
using System.Collections.Specialized;

namespace TinyFileManager.NET
{

    public partial class dialog : System.Web.UI.Page
    {
        public string strType;
        public string strApply;
        public string strCmd;
        public string strFolder;
        public string strFile;
        public string strLang;
        public string strEditor;
        public string strCurrPath;
        public string strCurrLink;      // dialog.aspx?editor=.... for simplicity
        public string strCallback;
        public ArrayList arrLinks = new ArrayList();

        private int intColNum;
        private string[] arrFolders;
        private string[] arrFiles;
        private TinyFileManager.NET.clsFileItem objFItem;
        private bool boolOnlyImage;
        private bool boolOnlyVideo;

        protected void Page_Load(object sender, EventArgs e)
        {
            strCmd = Request.QueryString["cmd"] + "";
            strType = Request.QueryString["type"] + "";
            strFolder = Request.QueryString["folder"] + "";
            strFile = Request.QueryString["file"] + "";
            strLang = Request.QueryString["lang"] + "";      //not used right now, but grab it
            strEditor = Request.QueryString["editor"] + "";
            strCurrPath = Request.QueryString["currpath"] + "";
            strCallback = Request.QueryString["callback"] + "";

            //check inputs
            if (this.strCurrPath.Length > 0)
            {
                this.strCurrPath = this.strCurrPath.TrimEnd('\\') + "\\";
            }

            //set the apply string, based on the passed type
            if (this.strType == "")
            {
                this.strType = "0";
            }
            switch (this.strType)
            {
                case "1":
                    this.strApply = "apply_img";
                    this.boolOnlyImage = true;
                    break;
                case "2":
                    this.strApply = "apply_link";
                    break;
                default:
                    if (Convert.ToInt32(this.strType) >= 3)
                    {
                        this.strApply = "apply_video";
                        this.boolOnlyVideo = true;
                    }
                    else
                    {
                        this.strApply = "apply";
                    }
                    break;
            }

            switch (strCmd)
            {
                case "debugsettings":
                    Response.Write("<b>AllowCreateFolder:</b> " + clsConfig.boolAllowCreateFolder + "<br>");
                    Response.Write("<b>AllowDeleteFile:</b> " + clsConfig.boolAllowDeleteFile + "<br>");
                    Response.Write("<b>AllowDeleteFolder:</b> " + clsConfig.boolAllowDeleteFolder + "<br>");
                    Response.Write("<b>AllowUploadFile:</b> " + clsConfig.boolAllowUploadFile + "<br>");
                    Response.Write("<b>MaxUploadSizeMb:</b> " + clsConfig.intMaxUploadSizeMb + "<br>");
                    Response.Write("<b>AllowedAllExtensions:</b> " + clsConfig.strAllowedAllExtensions + "<br>");
                    Response.Write("<b>AllowedFileExtensions:</b> " + clsConfig.strAllowedFileExtensions + "<br>");
                    Response.Write("<b>AllowedImageExtensions:</b> " + clsConfig.strAllowedImageExtensions + "<br>");
                    Response.Write("<b>AllowedMiscExtensions:</b> " + clsConfig.strAllowedMiscExtensions + "<br>");
                    Response.Write("<b>AllowedMusicExtensions:</b> " + clsConfig.strAllowedMusicExtensions + "<br>");
                    Response.Write("<b>AllowedVideoExtensions:</b> " + clsConfig.strAllowedVideoExtensions + "<br>");
                    Response.Write("<b>BaseURL:</b> " + clsConfig.strBaseURL + "<br>");
                    Response.Write("<b>DocRoot:</b> " + clsConfig.strDocRoot + "<br>");
                    Response.Write("<b>ThumbPath:</b> " + clsConfig.objDirectoryResolver.GetAbsolutePath("", DirectoryType.Thumbnail) + "<br>");
                    Response.Write("<b>ThumbURL:</b> " + clsConfig.strThumbURL + "<br>");
                    Response.Write("<b>UploadPath:</b> " + clsConfig.objDirectoryResolver.GetAbsolutePath("", DirectoryType.Upload) + "<br>");
                    Response.Write("<b>UploadURL:</b> " + clsConfig.strUploadURL + "<br>");
                    Response.End();
                    break;
                case "createfolder":

                    strFolder = Request.Form["folder"] + "";

                    // end response if we dont't want folders beeing created in this folder
                    if (!clsConfig.objDirectoryResolver.CanCreateFolderInFolder(strFolder))
                    {
                        Response.End();
                        break;
                    }

                    try
                    {
                        //forge ahead without checking for existence
                        //catch will save us
                        clsConfig.objDirectoryResolver.CreateDirectory(strFolder, DirectoryType.Upload);
                        clsConfig.objDirectoryResolver.CreateDirectory(strFolder, DirectoryType.Thumbnail);

                        // end response, since it's an ajax call
                        Response.End();
                    }
                    catch
                    {
                        //TODO: write error
                    }
                    break;

                case "upload":
                    strFolder = Request.Form["folder"] + "";

                    try
                    {
                        // end response if we dont't want files uploaded in this folder
                        if (!clsConfig.objDirectoryResolver.CanUploadInFolder(strFolder))
                        {
                            Response.End();
                            break;
                        }

                        HttpPostedFile filUpload = Request.Files["file"];
                        string strTargetFileName;
                        string strTargetFile;
                        string strThumbFile;

                        //check file was submitted
                        if ((filUpload != null) && (filUpload.ContentLength > 0))
                        {
                            strTargetFileName = Path.GetFileName(filUpload.FileName);
                            strTargetFile = clsConfig.objDirectoryResolver.GetAbsolutePath(this.strFolder + strTargetFileName, DirectoryType.Upload);
                            strThumbFile = clsConfig.objDirectoryResolver.GetAbsolutePath(this.strFolder + strTargetFileName, DirectoryType.Thumbnail);
                            filUpload.SaveAs(strTargetFile);

                            if (this.isImageFile(strTargetFile))
                            {
                                this.createThumbnail(strTargetFile, strThumbFile);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                    }

                    // end response
                    if (Request.Form["fback"] == "true")
                    {
                        Response.Redirect(this.strCurrLink);
                    }
                    else
                    {
                        Response.End();
                    }
                    
                    break;

                case "download":
                    string absPath = clsConfig.objDirectoryResolver.GetAbsolutePath(this.strFile, DirectoryType.Upload);

                    FileInfo objFile = new FileInfo(absPath);
                    Response.ClearHeaders();
                    Response.AddHeader("Pragma", "private");
                    Response.AddHeader("Cache-control", "private, must-revalidate");
                    Response.AddHeader("Content-Type", "application/octet-stream");
                    Response.AddHeader("Content-Length", objFile.Length.ToString());
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(absPath));
                    Response.WriteFile(absPath);
                    break;

                case "delfile":
                    try
                    {
                        clsConfig.objDirectoryResolver.DeleteFile(this.strFile, DirectoryType.Upload);

                        if (clsConfig.objDirectoryResolver.FileExists(this.strFile, DirectoryType.Thumbnail))
                        {
                            clsConfig.objDirectoryResolver.DeleteFile(this.strFile, DirectoryType.Thumbnail);
                        }
                    }
                    catch
                    {
                        //TODO: set error
                    }
                    goto default;

                case "delfolder":
                    try
                    {
                        clsConfig.objDirectoryResolver.DeleteDirectory(strFolder, DirectoryType.Upload);
                        clsConfig.objDirectoryResolver.DeleteDirectory(strFolder, DirectoryType.Thumbnail);
                    }
                    catch
                    {
                        //TODO: set error
                    }
                    goto default;


                default:    //just a regular page load 
                    if (this.strCurrPath != "" || this.strCmd == "search")
                    {
                        // add "up one" folder
                        this.objFItem = new TinyFileManager.NET.clsFileItem();
                        this.objFItem.strName = "..";
                        this.objFItem.boolIsFolder = true;
                        this.objFItem.boolIsFolderUp = true;
                        this.objFItem.intColNum = this.getNextColNum();
                        this.objFItem.strPath = this.strCmd == "search" ? this.strCurrPath : this.getUpOneDir(this.strCurrPath);
                        this.objFItem.strClassType = "dir";
                        this.objFItem.strDeleteLink = "";//= "<a class=\"btn erase-button top-right disabled\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                        this.objFItem.strThumbImage = "img/ico/folder_return.png";
                        this.objFItem.strLink = "<a title=\"" + clsConfig.objLocalizationService.GetValue("Open") + "\" href=\"" + getLink(new NameValueCollection() { { "currpath", this.objFItem.strPath } }) + "\"><img class=\"directory-img\" src=\"" + this.objFItem.strThumbImage + "\" alt=\"folder\" /><h3>..</h3></a>";
                        this.arrLinks.Add(objFItem);
                    }

                    if (strCmd == "search")
                    {
                        arrFolders = clsConfig.objDirectorySearchProvider.SearchFolders(this.strCurrPath, Request.Form["query"]);
                        arrFiles = clsConfig.objDirectorySearchProvider.SearchFiles(this.strCurrPath, Request.Form["query"]);
                    }
                    else
                    {
                        arrFolders = clsConfig.objDirectoryResolver.GetDirectoriesRelative(this.strCurrPath, DirectoryType.Upload);
                        arrFiles = clsConfig.objDirectoryResolver.GetFiles(this.strCurrPath, DirectoryType.Upload);
                    }


                    //load folders
                    foreach (string strF in arrFolders)
                    {
                        string absFPath = clsConfig.objDirectoryResolver.GetAbsolutePath(strF, DirectoryType.Upload);

                        this.objFItem = new TinyFileManager.NET.clsFileItem();
                        // the name is now the full relative path minus the current path to display subfolders in the search display
                        // this will not affect anything in the regular view
                        this.objFItem.strName = (this.strCurrPath.Equals("") ? strF : strF.Replace(this.strCurrPath, ""));
                        this.objFItem.boolIsFolder = true;
                        this.objFItem.intColNum = this.getNextColNum();
                        this.objFItem.strPath = this.strCurrPath + this.objFItem.strName;
                        this.objFItem.strClassType = "dir";
                        if (clsConfig.objDirectoryResolver.CanDeleteFolder(strF))
                        {
                            this.objFItem.strDeleteLink = "<a href=\"" + getLink(new NameValueCollection() { { "cmd", "delfolder" }, { "folder", this.objFItem.strPath } }) + "\" class=\"btn erase-button top-right\" onclick=\"return confirm('" + clsConfig.objLocalizationService.GetValue("Are you sure to delete the folder and all the objects in it?") + "');\" title=\"" + clsConfig.objLocalizationService.GetValue("Erase") + "\"><i class=\"icon-trash\"></i></a>";
                        }
                        else
                        {
                            this.objFItem.strDeleteLink = "<a class=\"btn erase-button top-right disabled\" title=\"" + clsConfig.objLocalizationService.GetValue("Erase") + "\"><i class=\"icon-trash\"></i></a>";
                        }
                        this.objFItem.strThumbImage = "img/ico/folder.png";
                        this.objFItem.strLink = "<a title=\"" + clsConfig.objLocalizationService.GetValue("Open") + "\" href=\"" + getLink(new NameValueCollection() { { "currpath", this.objFItem.strPath } }) + "\"><img class=\"directory-img\" src=\"" + this.objFItem.strThumbImage + "\" alt=\"folder\" /><h3>" + this.objFItem.strName + "</h3></a>";
                        this.arrLinks.Add(objFItem);
                    }

                    // load files
                    foreach (string strF in arrFiles)
                    {
                        string absFilePath = clsConfig.objDirectoryResolver.GetAbsolutePath(strF, DirectoryType.Upload);
                        string fileName = Path.GetFileName(absFilePath);

                        this.objFItem = new TinyFileManager.NET.clsFileItem();
                        this.objFItem.strName = (this.strCurrPath.Equals("") ? strF : strF.Replace(this.strCurrPath, ""));
                        this.objFItem.boolIsFolder = false;
                        this.objFItem.strPath = this.strCurrPath + this.objFItem.strName;
                        this.objFItem.boolIsImage = this.isImageFile(fileName);
                        this.objFItem.boolIsVideo = this.isVideoFile(fileName);
                        this.objFItem.boolIsMusic = this.isMusicFile(fileName);
                        this.objFItem.boolIsMisc = this.isMiscFile(fileName);
                        // get display class type
                        if (this.objFItem.boolIsImage)
                        {
                            this.objFItem.strClassType = "2";
                        }
                        else
                        {
                            if (this.objFItem.boolIsMisc)
                            {
                                this.objFItem.strClassType = "3";
                            }
                            else
                            {
                                if (this.objFItem.boolIsMusic)
                                {
                                    this.objFItem.strClassType = "5";
                                }
                                else
                                {
                                    if (this.objFItem.boolIsVideo)
                                    {
                                        this.objFItem.strClassType = "4";
                                    }
                                    else
                                    {
                                        this.objFItem.strClassType = "1";
                                    }
                                }
                            }
                        }



                        // get delete link
                        if (clsConfig.objDirectoryResolver.CanDeleteFile(strF))
                        {
                            this.objFItem.strDeleteLink = "<a href=\"" + getLink(new NameValueCollection() { { "cmd", "delfile" }, { "file", this.objFItem.strPath } }) + "\" class=\"btn erase-button\" onclick=\"return confirm('Are you sure to delete this file?');\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                        }
                        else
                        {
                            this.objFItem.strDeleteLink = "<a class=\"btn erase-button disabled\" title=\"" + clsConfig.objLocalizationService.GetValue("Erase") + "\"><i class=\"icon-trash\"></i></a>";
                        }
                        // get thumbnail image
                        if (this.objFItem.boolIsImage)
                        {
                            if (!clsConfig.objDirectoryResolver.FileExists(this.objFItem.strPath, DirectoryType.Thumbnail))
                            {
                                createThumbnail(clsConfig.objDirectoryResolver.GetAbsolutePath(this.objFItem.strPath, DirectoryType.Upload), clsConfig.objDirectoryResolver.GetAbsolutePath(this.objFItem.strPath, DirectoryType.Thumbnail));
                            }

                            this.objFItem.strThumbImage = clsConfig.objUrlResolver.GetUrl(this.objFItem.strPath, DirectoryType.Thumbnail);
                        }
                        else
                        {
                            if (File.Exists(Directory.GetParent(Request.PhysicalPath).FullName + "\\img\\ico\\" + Path.GetExtension(absFilePath).TrimStart('.').ToUpper() + ".png"))
                            {
                                this.objFItem.strThumbImage = "img/ico/" + Path.GetExtension(strF).TrimStart('.').ToUpper() + ".png";
                            }
                            else
                            {
                                this.objFItem.strThumbImage = "img/ico/Default.png";
                            }
                        }
                        this.objFItem.strDownFormOpen = "<form action=\"" + getLink(new NameValueCollection() { { "cmd", "download" }, { "file", this.objFItem.strPath } }) + "\" method=\"post\" class=\"download-form\">";
                        if (this.objFItem.boolIsImage)
                        {
                            this.objFItem.strPreviewLink = "<a class=\"btn preview\" title=\"" + clsConfig.objLocalizationService.GetValue("Preview") + "\" data-url=\"" + clsConfig.objUrlResolver.GetUrl(this.objFItem.strPath, DirectoryType.Upload) + "\" data-toggle=\"lightbox\" href=\"#previewLightbox\"><i class=\"icon-eye-open\"></i></a>";
                        }
                        else
                        {
                            this.objFItem.strPreviewLink = "<a class=\"btn preview disabled\" title=\"" + clsConfig.objLocalizationService.GetValue("Preview") + "\"><i class=\"icon-eye-open\"></i></a>";
                        }
                        this.objFItem.strLink = "<a href=\"#\" title=\"" + clsConfig.objLocalizationService.GetValue("Select") + "\" onclick=\"" + this.strApply + "('" + clsConfig.objUrlResolver.GetUrl(this.objFItem.strPath, DirectoryType.Upload) + "'," + this.strType + ")\";\"><img data-src=\"holder.js/140x100\" alt=\"140x100\" src=\"" + this.objFItem.strThumbImage + "\" height=\"100\"><h4>" + this.objFItem.strName + "</h4></a>";

                        // check to see if it's the type of file we are looking at
                        if ((this.boolOnlyImage && this.objFItem.boolIsImage) || (this.boolOnlyVideo && this.objFItem.boolIsVideo) || (!this.boolOnlyImage && !this.boolOnlyVideo))
                        {
                            this.objFItem.intColNum = this.getNextColNum();
                            this.arrLinks.Add(objFItem);
                        }
                    } // foreach

                    break;
            }   // switch

        }   // page load

        public string getLink(NameValueCollection param)
        {
            Dictionary<string, string> requestParams = ToDictionary(HttpContext.Current.Request.QueryString);


            foreach (string key in param.AllKeys)
            {
                if (requestParams.ContainsKey(key))
                {
                    requestParams[key] = param[key];
                }
                else
                {
                    requestParams.Add(key, param[key]);
                }
            }

            // allways remove the command so we dont get any sideeffects
            if (param["cmd"] == null && requestParams.ContainsKey("cmd"))
            {
                requestParams.Remove("cmd");
            }

            return "dialog.aspx?" + String.Join("&", requestParams.Select(x => x.Key + "=" + x.Value));
        }

        #region extension methods
        private Dictionary<string, string> ToDictionary(NameValueCollection nvcoll)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (string key in nvcoll)
            {
                dic.Add(key, nvcoll[key]);
            }

            return dic;
        }
        #endregion

        public string getBreadCrumb()
        {
            string strRet;
            string[] arrFolders;
            string strTempPath = "";
            int intCount = 0;

            strRet = "<li><a href=\"" + getLink(new NameValueCollection() { { "currpath", "" } }) + "\"><i class=\"icon-home\"></i></a>";
            arrFolders = this.strCurrPath.Split('\\');

            foreach (string strFolder in arrFolders)
            {
                if (strFolder != "")
                {
                    strTempPath += strFolder + "\\";
                    intCount++;

                    if (intCount == (arrFolders.Length - 1) && strCmd != "search")
                    {
                        strRet += " <span class=\"divider\">/</span></li> <li class=\"active\">" + strFolder + "</li>";
                    }
                    else
                    {
                        strRet += " <span class=\"divider\">/</span></li> <li><a href=\"" + getLink(new NameValueCollection() { { "currpath", strTempPath } }) + "\">" + strFolder + "</a>";
                    }
                }
            }   // foreach

            if (strCmd == "search")
            {
                strRet += " <span class=\"divider\">/</span></li> <li class=\"active\">" + clsConfig.objLocalizationService.GetValue("Search") + ": " + Request.Form["query"] + "</li>";
            }

            return strRet;
        }   // getBreadCrumb 

        private bool isImageFile(string strFilename)
        {
            // new system to match extensions
            // ignores cases

            string ext = Path.GetExtension(strFilename).TrimStart('.').ToUpper();
            return clsConfig.arrAllowedImageExtensions.Any(m => m.ToUpper().Equals(ext));
        } // isImageFile

        private bool isVideoFile(string strFilename)
        {
            // new system to match extensions
            // ignores cases

            string ext = Path.GetExtension(strFilename).TrimStart('.').ToUpper();
            return clsConfig.arrAllowedVideoExtensions.Any(m => m.ToUpper().Equals(ext));
        } // isVideoFile

        private bool isMusicFile(string strFilename)
        {
            // new system to match extensions
            // ignores cases

            string ext = Path.GetExtension(strFilename).TrimStart('.').ToUpper();
            return clsConfig.arrAllowedMusicExtensions.Any(m => m.ToUpper().Equals(ext));
        } // isMusicFile

        private bool isMiscFile(string strFilename)
        {
            // new system to match extensions
            // ignores cases

            string ext = Path.GetExtension(strFilename).TrimStart('.').ToUpper();
            return clsConfig.arrAllowedMiscExtensions.Any(m => m.ToUpper().Equals(ext));
        } // isMiscFile

        private void createThumbnail(string strFilename, string strThumbFilename)
        {
            System.Drawing.Image.GetThumbnailImageAbort objCallback;
            System.Drawing.Image objFSImage;
            System.Drawing.Image objTNImage;
            System.Drawing.RectangleF objRect;
            System.Drawing.GraphicsUnit objUnits = System.Drawing.GraphicsUnit.Pixel;
            int intHeight = 0;
            int intWidth = 0;

            // open image and get dimensions in pixels
            objFSImage = System.Drawing.Image.FromFile(strFilename);
            objRect = objFSImage.GetBounds(ref objUnits);

            // what are we going to resize to, to fit inside 156x78
            getProportionalResize(Convert.ToInt32(objRect.Width), Convert.ToInt32(objRect.Height), ref intWidth, ref intHeight);

            // create thumbnail
            objCallback = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
            objTNImage = objFSImage.GetThumbnailImage(intWidth, intHeight, objCallback, IntPtr.Zero);

            // making sure the folder exists
            if (!new FileInfo(strThumbFilename).Directory.Exists)
            {
                new FileInfo(strThumbFilename).Directory.Create();
            }


            // finish up
            objFSImage.Dispose();
            objTNImage.Save(strThumbFilename);
            objTNImage.Dispose();

        } // createThumbnail

        private void getProportionalResize(int intOldWidth, int intOldHeight, ref int intNewWidth, ref int intNewHeight)
        {
            int intHDiff = 0;
            int intWDiff = 0;
            decimal decProp = 0;
            int intTargH = 78;
            int intTargW = 156;

            if ((intOldHeight <= intTargH) && (intOldWidth <= intTargW))
            {
                // no resize needed
                intNewHeight = intOldHeight;
                intNewWidth = intOldWidth;
                return;
            }

            //get the differences between desired and current height and width
            intHDiff = intOldHeight - intTargH;
            intWDiff = intOldWidth - intTargW;

            //whichever is the bigger difference is the chosen proportion
            if (intHDiff > intWDiff)
            {
                decProp = (decimal)intTargH / (decimal)intOldHeight;
                intNewHeight = intTargH;
                intNewWidth = Convert.ToInt32(Math.Round(intOldWidth * decProp, 0));
            }
            else
            {
                decProp = (decimal)intTargW / (decimal)intOldWidth;
                intNewWidth = intTargW;
                intNewHeight = Convert.ToInt32(Math.Round(intOldHeight * decProp, 0));
            }
        } // getProportionalResize

        private bool ThumbnailCallback()
        {
            return false;
        } // ThumbnailCallback

        public string getEndOfLine(int intColNum)
        {
            if (intColNum == 6)
            {
                return "</div><div class=\"space10\"></div>";
            }
            else
            {
                return "";
            }
        } // getEndOfLine

        public string getStartOfLine(int intColNum)
        {
            if (intColNum == 1)
            {
                return "<div class=\"row-fluid\">";
            }
            else
            {
                return "";
            }
        } // getStartOfLine

        private int getNextColNum()
        {
            this.intColNum++;
            if (this.intColNum > 6)
            {
                this.intColNum = 1;
            }
            return this.intColNum;
        } // getNextColNum

        private string getUpOneDir(string strInput)
        {
            string[] arrTemp;

            arrTemp = strInput.TrimEnd('\\').Split('\\');
            arrTemp[arrTemp.Length - 1] = "";
            return String.Join("\\", arrTemp);
        }

    }   // class

}   // namespace

