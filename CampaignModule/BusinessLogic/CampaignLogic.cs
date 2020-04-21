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
        private ICampaignTableDB _campaignDB; // DB of campaign
        public List<Campaign> allCampaign; //Data of DB

        public CampaignLogic(ICampaignTableDB campaignDB)
        {
            _campaignDB = campaignDB;
            allCampaign = _campaignDB.GetAll();
        }

        public List<Campaign> Get() //Read, returns a list of all its members
        {
            return allCampaign;
        }

        private void Activate() //Deactivates any active campaign present, it considers only one active at the time
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

        public void Post(Campaign campaign) //Creates a new Campaign
        {
            if (allCampaign.Count == 0) //verifies if allCampaigns is empty
            {
                campaign.Id = 1; //if it is, its first member has id = 1
            }
            else
            {
                Campaign c = allCampaign.Last();
                campaign.Id = c.Id++; //if not, it is the last id + 1
            }
            

            switch (campaign.Type) // assigns a tipe of campaign
            {
                case "Navidad":
                    campaign.Type = "XMAS";
                    break;
                case "Verano":
                    campaign.Type = "SUMMER";
                    break;
                case "Black Friday":
                    campaign.Type = "BFRIDAY";
                    break;
                default:
                    break;
            }
            if (campaign.Active) //removes active campaign if any
            {
                Activate();
            }
            allCampaign.Add(campaign);
            _campaignDB.CUD(allCampaign); //Updates DataBase 
        }

        public void Put(int id, string newName, string newType, string newDescription, bool newActive) //Update, all fields in one
        {
            foreach(Campaign c in allCampaign)
            {
                if (c.Id == id)
                {
                    c.Name = newName;
                    c.Type = newType;
                    c.Description = newDescription;
                    if (newActive) //removes active campaign if any
                    {
                        Activate();
                    }
                    c.Active = newActive; //activates campaign
                    _campaignDB.CUD(allCampaign); //Updates DataBase 
                    break;  

                } //if none found does nothing
            }
        }
        public void Delete(int id) // Delete
        {
            foreach(Campaign c in allCampaign)
            {
                if (c.Id == id)
                    allCampaign.Remove(c);
                _campaignDB.CUD(allCampaign); //Updates DataBase 
                break;
            }
        } 

        public Campaign  ConvDTOtoDB(CampaignDTO old) //Converts a DTOCampaign to a DB Campaign
        {
            Campaign valid = new Campaign();

            valid.Id = old.Id;
            valid.Name = old.Name;
            valid.Description = old.Description;
            valid.Type = old.Type;
            valid.Active = old.Active;
            return valid;
        }

        public CampaignDTO ConvDBtoDTO(Campaign old) //Converts a DB Campaign to a DTOCampaign
        {
            CampaignDTO valid = new CampaignDTO();

            valid.Id = old.Id;
            valid.Name = old.Name;
            valid.Description = old.Description;
            valid.Type = old.Type;
            valid.Active = old.Active;
            return valid;
        }
    }
}
