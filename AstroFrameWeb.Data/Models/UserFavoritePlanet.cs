using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Data.Models
{
    public class UserFavoritePlanet
    {

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public int PlanetId { get; set; }

        [ForeignKey(nameof(PlanetId))]
        public Planet Planet { get; set; } = null!;
    }
}
