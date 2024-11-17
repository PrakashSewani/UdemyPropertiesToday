using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class CacheSetting
    {
        public int SlidingExpiration { get; set; }
        public string DestinationURL { get; set; }
        public string ApplicationName { get; set; }
        public bool BypassCache { get; set; }
    }
}
