using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using QuantumConcepts.Common.Mvc.Models.ImageGallery;
using QuantumConcepts.Common.Extensions;

namespace QuantumConcepts.Common.Mvc.Controllers
{
    public class ImageGalleryController : Controller
    {
        public virtual int ThumbnailWidth { get { return 125; } }
        public virtual int ThumbnailHeight { get { return 125; } }
        public virtual string RootPath { get { throw new NotImplementedException(); } }

        public IEnumerable<AlbumModel> GetAllAlbums()
        {
            List<AlbumModel> albums = new List<AlbumModel>();

            if (Directory.Exists(this.RootPath))
                foreach (string albumPath in Directory.GetFiles(this.RootPath, "Album.xml", SearchOption.AllDirectories))
                    albums.Add(AlbumModel.FromXml(albumPath));

            return albums.NonNull();
        }

        public FileResult PhotoThumb(string id)
        {
            int width, height;
            string filename;
            byte[] photoData = SizePhoto(id, this.ThumbnailWidth, this.ThumbnailHeight, ImageFormat.Png, out width, out height, out filename);

            //Send the thumbnail.
            return this.File(photoData, MimeMapping.GetMimeMapping(filename), filename);
        }

        public FileResult PhotoSized(string id, int? maxWidth, int? maxHeight)
        {
            int width, height;
            string filename;
            byte[] photoData = SizePhoto(id, maxWidth, maxHeight, ImageFormat.Jpeg, out width, out height, out filename);

            return this.File(photoData, MimeMapping.GetMimeMapping(filename), filename);
        }

        private byte[] SizePhoto(string id, int? maxWidth, int? maxHeight, ImageFormat format)
        {
            int width, height;
            string filename;

            return SizePhoto(id, maxWidth, maxHeight, format, out width, out height, out filename);
        }

        private byte[] SizePhoto(string id, int? maxWidth, int? maxHeight, ImageFormat format, out int width, out int height, out string filename)
        {
            FileInfo sourceFileInfo = new FileInfo(GetPhotoPath(id));
            Bitmap output = null;

            using (Bitmap source = (Bitmap)Bitmap.FromFile(sourceFileInfo.FullName))
            {
                int scaledWidth, scaledHeight;

                width = (maxWidth.HasValue && maxWidth.Value < source.Width ? maxWidth.Value : source.Width);
                height = (maxHeight.HasValue && maxHeight.Value < source.Height ? maxHeight.Value : source.Height);

                //Default the scaled width to the width/height.
                scaledWidth = width;
                scaledHeight = height;

                //If the width was provided but height was not, then constrain the height.
                //If the height was provided but the width was not, then constrain the width;
                if (width != source.Width)
                    scaledHeight = (int)Math.Floor((width / (decimal)source.Width) * source.Height);
                else if (height != source.Height)
                    scaledWidth = (int)Math.Floor((height / (decimal)source.Height) * source.Width);

                //If the maxWidth was not provided, revert to the scaledWidth.
                if (!maxWidth.HasValue)
                    width = scaledWidth;

                //If the maxHeight was not provided, revert to the scaledHeight.
                if (!maxHeight.HasValue)
                    height = scaledHeight;

                //Finally figued out the dimensions, now check if this photo needs to be resized or not.
                if (width == source.Width && height == source.Height)
                {
                    filename = sourceFileInfo.Name;
                    output = new Bitmap(source);
                }
                else
                {
                    //Need a different size; check if it has already been cached.
                    FileInfo scaledFileInfo = new FileInfo(GetPhotoPath(id, width, height));

                    //If the photo has not been cached or it is stale (older than the source photo), then re-generate the file.
                    if (!System.IO.File.Exists(scaledFileInfo.FullName) || scaledFileInfo.LastWriteTime < sourceFileInfo.LastWriteTime)
                    {
                        //Scale the source and then draw it onto the sized Bitmap.
                        using (Bitmap scaled = (Bitmap)new Bitmap(source, new Size(scaledWidth, scaledHeight)),
                                      sized = (Bitmap)new Bitmap(width, height))
                        {
                            using (Graphics g = Graphics.FromImage(sized))
                            {
                                //Crop the photo to the specified size.
                                g.DrawImage(scaled, new Point(0, 0));

                                using (Stream stream = scaledFileInfo.Create())
                                {
                                    sized.Save(stream, format);
                                }
                            }
                        }
                    }

                    //Finally, load the scaled photo.
                    filename = scaledFileInfo.Name;
                    output = new Bitmap(scaledFileInfo.FullName);
                }
            }

            using (MemoryStream stream = new MemoryStream())
            {
                output.Save(stream, format);
                output.Dispose();

                return stream.ToArray();
            }
        }

        public ActionResult RandomPhoto()
        {
            List<AlbumModel> allAlbums = GetAllAlbums().ToList();
            List<PhotoModel> allPhotos = allAlbums.SelectMany(o => o.Photos).ToList();
            Random random = new Random();

            return this.View("ImageGallery/RandomPhoto", allPhotos[random.Next(allPhotos.Count)]);
        }

        public string GetAlbumPath(string id)
        {
            Match match = Regex.Match(id, @"^(?<AlbumID>\d+)");

            return @"{0}{1}\".FormatString(this.RootPath, match.Groups["AlbumID"].Value);
        }

        public string GetAlbumXmlPath(string id)
        {
            return @"{0}Album.xml".FormatString(GetAlbumPath(id), id);
        }

        public string GetPhotoPath(string id)
        {
            return @"{0}{1}.jpg".FormatString(GetAlbumPath(id), id);
        }

        public string GetPhotoPath(string id, int width, int height)
        {
            return @"{0}{1}.{2}x{3}.jpg".FormatString(GetAlbumPath(id), id, width, height);
        }
    }
}
