using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AstroFrameWeb.Data.Models
{
    using static Common.ValidationConstants.Planet;
    public class Planet
    {
        [Key]
        [Comment("Unique identifier for Planet")]
        public int Id { get; set; }

        [Required]
        [MaxLength(PlanetNameMaxLength)]
        [MinLength(PlanetNameMinLength)]
        [Display(Name = "Planet Name")]
        [Comment("The name of the planet")]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(PlanetDescriptionMaxLength)]
        [Display(Name = "Description")]
        [Comment("Detailed description of the planet")]
        public string Description { get; set; } = null!;

        [Range(0.1, PlanetMaxMass)]
        [Display(Name = "Mass (Earth masses)")]
        [Comment("Mass of the planet in Earth masses")]
        public double Mass { get; set; }

        [Range(0.1, PlanetMaxRadius)]
        [Display(Name = "Radius (Earth radii)")]
        [Comment("Radius of the planet in Earth radiius")]
        public double Radius { get; set; }

        [Range(0, PlanetMaxDistance)]
        [Display(Name = "Distance from Earth (light years)")]
        [Comment("Distance of the planet from Earth in light years")]
        public double DistanceFromEarth { get; set; }

        [Display(Name = "Discovered On")]
        [Comment("Discovery date")]
        public DateTime DiscoveredOn { get; set; }
            = DateTime.UtcNow;

        public string? DiscoveredAgo { get; set; }
        [Display(Name = "Image URL")]
        [Comment("Local image file for the Planet")]
        public string? ImageUrl { get; set; }


        [Required]
        [Display(Name = "Star Id")]
        [Comment("FK to the Star")]
        public int StarId { get; set; }

        [ForeignKey(nameof(StarId))]
        [Display(Name = "Star")]
        [Comment("The star this planet orbits")]
        [ValidateNever]
        public Star Star { get; set; } = null!;

        
        [Display(Name = "Galaxy Id")]
        [Comment("FK to the Galaxy")]
        public int? GalaxyId { get; set; }

        [ForeignKey(nameof(GalaxyId))]
        [Display(Name = "Galaxy")]
        [Comment("The galaxy this planet belongs to")]
        [ValidateNever]
        public Galaxy? Galaxy { get; set; }

        
        [Display(Name = "Creator Id")]
        [Comment("FK to the creator")]
        public string? CreatorId { get; set; }

        [ForeignKey(nameof(CreatorId))]
        [Display(Name = "Creator")]
        [ValidateNever]
        public ApplicationUser? Creator { get; set; }

        [ValidateNever]
        public ICollection<UserFavoritePlanet> FavoritedByUsers { get; set; }
              = new HashSet<UserFavoritePlanet>();

        [Comment("Comments on the planet")]
        [ValidateNever]
        public ICollection<PlanetComment> Comments { get; set; }
            = new HashSet<PlanetComment>();
    }
}
