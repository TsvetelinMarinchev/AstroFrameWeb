using AstroFrameWeb.Data.Models.ViewModels;
using AstroFrameWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Services.Interfaces
{
    public interface IStarService
    {
        Task<IEnumerable<StarViewModel>> GetAllAsync();
        Task<StarViewModel?> GetByIdAsync(int id);
        Task CreateStarAsync(StarCreateViewModel model, string creatorId);
        Task UpdateStarAsync(int id, StarCreateViewModel model);
        Task DeleteStarAsync(int id);
    }
}
