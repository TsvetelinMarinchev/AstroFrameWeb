using AstroFrameWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Services.Interfaces
{
    public interface IPlanetService
    {
        Task<IEnumerable<PlanetViewModel>> GetAllAsync();
    }
}
