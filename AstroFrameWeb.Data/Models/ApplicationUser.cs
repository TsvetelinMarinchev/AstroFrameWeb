using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Star> OwnedStars { get; set; }
            =new HashSet<Star>();

        public ICollection<UserFavoritePlanet> FavoritePlanets { get; set; }
                 = new HashSet<UserFavoritePlanet>();
    }
}
