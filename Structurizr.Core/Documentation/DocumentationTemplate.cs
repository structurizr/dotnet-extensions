using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Structurizr.Util;

namespace Structurizr.Documentation
{
    
    /// <summary>
    /// The superclass for all documentation templates.
    /// </summary>
    public abstract class DocumentationTemplate
    {
        
        private readonly Documentation _documentation;

        /// <summary>
        /// Creates a new documentation template for the given workspace.
        /// </summary>
        /// <param name="workspace">the Workspace instance to create documentation for</param>
        public DocumentationTemplate(Workspace workspace)
        {
            if (workspace == null) {
                throw new ArgumentException("A workspace must be specified.");
            }

            _documentation = workspace.Documentation;
        }
        
        private FormattedContent ReadFiles(params FileSystemInfo[] files)
        {
            if (files == null || files.Length == 0)
            {
                throw new ArgumentException("One or more files must be specified.");
            }

            Format format = Format.Markdown;
            StringBuilder content = new StringBuilder();
            foreach (FileSystemInfo file in files)
            {
                if (file == null)
                {
                    throw new ArgumentException("One or more files must be specified.");
                }

                if (!File.Exists(file.FullName) && !Directory.Exists(file.FullName))
                {
                    throw new ArgumentException(file.FullName + " does not exist.");
                }

                if (content.Length > 0)
                {
                    content.Append(Environment.NewLine);
                }

                if (File.Exists(file.FullName))
                {
                    format = FormatFinder.FindFormat(file);
                    string contentInFile = File.ReadAllText(file.FullName, Encoding.UTF8);
                    content.Append(contentInFile);
                }
                else if (Directory.Exists(file.FullName))
                {
                    DirectoryInfo directory = new DirectoryInfo(file.FullName);
                    FileSystemInfo[] children = directory.GetFileSystemInfos();
                    Array.Sort(children, (f1, f2) => f1.Name.CompareTo(f2.Name));
                    content.Append(ReadFiles(children).Content);
                }
            }

            return new FormattedContent(content.ToString(), format);
        }

        /// <summary>
        /// Adds a custom section from a file, that isn't related to any element in the model.
        /// </summary>
        /// <param name="name">the name of the section</param>
        /// <param name="files">one or more FileInfo objects that point to the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddSection(string name, params FileInfo[] files)
        {
            return Add(null, name, files);
        }
    
        /// <summary>
        /// Adds a custom section, that isn't related to any element in the model.
        /// </summary>
        /// <param name="name">the name of the section</param>
        /// <param name="format">the format of the documentation content</param>
        /// <param name="content">a string containing the documentation content</param>
        /// <returns>a documentation section</returns>
        public Section AddSection(string name, Format format, string content)
        {
            return Add(null, name, format, content);
        }
    
        /// <summary>
        /// Adds a custom section relating to a SoftwareSystem from one or more files.
        /// </summary>
        /// <param name="softwareSystem">the SoftwareSystem the documentation content relates to</param>
        /// <param name="name">the name of the section</param>
        /// <param name="files">one or more FileInfo objects that point to the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddSection(SoftwareSystem softwareSystem, string name, params FileSystemInfo[] files)
        {
            return Add(softwareSystem, name, files);
        }
    
        /// <summary>
        /// Adds a custom section relating to a SoftwareSystem.
        /// </summary>
        /// <param name="softwareSystem">the SoftwareSystem the documentation content relates to</param>
        /// <param name="name">the name of the section</param>
        /// <param name="format">the format of the documentation content</param>
        /// <param name="content">a string containing the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddSection(SoftwareSystem softwareSystem, string name, Format format, String content)
        {
            return Add(softwareSystem, name, format, content);
        }

        /// <summary>
        /// Adds a custom section relating to a Container from one or more files.
        /// </summary>
        /// <param name="container">the Container the documentation content relates to</param>
        /// <param name="name">the name of the section</param>
        /// <param name="files">one or more FileInfo objects that point to the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddSection(Container container, string name, params FileSystemInfo[] files)
        {
            return Add(container, name, files);
        }
    
        /// <summary>
        /// Adds a custom section relating to a Container.
        /// </summary>
        /// <param name="container">the Container the documentation content relates to</param>
        /// <param name="name">the name of the section</param>
        /// <param name="format">the format of the documentation content</param>
        /// <param name="content">a string containing the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddSection(Container container, string name, Format format, String content)
        {
            return Add(container, name, format, content);
        }

        /// <summary>
        /// Adds a custom section relating to a Component from one or more files.
        /// </summary>
        /// <param name="component">the Component the documentation content relates to</param>
        /// <param name="name">the name of the section</param>
        /// <param name="files">one or more FileInfo objects that point to the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddSection(Component component, string name, params FileSystemInfo[] files)
        {
            return Add(component, name, files);
        }
    
        /// <summary>
        /// Adds a custom section relating to a Component.
        /// </summary>
        /// <param name="component">the Component the documentation content relates to</param>
        /// <param name="name">the name of the section</param>
        /// <param name="format">the format of the documentation content</param>
        /// <param name="content">a string containing the documentation content</param>
        /// <returns>a documentation Section</returns>
        public Section AddSection(Component component, string name, Format format, String content)
        {
            return Add(component, name, format, content);
        }

        private Section Add(Element element, string type, params FileSystemInfo[] files)
        {
            FormattedContent content = ReadFiles(files);
            return _documentation.AddSection(element, type, content.Format, content.Content);
        }

        private Section Add(Element element, string type, Format format, string content) {
            return _documentation.AddSection(element, type, format, content);
        }

        /// <summary>
        /// Adds png/jpg/jpeg/gif images in the given directory to the workspace.
        /// </summary>
        /// <param name="directory">a DirectoryInfo representing the directory containing image files</param>
        public IEnumerable<Image> AddImages(DirectoryInfo directory)
        {
            if (directory == null)
            {
                throw new ArgumentException("Directory path must not be null.");
            }
            
            if (File.Exists(directory.FullName))
            {
                throw new ArgumentException(directory.FullName + " is not a directory.");
            }
            
            if (Directory.Exists(directory.FullName))
            {
                return AddImagesFromPath("", directory);
            }
            else
            {
                throw new ArgumentException(directory.FullName + " does not exist.");
            }
        }

        private IEnumerable<Image> AddImagesFromPath(string root, DirectoryInfo directory)
        {
            List<Image> images = new List<Image>();
            
            images.AddRange(AddImagesFromPath(root, directory, "*.png"));
            images.AddRange(AddImagesFromPath(root, directory, "*.jpg"));
            images.AddRange(AddImagesFromPath(root, directory, "*.jpeg"));
            images.AddRange(AddImagesFromPath(root, directory, "*.gif"));

            foreach (string directoryName in Directory.EnumerateDirectories(directory.FullName))
            {
                images.AddRange(AddImagesFromPath(new FileInfo(directoryName).Name + "/", new DirectoryInfo(directoryName)));
            }

            return images;
        }

        private IEnumerable<Image> AddImagesFromPath(string root, DirectoryInfo directory, string fileExtension)
        {
            List<Image> images = new List<Image>();

            foreach (string fileName in Directory.EnumerateFiles(directory.FullName, fileExtension, SearchOption.TopDirectoryOnly))
            {
                Image image = AddImage(new FileInfo(fileName));

                if (!String.IsNullOrEmpty(root))
                {
                    image.Name = root + image.Name;
                }
                
                images.Add(image);
            }

            return images;
        }

        /// <summary>
        /// Adds an image from the given file to the workspace.
        /// </summary>
        /// <param name="file">a FileInfo representing the image file on disk</param>
        /// <returns>an Image object representing the image added</returns>
        public Image AddImage(FileInfo file)
        {
            string contentType = ImageUtils.GetContentType(file);
            string base64String = ImageUtils.GetImageAsBase64(file);

            Image image = new Image(file.Name, base64String, contentType);
            _documentation.Add(image);

            return image;
        }

    }
    
}