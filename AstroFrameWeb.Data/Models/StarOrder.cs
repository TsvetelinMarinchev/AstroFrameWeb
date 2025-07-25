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
    public class StarOrder
    {
        public int Id { get; set; }

        [Required]
        public int StarId { get; set; }

        [ForeignKey(nameof(StarId))]
        public Star? Star { get; set; }


        public string? UserId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Comment("Shows the bonus is locked or not")]
        public bool BonusUnlocked { get; set; } = false; // experince the Moon TODO


    }
}
