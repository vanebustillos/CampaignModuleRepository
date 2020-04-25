using System;
using System.Collections.Generic;
using CampaignModule.Database.Models;

namespace CampaignModule.Database
{
    public interface ICampaignTableDB
    {
        public List<Campaign> GetAll(); //Get, Read all in Database
        public void Create(Campaign campaign); //Post
        public void Update(Campaign campaign); //Put
        public void Delete(Campaign campaign); //Delete
        public bool OneCampaignActive();//Verify if any campaign is actives
    }
}