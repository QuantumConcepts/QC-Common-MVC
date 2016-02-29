using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using QuantumConcepts.Common.Extensions;

namespace QuantumConcepts.Common.Mvc.Models.ImageGallery
{
    [XmlRoot("Album")]
    public class AlbumModel
    {
        [XmlAttribute]
        public string ID { get; set; }

        [XmlAttribute]
        public string Title { get; set; }

        [XmlAttribute]
        public string Description { get; set; }

        [XmlArray]
        [XmlArrayItem("Photo", typeof(PhotoModel))]
        public List<PhotoModel> Photos { get; set; }

        public AlbumModel() { }

        public AlbumModel(string id, string title, string description)
        {
            this.ID = id;
            this.Title = (title.IsNullOrEmpty() ? null : title);
            this.Description = (description.IsNullOrEmpty() ? null : description);
        }

        public static AlbumModel FromXml(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AlbumModel));
            AlbumModel album = null;

            try
            {
                using (Stream stream = File.OpenRead(path))
                {
                    album = (AlbumModel)serializer.Deserialize(stream);
                }
            }
            catch
            {
                return null;
            }

            if (album != null)
                album.Photos.ForEach(o => o.Album = album);

            return album;
        }
    }
}