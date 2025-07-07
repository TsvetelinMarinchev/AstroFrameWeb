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
    public class StarComment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]
        [Display(Name = "Comment")]
        [Comment("User comment for a star")]
        public string Content { get; set; } = null!;
     
        [Required]
        public int StarId { get; set; }

        [ForeignKey(nameof(StarId))]
        public Star Star { get; set; } = null!;

        public string? AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public ApplicationUser? Author { get; set; }

    }
}
