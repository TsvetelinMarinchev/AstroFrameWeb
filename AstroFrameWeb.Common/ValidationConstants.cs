using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Common
{
    public static class ValidationConstants
    {
        public static class Star
        {
            public const int StarMinLength = 2;
            public const int StarMaxLength = 100;
            public const int StarDescriptionMaxLength = 2000;
            public const decimal StarMinPrice = 0.01m;
            public const decimal StarMaxPrice = 100000m;
            public const string StarCreatedDateFormat = "dd-MM-yyyy";
        }
        public static class Galaxy
        {
            public const int GalaxyMinLength = 2;
            public const int GalaxyMaxLength = 50;
            public const int GalaxyDescriptionMaxLength = 2000;
            public const double NumberOfStarsInGalaxy = 10000;
            public const double Distance = 1000000;
        }
        public static class Planet
        {
            public const int PlanetNameMinLength = 2;
            public const int PlanetNameMaxLength = 100;
            public const int PlanetDescriptionMaxLength = 2000;

            public const double PlanetMaxMass = 100000;
            public const double PlanetMaxRadius = 10000;
            public const double PlanetMaxDistance = 1000000;
        }

        public static class StarType
        {
            public const int StarTypeMinLenght = 1;
            public const int StarTypeMaxLenght = 10;
            public const int DescriptionStarTypeMaxLength = 100;

        }

    }
}
