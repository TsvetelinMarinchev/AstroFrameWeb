using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.ViewModels
{
    public class PlanetViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public double Mass { get; set; }

        public double Radius { get; set; }

        public double DistanceFromEarth { get; set; }

        public string ImageUrl { get; set; } = null!;

        public string GalaxyName { get; set; } = null!;

        public string StarName { get; set; } = null!;

        public string DiscoveredAgo { get; set; } = "Unknown";
    }
}
