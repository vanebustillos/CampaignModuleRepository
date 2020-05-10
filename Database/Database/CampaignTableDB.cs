using System;
using System.Collections.Generic;
using System.IO;
using CampaignModule.Database.Models;
using Database.Database.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CampaignModule.Database
{
    public class CampaignTableDB : ICampaignTableDB
    {
        private readonly IConfiguration _configuration;
        private string _dbPath;
        //private DBContext _dbContext;
        private List<Campaign> _campaignList;

        /*List<Campaign> DataBase = new List<Campaign>
        {
                new Campaign() { Id = "CAMPAIGN-1",Name = "Christmas Part I", Type = "XMAS", Description = "Before 24th December", Active = false},
                new Campaign() { Id = "CAMPAIGN-2",Name = "Christmas Part II", Type = "XMAS", Description = "After 25th December", Active = false},
                new Campaign() { Id = "CAMPAIGN-3",Name = "Summer discounts", Type = "SUMMER", Description = "All July", Active = false},
                new Campaign() { Id = "CAMPAIGN-4",Name = "Black Friday discounts", Type = "BFRIDAY", Description = "Buy all you can", Active = false}
        };*/

        public CampaignTableDB(IConfiguration configuration)
        {
            // assign config
            _configuration = configuration;
            InitDBContext(); // new List<T>()   
        }

        public void InitDBContext()
        {
            // read path from config for DB (JSON)
            _dbPath = _configuration.GetSection("Database").GetSection("connectionString").Value;

            // "Connect to JSON File" => DeserializeObject
            _campaignList = JsonConvert.DeserializeObject<List<Campaign>>(File.ReadAllText(_dbPath));

            // "Connect to JSON File" => DeserializeObject
            //_dbContext = JsonConvert.DeserializeObject<DBContext>(File.ReadAllText(_dbPath));
            //_campaignList = _dbContext.campaigns;
        }
        public void SaveChanges()
        {
            File.WriteAllText(_dbPath, JsonConvert.SerializeObject(_campaignList)); //_dbContext
        }

        public List<Campaign> GetAll() //Read, returns all campaigns
        {
            //return DataBase;
            return _campaignList;
        }

        public Campaign Create(Campaign campaign) // Creates a New Campaign 
        {
            //DataBase.Add(campaign);
            _campaignList.Add(campaign);
            SaveChanges();
            return campaign;
        }

        public void Update(Campaign campaign) //Updates all fields in a Campaign except its id
        {
            foreach(Campaign camp in _campaignList) //DataBase
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
            SaveChanges();
        }

        public void Delete(Campaign campaign) //Removes a Campaign
        {
            //DataBase.Remove(campaign);
            _campaignList.Remove(campaign);
            SaveChanges();
        }

        public bool OneCampaignActive()
        {
            foreach(Campaign campaign in _campaignList) //DataBase
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