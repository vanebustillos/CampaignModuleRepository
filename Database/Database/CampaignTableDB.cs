using System;
using System.Collections.Generic;
using System.IO;
using CampaignModule.Database.Models;
using Database.Exceptions;
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
        public CampaignTableDB(IConfiguration configuration)
        {
            // assign config
            _configuration = configuration;
            InitDBContext(); // new List<T>()   
        }

        public async void InitDBContext()
        {
            
            // read path from config for DB (JSON)
            _dbPath = _configuration.GetSection("Database").GetSection("connectionString").Value;
            
            try
            {
                // "Connect to JSON File" => DeserializeObject
                _campaignList = JsonConvert.DeserializeObject<List<Campaign>>(File.ReadAllText(_dbPath));

                // "Connect to JSON File" => DeserializeObject
                //_dbContext = JsonConvert.DeserializeObject<DBContext>(File.ReadAllText(_dbPath));
                //_campaignList = _dbContext.campaigns;
            }
            catch(Exception ex)
            {
                throw new Database_Exceptions("Problems in: " + _dbPath);
            }
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
            
            foreach (Campaign camp in _campaignList) //DataBase
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
            foreach (Campaign campaign in _campaignList) //DataBase
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