using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampaignModule.Controllers.DTOModels;

namespace CampaignModule.BusinessLogic
{
    public interface ICampaignLogic
    {
        public List<CampaignDTO> Campaingclass();
    }
}
