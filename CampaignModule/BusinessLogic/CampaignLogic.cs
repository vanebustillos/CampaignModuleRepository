using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampaignModule.Controllers.DTOModels;
using CampaignModule.Database;
using CampaignModule.Database.Models;

namespace CampaignModule.BusinessLogic
{
    public class CampaignLogic : ICampaignLogic
    {
        private ICampaignTableDB _campaignDB;
        public List<Campaign> allCampaign;

        public CampaignLogic(ICampaignTableDB campaignDB)
        {
            _campaignDB = campaignDB;
            allCampaign = _campaignDB.GetAll();
        }

        public List<Campaign> Get()
        {
            
            foreach (Campaign campaign in allCampaign)
            {
                Activate(campaign, "XMAS");
            }

            return allCampaign;
        }

        private void Activate(Campaign campaign, string pType)
        {
            if (campaign.Active == true)
            {
                campaign.Active = false;
            }

            if (campaign.Type == pType)
            {
                campaign.Active = true;
            }
        }

        public void Post(Campaign campaign)
        {
            Campaign c = allCampaign.LastOrDefault();
            campaign.Id= c.Id ++;

            switch (c.Type)
            {
                case "Navidad": 
                    c.Type = "XMAS";
                    break;
                case "Verano":
                    c.Type = "SUMMER";
                    break;
                case "Black Friday":
                    c.Type = "BFRIDAY";
                    break;
                default:
                    break;
            }
            allCampaign.Add(campaign);
        }
        public void Put(int id, string newName, string newType, string newDescription)
        {
            foreach(Campaign c in allCampaign)
            {
                if (c.Id == id)
                {
                    c.Name = newName;
                    c.Type = newType;
                    c.Description = newDescription;
                    // Activate()??
                    break;
                }
            }
        }
        public void Delete(int id)
        {
            foreach(Campaign c in allCampaign)
            {
                if (c.Id == id)
                    allCampaign.Remove(c);
                break;
            }
        } 
    }
}
