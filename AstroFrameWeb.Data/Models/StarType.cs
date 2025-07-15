using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Data.Models
{
    using static Common.ValidationConstants.StarType;
    public class StarType
    {

        [Key]
        [Comment("PK for StarType")]
        public int Id { get; set; }

        [Required]
        [MaxLength(StarTypeMaxLenght)]
        [MinLength(StarTypeMinLenght)]
        [Comment("The type of StarType")]
        public string Name { get; set; } = null!;


        [Display(Name = "Image URL")]
        [Comment("Local image file for the TypeOfStar")]
        public string? ImageUrl { get; set; }


        [MaxLength(DescriptionStarTypeMaxLength)]
        [Comment("Optional describe for StarType")]
        public string? Description  { get; set; }


        [Comment("List of stars that belong to this StarType")]
        public ICollection<Star>Stars { get; set; }
            = new HashSet<Star>();



    }
}
