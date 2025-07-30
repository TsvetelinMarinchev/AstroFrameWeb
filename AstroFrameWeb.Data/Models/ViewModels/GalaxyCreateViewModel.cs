using AstroFrameWeb.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Data.Models.ViewModels
{
    public class GalaxyCreateViewModel
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public GalaxyType GalaxyType { get; set; }

        [Required]
        [Range(1, 100000)]
        public int NumberOfStars { get; set; }

        [Required]
        [Range(0.1, 1000000)]
        public double DistanceFromEarth { get; set; }
        public string? ImageUrl { get; set; }
    }
}

