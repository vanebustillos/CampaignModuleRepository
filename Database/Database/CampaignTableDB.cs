using System;
using System.Collections.Generic;
using CampaignModule.Database.Models;

namespace CampaignModule.Database
{
    public class CampaignTableDB : ICampaignTableDB
    {
        List<Campaign> DataBase = new List<Campaign>
        {
                new Campaign() { Id = "CAMPAIGN-1",Name = "Christmas Part I", Type = "XMAS", Description = "Before 24th December", Active = false},
                new Campaign() { Id = "CAMPAIGN-2",Name = "Christmas Part II", Type = "XMAS", Description = "After 25th December", Active = false},
                new Campaign() { Id = "CAMPAIGN-3",Name = "Summer discounts", Type = "SUMMER", Description = "All July", Active = false},
                new Campaign() { Id = "CAMPAIGN-4",Name = "Black Friday discounts", Type = "BFRIDAY", Description = "Buy all you can", Active = false}
        };

        public List<Campaign> GetAll() //Read, returns all campaigns
        {
            return DataBase;
        }

        public Campaign Create(Campaign campaign) // Creates a New Campaign 
        {
            DataBase.Add(campaign);
            return campaign;
        }

        public void Update(Campaign campaign) //Updates all fields in a Campaign except its id
        {
            foreach(Campaign camp in DataBase)
            {
                if (camp == campaign)
                {
                    camp.Name = campaign.Name;
                    camp.Type = campaign.Type;
                    camp.Description = campaign.Description;
                    camp.Active = campaign.Active;
                    break;
                }
            }
        }

        public void Delete(Campaign campaign) //Removes a Campaign
        {
            DataBase.Remove(campaign);
        }

        public bool OneCampaignActive()
        {
            foreach(Campaign campaign in DataBase)
            {
                if (campaign.Active)
                {
                    return true;
                }
            }
            return false;
        }
    }
}