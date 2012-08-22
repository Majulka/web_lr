using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Telerik.Web.Mvc.UI;

namespace Web_LR.Controllers
{
    public class ImageBrowserController : EditorFileBrowserController
    {
        private const string contentFolderRoot = "~/Content/";
        private const string prettyName = "images/";
        private static readonly string[] foldersToCopy = new[] { "~/Content/images" };
        /// <summary>
        /// Gets the base paths from which content will be served.
        /// </summary>
        public override string[] ContentPaths
        {
            get
            {
                return new[] { CreateUserFolder() };
            }
        }
        private string UserID
        {
            get
            {
                var obj = Session["UserID"];
                if (obj == null)
                {
                    Session["UserID"] = obj = DateTime.Now.Ticks.ToString();
                }
                return (string)obj;
            }
        }

        private string CreateUserFolder()
        {
            //var userFolder = Path.Combine("UserFiles", UserID);
            //var userFolder = "images";
            //var virtualPath = Path.Combine(contentFolderRoot, userFolder, prettyName);
            var virtualPath = Path.Combine(contentFolderRoot, prettyName);
            var path = Server.MapPath(virtualPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                foreach (var sourceFolder in foldersToCopy)
                {
                    CopyFolder(Server.MapPath(sourceFolder), path);
                }
            }
            return virtualPath;
        }
        private void CopyFolder(string source, string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }
            foreach (var file in Directory.EnumerateFiles(source))
            {
                var dest = Path.Combine(destination, Path.GetFileName(file));
                System.IO.File.Copy(file, dest);
            }
            foreach (var folder in Directory.EnumerateDirectories(source))
            {
                var dest = Path.Combine(destination, Path.GetFileName(folder));
                CopyFolder(folder, dest);
            }
        }
    }
}
