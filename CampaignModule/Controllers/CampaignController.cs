using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CampaignModule.Database.Models;
using CampaignModule.BusinessLogic;

namespace CampaignModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {

        private ICampaignLogic _campaignLogic; //To be added

        public CampaignController(ICampaignLogic campaignLogic)
        {
            _campaignLogic = campaignLogic;
        }

        // GET: api/Campaign
        [HttpGet]
        public IEnumerable<Database.Models.Campaign> GetAll()
        {
            return _campaignLogic.Get(); // Read, Returns all elements in database
        }

        // POST: api/Campaign
        [HttpPost]
        public void Post([FromBody] Campaign value)
        {
           _campaignLogic.Post(value); //Create, Makes a new Campaign
        }

        // PUT: api/Campaign/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string newName, string newType, string newDescription, bool newActive)
        {
           _campaignLogic.Put(id, newName, newType, newDescription, newActive); //Update, Changes all fields in a Campaign in DB, except for the id
        }

        // DELETE: api/Campaign/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
           _campaignLogic.Delete(id); //Delete, Removes a campaign from DB
        }
    }
}