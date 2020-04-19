using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampaignModule.Database;
using CampaignModule.Database.Models;

namespace CampaignModule.BusinessLogic
{
    public class CampaignLogic : ICampaignLogic
    {
        private readonly ICampaignTableDB _campaignDB;

        public CampaignLogic(ICampaignTableDB campaignDB)
        {
            _campaignDB = campaignDB;
        }

        public List<Campaign> Campaingclass()
        {
            List<Campaign> allCampaign = _campaignDB.GetAll();

            foreach (CampaignDTO campaign in allCampaign)
            {
                Activate(campaign, "XMAS");
            }

            return allCampaign;
        }

        private void Activate(Campaign campaign, string pType)
        {
            if (campaign.Active == true)
            {
                campaign.Active == false;
            }

            if (campaign.Type == pType)
            {
                campaign.Active == true;
            }
        }
    }
}
