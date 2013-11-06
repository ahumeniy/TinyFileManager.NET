﻿<%@ Page Title="Tiny File Manager" Language="C#" AutoEventWireup="true" CodeBehind="dialog.aspx.cs" Inherits="TinyFileManager.NET.dialog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="robots" content="noindex,nofollow">
    <title>Tiny File Manager</title>
    <link href="<%Response.Write(TinyFileManager.NET.clsConfig.strRessourcePrefix);%>css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="<%Response.Write(TinyFileManager.NET.clsConfig.strRessourcePrefix);%>css/bootstrap-lightbox.min.css" rel="stylesheet" type="text/css" />
    <link href="<%Response.Write(TinyFileManager.NET.clsConfig.strRessourcePrefix);%>css/style.css" rel="stylesheet" type="text/css" />
    <link href="<%Response.Write(TinyFileManager.NET.clsConfig.strRessourcePrefix);%>css/dropzone.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="<%Response.Write(TinyFileManager.NET.clsConfig.strRessourcePrefix);%>js/jquery.1.9.1.min.js"></script>
    <script type="text/javascript" src="<%Response.Write(TinyFileManager.NET.clsConfig.strRessourcePrefix);%>js/bootstrap.min.js"></script>
    <script type="text/javascript" src="<%Response.Write(TinyFileManager.NET.clsConfig.strRessourcePrefix);%>js/bootstrap-lightbox.min.js"></script>
    <script type="text/javascript" src="<%Response.Write(TinyFileManager.NET.clsConfig.strRessourcePrefix);%>js/dropzone.min.js"></script>
    <script type="text/javascript">
        var ext_img=new Array(<% Response.Write(TinyFileManager.NET.clsConfig.strAllowedImageExtensions); %>);
        var allowed_ext=new Array(<% Response.Write(TinyFileManager.NET.clsConfig.strAllowedAllExtensions); %>);
        var track = '<% Response.Write(this.strEditor); %>';
        var curr_dir = '<% Response.Write(this.strCurrPath.Replace("\\", "\\\\")); %>';
        var callback = '<% Response.Write(this.strCallback); %>';

        //dropzone config
        Dropzone.options.myAwesomeDropzone = {
            dictInvalidFileType: "File extension is not allowed",
            dictFileTooBig: "The upload exceeds the max filesize allowed",
            dictResponseError: "SERVER ERROR",
            paramName: "file", // The name that will be used to transfer the file
            maxFilesize: <% Response.Write(TinyFileManager.NET.clsConfig.intMaxUploadSizeMb); %>, // MB
            accept: function(file, done) {
                var extension=file.name.split('.').pop();
                if ($.inArray(extension, allowed_ext) > -1) {
                    done();
                } else { 
                    done("File extension is not allowed"); 
                }
            }
        };
    </script>
    <script type="text/javascript" src="js/include.js"></script>
</head>
<body>

    <!----- uploader div start ------->
    <div class="uploader">
        <form action="dialog.aspx?cmd=upload" id="myAwesomeDropzone" class="dropzone">
            <input type="hidden" name="folder" value="<% Response.Write(this.strCurrPath); %>" />
            <div class="fallback">
                <input name="file" type="file" multiple />
            </div>
        </form>
        <center>
            <button class="btn btn-large btn-primary close-uploader"><i class="icon-backward icon-white"></i>Return to files list</button></center>
        <div class="space10"></div>
        <div class="space10"></div>
    </div>
    <!----- uploader div end ------->

    <div class="container-fluid">


        <!----- header div start ------->
        <div class="filters">
            <% if (TinyFileManager.NET.clsConfig.boolAllowUploadFile)
               { %>
            <button class="btn btn-primary upload-btn" style="margin-left: 5px;"><i class="icon-upload icon-white"></i><%Response.Write(TinyFileManager.NET.clsConfig.objLocalizationService.GetValue("Upload a file")); %></button>
            <% } %>
            <% if (TinyFileManager.NET.clsConfig.boolAllowCreateFolder)
               { %>
            <button class="btn new-folder" style="margin-left: 5px;"><i class="icon-folder-open"></i><% Response.Write(TinyFileManager.NET.clsConfig.objLocalizationService.GetValue("New Folder")); %></button>
            <% } %>

            <div class="pull-right">
                <% if ((Convert.ToInt32(this.strType) != 1) && (Convert.ToInt32(this.strType) < 3))
                   { // not only image or only video %>
                <%Response.Write(TinyFileManager.NET.clsConfig.objLocalizationService.GetValue("Filter")); %>: &nbsp;&nbsp;
                <input id="select-type-all" name="radio-sort" type="radio" data-item="ff-item-type-all" class="hide" />
                <label id="ff-item-type-all" for="select-type-all" class="btn btn-info ff-label-type-all"><%Response.Write(TinyFileManager.NET.clsConfig.objLocalizationService.GetValue("All")); %></label>
                &nbsp;
                    <input id="select-type-1" name="radio-sort" type="radio" data-item="ff-item-type-1" checked="checked" class="hide" />
                <label id="ff-item-type-1" for="select-type-1" class="btn ff-label-type-1"><%Response.Write(TinyFileManager.NET.clsConfig.objLocalizationService.GetValue("Files")); %></label>
                &nbsp;
                    <input id="select-type-2" name="radio-sort" type="radio" data-item="ff-item-type-2" class="hide" />
                <label id="ff-item-type-2" for="select-type-2" class="btn ff-label-type-2"><%Response.Write(TinyFileManager.NET.clsConfig.objLocalizationService.GetValue("Images")); %></label>
                &nbsp;
                    <input id="select-type-3" name="radio-sort" type="radio" data-item="ff-item-type-3" class="hide" />
                <label id="ff-item-type-3" for="select-type-3" class="btn ff-label-type-3"><%Response.Write(TinyFileManager.NET.clsConfig.objLocalizationService.GetValue("Archives")); %></label>
                &nbsp;
                    <input id="select-type-4" name="radio-sort" type="radio" data-item="ff-item-type-4" class="hide" />
                <label id="ff-item-type-4" for="select-type-4" class="btn ff-label-type-4"><%Response.Write(TinyFileManager.NET.clsConfig.objLocalizationService.GetValue("Videos")); %></label>
                &nbsp;
                    <input id="select-type-5" name="radio-sort" type="radio" data-item="ff-item-type-5" class="hide" />
                <label id="ff-item-type-5" for="select-type-5" class="btn ff-label-type-5"><%Response.Write(TinyFileManager.NET.clsConfig.objLocalizationService.GetValue("Music")); %></label>
                <% } %>

                <% if (TinyFileManager.NET.clsConfig.boolAllowSearch)
                   {%>
                <form style="display: inline;" method="post" action="<% Response.Write(this.strCurrLink + "&currpath=" + this.strCurrPath + "&cmd=search"); %>">
                    &nbsp;
                        <div class="verticalLine"></div>
                    &nbsp; 
                        <input class="span2 search" type="text" name="query" placeholder="<%Response.Write(TinyFileManager.NET.clsConfig.objLocalizationService.GetValue("Search")); %>" />
                    <button type="submit" class="btn" style="margin-left: 5px; height: 100%;" title="<%Response.Write(TinyFileManager.NET.clsConfig.objLocalizationService.GetValue("Search")); %>"><i class="icon-search icon-fit"></i></button>
                </form>
                <% } %>
            </div>
        </div>
        <!----- header div end ------->

        <!----- breadcrumb div start ------->
        <div class="row-fluid">
            <ul class="breadcrumb">
                <%= this.getBreadCrumb() %>
            </ul>
        </div>
        <!----- breadcrumb div end ------->

        <div class="row-fluid ff-container">
            <div class="span12 pull-right">
                <ul class="thumbnails ff-items">

                    <%  
                        // loop through folder/file list that we have already created
                        foreach (TinyFileManager.NET.clsFileItem objF in this.arrLinks)
                        {
                            //get start of line html, if necessary
                            Response.Write(this.getStartOfLine(objF.intColNum));

                            // start of item
                            Response.Write("<li class=\"span2 ff-item-type-" + objF.strClassType + "\">");
                            Response.Write("<div class=\"boxes thumbnail\">");

                            if (objF.boolIsFolder)
                            {
                                // if folder
                                Response.Write(objF.strDeleteLink);
                                Response.Write(objF.strLink);
                            }
                            else
                            {
                                // if file
                                Response.Write(objF.strDownFormOpen);
                                Response.Write("<div class=\"btn-group toolbox\">");
                                Response.Write("<button type=\"submit\" title=\"" + TinyFileManager.NET.clsConfig.objLocalizationService.GetValue("Download") + "\" class=\"btn\"><i class=\"icon-download\"></i></button>");
                                Response.Write(objF.strPreviewLink);
                                Response.Write(objF.strDeleteLink);
                                Response.Write("</div>");
                                Response.Write("</form>");
                                Response.Write(objF.strLink);
                            }

                            // end of item
                            Response.Write("</div>");
                            Response.Write("</li>");

                            //get end of line html, if necessary
                            Response.Write(this.getEndOfLine(objF.intColNum));
                        }
                    %>
                </ul>
            </div>
        </div>
    </div>

    <!----- lightbox div end ------->
    <div id="previewLightbox" class="lightbox hide fade" tabindex="-1" role="dialog" aria-hidden="true">
        <div class='lightbox-content'>
            <img id="full-img" src="">
        </div>
    </div>
    <!----- lightbox div end ------->
</body>
</html>
