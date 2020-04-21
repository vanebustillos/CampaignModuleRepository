using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampaignModule.Controllers.DTOModels;
using CampaignModule.Database.Models;

namespace CampaignModule.BusinessLogic
{
    public interface ICampaignLogic
    {
        public List<Campaign> Get();
        public void Post(Campaign campaign);
        public void Put(int id, string newName, string newType, string newDescription, bool newActive );
        public void Delete(int id);
    }
}
