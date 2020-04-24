using System;
using System.Collections.Generic;
using CampaignModule.Database.Models;
using CampaignModule.Controllers.DTOModels;

namespace CampaignModule.BusinessLogic
{
    public interface ICampaignLogic
    {
        public List<CampaignDTO> Get(); //Read all
        public void Post(CampaignDTO campaign); //Create
        public void Put(CampaignDTO campaign); //Update
        public void Delete(int id); //Delete
    }
}
