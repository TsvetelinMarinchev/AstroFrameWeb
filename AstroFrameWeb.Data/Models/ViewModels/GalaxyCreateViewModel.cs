using AstroFrameWeb.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Data.Models.ViewModels
{
    using static AstroFrameWeb.Common.ValidationConstants.GalaxyCreateViewModel;
    public class GalaxyCreateViewModel
    {
        [Required]
        [MinLength(NameMinLenghtGalaxyCreateViewModel, ErrorMessage = ErrorMessage)]
        [MaxLength(NameMaxLenghtGalaxyCreateViewModel, ErrorMessage = ErrorMessage)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(DescriptionMinLenghtGalaxyCreateViewModel, ErrorMessage = ErrorMessageDis)]
        [MaxLength(DescriptionMaxLenghtGalaxyCreateViewModel, ErrorMessage = ErrorMessageDis)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(GalaxyTypeMaxLenghtGalaxyCreateViewModel, ErrorMessage=ErrorMessageGalaxyType)]
        [MinLength(GalaxyTypeMinLenghtGalaxyCreateViewModel, ErrorMessage=ErrorMessageGalaxyType)]
        public GalaxyType GalaxyType { get; set; }

        [Required]
        [Range(NumberOfStarsMinLenghtGalaxyCreateViewModel, NumberOfStarsMaxLenghtGalaxyCreateViewModel,
                ErrorMessage = ErrorMessageNumberOfStars)]
        public int NumberOfStars { get; set; }

        [Required]
        [Range(0.1, 1000000)]
        public double DistanceFromEarth { get; set; }



        [Display(Name = "Image URL")]
        [Url(ErrorMessage = "Invalid URL")]
        public string? ImageUrl { get; set; }
    }
}

