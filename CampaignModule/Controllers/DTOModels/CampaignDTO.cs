using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampaignModule.Controllers.DTOModels
{
    public class CampaignDTO
    {
        public string CampaignName { get; set; }
        public string CampaignType { get; set; }
        public string CampaignDescription { get; set; }
        public bool Active { get; set; }
    }
}
