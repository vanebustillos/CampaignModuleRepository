using System;
using System.Collections.Generic;
using CampaignModule.Database.Models;

namespace CampaignModule.BusinessLogic
{
    public interface ICampaignLogic
    {
        public List<Campaign> Get(); //Read all
        public void Post(Campaign campaign); //Create
        public void Put(int id, string newName, string newType, string newDescription, bool newActive); //Update
        public void Delete(int id); //Delete
    }
}
