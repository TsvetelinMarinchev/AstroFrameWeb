using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Data.Models
{
    public class PlanetComment
    {
        [Key]
        [Comment("Comment identifier")]
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]
        [Display(Name = "Comment Content")]
        [Comment("The content of the comment")]
        public string Content { get; set; } = null!;
        public string UserId { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        public int PlanetId { get; set; }
        [ForeignKey(nameof(PlanetId))]
        public Planet Planet { get; set; } = null!;
    }
}
