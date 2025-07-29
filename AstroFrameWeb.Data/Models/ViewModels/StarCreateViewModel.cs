using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace AstroFrameWeb.Data.Models.ViewModels
{
    
    public class StarCreateViewModel
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        [Range(0.01, 1000000, ErrorMessage = "Въведи валидна цена.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Моля, избери галактика.")]
        public int GalaxyId { get; set; }

        [Required(ErrorMessage = "Моля, избери тип звезда.")]
        public int StarTypeId { get; set; }

        public string? ImageUrl { get; set; }

        public IEnumerable<SelectListItem> Galaxies { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> StarTypes { get; set; } = new List<SelectListItem>();
    }
}
