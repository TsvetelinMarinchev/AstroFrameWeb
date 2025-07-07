using AstroFrameWeb.Data;
using AstroFrameWeb.Services.Interfaces;
using AstroFrameWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Services.Implementations
{
    public class PlanetService : IPlanetService
    {
        public Task<IEnumerable<PlanetViewModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }

}
