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
    using static Common.ValidationConstants.Star;
    public class Star
    {
        [Key]
        [Comment("Unique identifier")]
        public int Id { get; set; }

        [Required]
        [MaxLength(StarMaxLength)]
        [MinLength(StarMinLength)]
        [Display(Name = "Star Name")]
        [Comment("Shows name of the star")]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(StarDescriptionMaxLength)]
        [Display(Name = "Star Description")]
        [Comment("Information for Star")]
        public string Description { get; set; } = null!;

        [Required]
        [Display(Name = "Star Price")]
        [Column(TypeName = "decimal(18,2)")]
        [Range((double)StarMinPrice, (double)StarMaxPrice,
             ErrorMessage = "Price must be positive.")]
        [Comment("Shows the price of the Star")]
        public decimal Price { get; set; }

        [Display(Name = "Is Purchased")]
        [Comment("Shows the star if it has been purchased")]
        public bool IsPurchased { get; set; } = false;

        [Display(Name = "Created On")]
        [Comment("Shows real time now")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

     
        [Display(Name = "Owner Id")]
        [Comment("FK to the user who owns the star")]
        public string? OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [Display(Name = "Owner")]
        [Comment("Navigation property to the user who owns the star")]
        public ApplicationUser? Owner { get; set; }

        
        [Required]
        [Comment("Galaxy the star belongs to")]
        public int GalaxyId { get; set; }

        [ForeignKey(nameof(GalaxyId))]
        public Galaxy Galaxy { get; set; } = null!;

        [Comment("List of planets orbiting this star")]
        public ICollection<Planet> Planets { get; set; } 
            = new HashSet<Planet>();

        [Comment("User comments on this star")]
        public ICollection<StarComment> Comments { get; set; } 
            = new HashSet<StarComment>();
    }
}
