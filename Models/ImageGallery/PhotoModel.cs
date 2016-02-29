using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using QuantumConcepts.Common.Extensions;

namespace QuantumConcepts.Common.Mvc.Models.ImageGallery
{
    [XmlRoot("Photo")]
    public class PhotoModel
    {
        [XmlAttribute]
        public string ID { get; set; }

        [XmlAttribute]
        public string Title { get; set; }

        [XmlAttribute]
        public string Description { get; set; }

        [XmlIgnore]
        public AlbumModel Album { get; set; }

        public PhotoModel() { }

        public PhotoModel(string id, string title, string description)
        {
            this.ID = id;
            this.Title = (title.IsNullOrEmpty() ? null : title);
            this.Description = (description.IsNullOrEmpty() ? null : description);
        }
    }
}