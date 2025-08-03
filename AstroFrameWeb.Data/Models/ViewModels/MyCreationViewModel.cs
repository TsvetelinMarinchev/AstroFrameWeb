using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AstroFrameWeb.Data.Models;
using System.Collections.Generic;

namespace AstroFrameWeb.Data.Models.ViewModels
{
    public class MyCreationViewModel
    {
        public List<Star>? Stars { get; set; }
        public List<Galaxy>? Galaxies { get; set; }
        public List<Planet>? Planets { get; set; }
    }
}
