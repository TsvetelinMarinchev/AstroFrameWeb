using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Data.Models.ViewModels
{
    public class PlanetCreateViewModel
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Range(0.1, 1000)]
        public double Mass { get; set; }

        [Range(0.1, 1000)]
        public double Radius { get; set; }

        [Range(0, 1000000)]
        public double DistanceFromEarth { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public int StarId { get; set; }

        [Required]
        public int GalaxyId { get; set; }

        public IEnumerable<SelectListItem> Stars { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Galaxies { get; set; } = new List<SelectListItem>();
    }
}
