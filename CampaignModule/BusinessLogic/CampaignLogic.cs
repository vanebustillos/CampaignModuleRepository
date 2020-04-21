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
            return allCampaign;
        }

        private void Activate()
        {
            foreach (Campaign c2 in allCampaign)
            {
                if (c2.Active)
                {
                    c2.Active = false;
                }
                break;
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
            if (campaign.Active)
            {
                Activate();
            }
            allCampaign.Add(campaign);
        }
        public void Put(int id, string newName, string newType, string newDescription, bool newActive)
        {
            foreach(Campaign c in allCampaign)
            {
                if (c.Id == id)
                {
                    c.Name = newName;
                    c.Type = newType;
                    c.Description = newDescription;
                    if (newActive)
                    {
                        Activate();
                    }
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
