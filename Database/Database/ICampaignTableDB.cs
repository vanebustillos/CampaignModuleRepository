using System;
using System.Collections.Generic;
using CampaignModule.Database.Models;
using Database.Database;

namespace CampaignModule.Database
{
    public interface ICampaignTableDB : IDBManager
    {
        public List<Campaign> GetAll(); //Get, Read all in Database
        public Campaign Create(Campaign campaign); //Post
        public void Update(Campaign campaign); //Put
        public void Delete(Campaign campaign); //Delete
        public bool OneCampaignActive();//Verify if any campaign is actives
    }
}