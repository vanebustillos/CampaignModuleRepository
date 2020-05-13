using System;
using System.Collections.Generic;
using CampaignModule.Database.Models;
using CampaignModule.Controllers.DTOModels;

namespace CampaignModule.BusinessLogic
{
    public interface ICampaignLogic
    {
        public List<CampaignDTO> Get(); //Read all
        public CampaignDTO GetActive(); //GetActive
        public CampaignDTO Post(CampaignDTO campaign); //Create
        public void Put(CampaignDTO campaign, string id); //Update
        public void Delete(string id); //Delete
        public void Activate(string id); //Activate a campaign
        public void Deactivate(string id);//Deactivate a campaign
    }
}