using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Structurizr.Documentation;
using Xunit;

namespace Structurizr.Core.Tests.Documentation
{

    public class DocumentationTemplateTests : AbstractTestBase
    {

        private const string PngAsBase64 = "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";
        private const string JpegAsBase64 = "/9j/4AAQSkZJRgABAQAASABIAAD/4QCMRXhpZgAATU0AKgAAAAgABQESAAMAAAABAAEAAAEaAAUAAAABAAAASgEbAAUAAAABAAAAUgEoAAMAAAABAAIAAIdpAAQAAAABAAAAWgAAAAAAAABIAAAAAQAAAEgAAAABAAOgAQADAAAAAQABAACgAgAEAAAAAQAAACCgAwAEAAAAAQAAABoAAAAA/+0AOFBob3Rvc2hvcCAzLjAAOEJJTQQEAAAAAAAAOEJJTQQlAAAAAAAQ1B2M2Y8AsgTpgAmY7PhCfv/AABEIABoAIAMBIgACEQEDEQH/xAAfAAABBQEBAQEBAQAAAAAAAAAAAQIDBAUGBwgJCgv/xAC1EAACAQMDAgQDBQUEBAAAAX0BAgMABBEFEiExQQYTUWEHInEUMoGRoQgjQrHBFVLR8CQzYnKCCQoWFxgZGiUmJygpKjQ1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4eLj5OXm5+jp6vHy8/T19vf4+fr/xAAfAQADAQEBAQEBAQEBAAAAAAAAAQIDBAUGBwgJCgv/xAC1EQACAQIEBAMEBwUEBAABAncAAQIDEQQFITEGEkFRB2FxEyIygQgUQpGhscEJIzNS8BVictEKFiQ04SXxFxgZGiYnKCkqNTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqCg4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2dri4+Tl5ufo6ery8/T19vf4+fr/2wBDAAICAgICAgMCAgMEAwMDBAUEBAQEBQcFBQUFBQcIBwcHBwcHCAgICAgICAgKCgoKCgoLCwsLCw0NDQ0NDQ0NDQ3/2wBDAQICAgMDAwYDAwYNCQcJDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ3/3QAEAAL/2gAMAwEAAhEDEQA/APxy8ZeMvEvj/wAS6j4w8X6jcanquqXEt1c3NzK8rFpXZyAXZtqJuwiDCqoAA9bVn8PfHGoW0d5Z6LcPDMoeNi0SblPIIV5AwB9wK4i55tph/wBM3/8AQTX65fAjwhonjDxOtlr0H2m0tNONx5BJVZHBRFDYIJUZJx3OK/YeFshwuPjWliJSjGmov3bdb90+x+V8SZ3icFKjGhFSlUb+K/S3Zrufmh/wq74h/wDQCuP+/kH/AMdq7o0nxN+D3iDTPG+mLf6Bf6ddwzWt7DNtxLG4cIxhkOVfbhkf5XUkV+8v/Cnvhh/0Ldj/AN8N/wDFV+fX7bPgzQfB2keT4ftxa216lncGBSSiSJdohKZJIDAjjoD06162M4Yyz6rVnSnPmjGUlfls7K9nZJnnYXiLMfrNKNSMOWUknbmuru11d+Z//9D8SryCeJJ7eSNllVZEKMNrB1ypGDjBDAgjsRg1+pfwR+J/h/whrEHiG4kF3p97YfZ3a3dGkTdsZWClhnBXDL159q+ff299D0Xw/wDtSeNbHQdPtdNtmuzO0NpCkEZlmZ2kcqgUbnblm6k8mvjdrS0k+d4Y2ZupKAk/jiv1HIOJv7Np1JSpc6qpXV7Wtd72ffsfnWe8OfX6sFGpyum207Xve3S67H78f8NL/DD/AJ63v/gOP/i6+Ff2wPiTo3xHsIk0JXwDZ2lrE+0zzt9pWR2EaFiB0AHUn3IB/O/7DY/8+8X/AHwv+FfZX7BHhzw9rP7UXgqy1fS7K+txeLMIrm3jmj82FkaN9rqRuRuVOMg8iuzFcbUpYarSp4ezlGSu53tdWenKuhzYbhCpHEU51K91GSdlG17O615n1P/Z";
        private const string GifAsBase64 = "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";

        private SoftwareSystem _softwareSystem;
        private Structurizr.Documentation.Documentation _documentation;
        private StructurizrDocumentationTemplate _template;

        public DocumentationTemplateTests()
        {
            _softwareSystem = Model.AddSoftwareSystem("Name", "Description");
            _template = new StructurizrDocumentationTemplate(Workspace);
            _documentation = Workspace.Documentation;
        }

        [Fact]
        public void test_Construction()
        {
            Assert.Equal(0, _documentation.Sections.Count);
            Assert.Equal(0, _documentation.Images.Count);
            Assert.True(_documentation.IsEmpty());
            Assert.Same(Workspace.Model, _documentation.Model);
        }

        [Fact]
        public void Test_AddWithContentForSoftwareSystem_ThrowsAnException_WhenThatSectionAlreadyExists()
        {
            _template.AddContextSection(_softwareSystem, Format.Markdown, "Some Markdown content");
            Assert.Equal(1, _documentation.Sections.Count);

            try
            {
                _template.AddContextSection(_softwareSystem, Format.Markdown, "Some Markdown content");
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                // this is the expected exception
                Assert.Equal("A section with a title of Context already exists for the element named Name.", ae.Message);
                Assert.Equal(1, _documentation.Sections.Count);
            }
        }

        [Fact]
        public void Test_Construction_ThrowsAnException_WhenANullWorkspaceIsSpecified() {
            try {
                new StructurizrDocumentationTemplate(null);
                throw new TestFailedException();
            } catch (ArgumentException e) {
                Assert.Equal("A workspace must be specified.", e.Message);
            }
        }
        
        [Fact]
        public void Test_AddImages_ThrowsAnException_WhenTheSpecifiedDirectoryIsNull()
        {
            try
            {
                _template.AddImages(null);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("Directory path must not be null.", ae.Message);
            }
        }

        [Fact]
        public void Test_AddImages_DoesNothing_WhenThereAreNoImageFilesInTheSpecifiedDirectory()
        {
            DirectoryInfo directory = new DirectoryInfo("Documentation" + Path.DirectorySeparatorChar + "noimages");
            _template.AddImages(directory);
            Assert.Equal(0, _documentation.Images.Count);
        }

        [Fact]
        public void Test_AddImages_ThrowsAnException_WhenTheSpecifiedDirectoryIsNotADirectory()
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo("Documentation" + Path.DirectorySeparatorChar + "example.md");
                _template.AddImages(directory);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.True(ae.Message.EndsWith("example.md is not a directory."));
            }
        }

        [Fact]
        public void Test_AddImages_ThrowsAnException_WhenTheSpecifiedDirectoryDoesNotExist()
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo("Documentation" + Path.DirectorySeparatorChar + "foo");
                _template.AddImages(directory);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.True(ae.Message.EndsWith("foo does not exist."));
            }
        }

        [Fact]
        public void Test_AddImages_AddsAllImagesFromTheSpecifiedDirectory_WhenThereAreImageFilesInTheSpecifiedDirectory()
        {
            Assert.Equal(0, _documentation.Images.Count);
            DirectoryInfo directory = new DirectoryInfo("Documentation" + Path.DirectorySeparatorChar + "images");
            IEnumerable<Image> images = _template.AddImages(directory);
            Assert.Equal(4, _documentation.Images.Count);
            Assert.Equal(4, images.Count());

            Image png = _documentation.Images.Where(i => i.Name.Equals("image.png")).First();
            Assert.Equal("image/png", png.Type);
            Assert.Equal(PngAsBase64, png.Content);
            Assert.True(images.Contains(png));

            Image jpg = _documentation.Images.Where(i => i.Name.Equals("image.jpg")).First();
            Assert.Equal("image/jpeg", jpg.Type);
            Assert.Equal(JpegAsBase64, jpg.Content);
            Assert.True(images.Contains(jpg));

            Image jpeg = _documentation.Images.Where(i => i.Name.Equals("image.jpeg")).First();
            Assert.Equal("image/jpeg", jpeg.Type);
            Assert.Equal(JpegAsBase64, jpeg.Content);
            Assert.True(images.Contains(jpeg));

            Image gif = _documentation.Images.Where(i => i.Name.Equals("image.gif")).First();
            Assert.Equal("image/gif", gif.Type);
            Assert.Equal(GifAsBase64, gif.Content);
            Assert.True(images.Contains(gif));
        }

        [Fact]
        public void Test_AddImages_AddsAllImagesFromTheSpecifiedDirectory_WhenThereAreImageFilesInTheSpecifiedDirectoryAndSubDirectories()
        {
            Assert.Equal(0, _documentation.Images.Count);
            _template.AddImages(new DirectoryInfo("Documentation"));
            Assert.Equal(8, _documentation.Images.Count);

            Image pngInDirectory = _documentation.Images.Where(i => i.Name.Equals("image.png")).First();
            Assert.Equal("image/png", pngInDirectory.Type);
            Assert.Equal(PngAsBase64, pngInDirectory.Content);

            Image jpgInDirectory = _documentation.Images.Where(i => i.Name.Equals("image.jpg")).First();
            Assert.Equal("image/jpeg", jpgInDirectory.Type);
            Assert.Equal(JpegAsBase64, jpgInDirectory.Content);

            Image jpegInDirectory = _documentation.Images.Where(i => i.Name.Equals("image.jpeg")).First();
            Assert.Equal("image/jpeg", jpegInDirectory.Type);
            Assert.Equal(JpegAsBase64, jpegInDirectory.Content);

            Image gifInDirectory = _documentation.Images.Where(i => i.Name.Equals("image.gif")).First();
            Assert.Equal("image/gif", gifInDirectory.Type);
            Assert.Equal(GifAsBase64, gifInDirectory.Content);

            Image pngInSubdirectory = _documentation.Images.Where(i => i.Name.Equals("image.png")).First();
            Assert.Equal("image/png", pngInSubdirectory.Type);
            Assert.Equal(PngAsBase64, pngInSubdirectory.Content);

            Image jpgInSubdirectory = _documentation.Images.Where(i => i.Name.Equals("image.jpg")).First();
            Assert.Equal("image/jpeg", jpgInSubdirectory.Type);
            Assert.Equal(JpegAsBase64, jpgInSubdirectory.Content);

            Image jpegInSubdirectory = _documentation.Images.Where(i => i.Name.Equals("image.jpeg")).First();
            Assert.Equal("image/jpeg", jpegInSubdirectory.Type);
            Assert.Equal(JpegAsBase64, jpegInSubdirectory.Content);

            Image gifInSubdirectory = _documentation.Images.Where(i => i.Name.Equals("image.gif")).First();
            Assert.Equal("image/gif", gifInSubdirectory.Type);
            Assert.Equal(GifAsBase64, gifInSubdirectory.Content);
        }

        [Fact]
        public void Test_AddImage_AddsTheSpecifiedImage_WhenTheSpecifiedFileExists()
        {
            Assert.Equal(0, _documentation.Images.Count);
            _template.AddImage(new FileInfo("Documentation" + Path.DirectorySeparatorChar + "image.png"));
            Assert.Equal(1, _documentation.Images.Count);

            Image png = _documentation.Images.Where(i => i.Name.Equals("image.png")).First();
            Assert.Equal("image/png", png.Type);
            Assert.Equal(PngAsBase64, png.Content);
        }

        [Fact]
        public void Test_AddImage_ThrowsAnException_WhenTheSpecifiedFileIsNull()
        {
            try
            {
                _template.AddImage(null);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("A file must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_AddImage_ThrowsAnException_WhenTheSpecifiedFileIsNotAFile()
        {
            try
            {
                _template.AddImage(new FileInfo("Documentation"));
            }
            catch (ArgumentException ae)
            {
                Assert.True(ae.Message.EndsWith("Documentation is not a file."));
            }
        }

        [Fact]
        public void Test_AddImage_ThrowsAnException_WhenTheSpecifiedFileDoesNotExist()
        {
            try
            {
                _template.AddImage(new FileInfo("Documentation" + Path.DirectorySeparatorChar + "some-other-image.png"));
            }
            catch (ArgumentException ae)
            {
                Assert.True(ae.Message.EndsWith("Documentation" + Path.DirectorySeparatorChar + "some-other-image.png does not exist."));
            }
        }

        [Fact]
        public void Test_AddImage_ThrowsAnException_WhenTheSpecifiedFileIsNotAnImage()
        {
            try
            {
                _template.AddImage(new FileInfo("Documentation" + Path.DirectorySeparatorChar + "example.md"));
            }
            catch (ArgumentException ae)
            {
                Assert.True(ae.Message.EndsWith("Documentation" + Path.DirectorySeparatorChar + "example.md is not a supported image file."));
            }
        }
        
        [Fact]
        public void Test_ReadFiles_ThrowsAnException_WhenPassedAFileThatDoesNotExist() {
            try
            {
                _template.AddContextSection(_softwareSystem, new FileInfo("foo.txt"));
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.True(ae.Message.EndsWith("foo.txt does not exist."));
            }
        }

        [Fact]
        public void Test_ReadFiles_ThrowsAnException_WhenPassedANullFile()
        {
            try
            {
                _template.AddContextSection(_softwareSystem, (FileInfo)null);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("One or more files must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_ReadFiles_ThrowsAnException_WhenPassedAnEmptyArray()
        {
            try
            {
                _template.AddContextSection(_softwareSystem, new FileInfo[]{});
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("One or more files must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_ReadFiles_AddsAllFiles_WhenPassedADirectory()
        {
            Section section = _template.AddContextSection(_softwareSystem, new FileInfo("Documentation" + Path.DirectorySeparatorChar + "markdown"));
            Assert.Equal("File 1" + Environment.NewLine +
                    "File 2", section.Content);
        }

    }
}
