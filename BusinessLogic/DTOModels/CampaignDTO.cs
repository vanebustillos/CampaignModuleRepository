using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampaignModule.Controllers.DTOModels
{
    public class CampaignDTO
    {
        public string Id { get; set; } 
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}