using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherMobile
{
    public class Cities
    {
        public long Id { get; set; }

        public string City1 { get; set; } = null!;

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public virtual ICollection<int> CityInfos { get; set; }

        public virtual ICollection<int> UserInfos { get; set; }

    }
}
