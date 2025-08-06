using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.ViewModels
{
    public class GalaxyViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string GalaxyType { get; set; } = null!;

        public int NumberOfStars { get; set; }

        public double DistanceFromEarth { get; set; }

        public string? ImageUrl { get; set; }

        public string? CreatorId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? DiscoveredAgo { get; set; }
    }
}
