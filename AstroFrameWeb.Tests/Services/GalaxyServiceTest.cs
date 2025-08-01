using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AstroFrameWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace AstroFrameWeb.Tests.Services
{
    public class GalaxyServiceTest
    {

        [Fact]
        public async Task CreateGalaxyAsyncShouldAddGalaxyToDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "TestGalaxyDb")
               .Options;

        }
    }
}
