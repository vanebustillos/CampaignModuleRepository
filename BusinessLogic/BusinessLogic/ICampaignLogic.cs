using System;
using System.Collections.Generic;
using CampaignModule.Database.Models;
using CampaignModule.Controllers.DTOModels;

namespace CampaignModule.BusinessLogic
{
    public interface ICampaignLogic
    {
        public List<CampaignDTO> Get(); //Read all
        public CampaignDTO Post(CampaignDTO campaign); //Create
        public void Put(CampaignDTO campaign, int id); //Update
        public void Delete(int id); //Delete
        public void Activate(int id); //Activate a campaign
        public void Deactivate(int id);//Deactivate a campaign
    }
}