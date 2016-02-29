using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using QuantumConcepts.Common.Extensions;
using QuantumConcepts.Common.Mvc.Controllers;
using QuantumConcepts.Common.Mvc.Models.ImageGallery;

namespace QuantumConcepts.Common.Mvc.Models.ImageGallery
{
    public class IndexModel
    {
        protected virtual ImageGalleryController GetImageGalleryController()
        {
            throw new NotImplementedException();
        }

        public List<AlbumModel> Albums { get; private set; }

        public IndexModel()
        {
            ImageGalleryController portfolio = GetImageGalleryController();

            this.Albums = portfolio.GetAllAlbums().ToList();
        }
    }
}