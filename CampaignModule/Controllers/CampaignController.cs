using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CampaignModule.BusinessLogic; //To be added
using CampaignModule.Database.Models;

namespace CampaignModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {

        private  ICampaignLogic _campaignLogic; //To be added

        public CampaignController (ICampaignLogic campaignLogic)
        {
            _campaignLogic = campaignLogic;
        }

        // GET: api/Campaign
        [HttpGet]
        public IEnumerable<Database.Models.Campaign> GetAll()
        {
            return _campaignLogic.Get(); //To be added
        }

        // POST: api/Campaign
        [HttpPost]
        public void Post([FromBody] Campaign value)
        {
           _campaignLogic.Post(value); //To be added
        }

        // PUT: api/Campaign/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string newName, string newType, string newDescription, bool newActive)
        {
           _campaignLogic.Put(id, newName, newType, newDescription, newActive); //To be added
        }

        // DELETE: api/Campaign/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
           _campaignLogic.Delete(id); //To be added
        }
    }
}