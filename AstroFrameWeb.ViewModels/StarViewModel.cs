using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.ViewModels
{
    public class StarViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public string? GalaxyName { get; set; } 

        public string? StarTypeName { get; set; } 
        public string? ImageUrl { get; set; }

        public string? DiscoveredAgo { get; set; } 
    }
}
