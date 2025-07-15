using AstroFrameWeb.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Data.Models
{
    using static Common.ValidationConstants.Galaxy;
    public class Galaxy
    {
        [Key]
        [Comment("Unique identifier for Galaxy")]
        public int Id { get; set; }

        [Required]
        [MaxLength(GalaxyMaxLength)]
        [MinLength(GalaxyMinLength)]
        [Display(Name = "Galaxy Name")]
        [Comment("The name of the Galaxy")]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(GalaxyDescriptionMaxLength)]
        [Display(Name = "Description")]
        [Comment("Description of the Galaxy")]
        public string Description { get; set; } = null!;

        [Required]
        [Display(Name = "Galaxy Type")]
        [Comment("The type of Galaxy")]
        public GalaxyType GalaxyType { get; set; }

        [Range(1, NumberOfStarsInGalaxy)]
        [Display(Name = "Number of Stars")]
        [Comment("Total number of stars in the Galaxy")]
        public int NumberOfStars { get; set; }

        [Range(0,Distance)]
        [Display(Name = "Distance from Earth (light years)")]
        [Comment("Distance of the Galaxy from Earth in light years")]
        public double DistanceFromEarth { get; set; }

        [Display(Name = "Discovered On")]
        [Comment("Date when the Galaxy was discovered or added")]
        public DateTime DiscoveredOn { get; set; } = DateTime.UtcNow;

        [Display(Name = "Image URL")]
        [Comment("Local image file for the Galaxy")]
        public string? ImageUrl { get; set; }

        public string? DiscoveredAgo { get; set; }

        [Display(Name = "Creator Id")]
        [Comment("FK to the user who created this Galaxy")]
        public string? CreatorId { get; set; }

        [ForeignKey(nameof(CreatorId))]
        [Display(Name = "Creator")]
        [Comment("Navigation property to the creator")]
        public ApplicationUser? Creator { get; set; }

        [Comment("Stars that belong to this Galaxy")]
        public ICollection<Star> Stars { get; set; } 
            = new HashSet<Star>();
    }
}

