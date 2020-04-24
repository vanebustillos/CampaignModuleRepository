using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CampaignModule.Database.Models;
using CampaignModule.BusinessLogic;
using CampaignModule.Controllers.DTOModels;

namespace CampaignModule.Controllers
{
    [Route("api")]
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
        [Route("campaigns")]
        public IEnumerable<CampaignDTO> GetAll()
        {
            return _campaignLogic.Get(); // Read, Returns all elements in database
        }

        // POST: api/Campaign
        [HttpPost]
        [Route("campaigns")]
        public void Post([FromBody] CampaignDTO value)
        {
            _campaignLogic.Post(value); //Create, Makes a new Campaign
        }

        // PUT: api/Campaign/5
        [HttpPut]
        [Route("campaigns/{id}")]
        public void Put([FromBody]CampaignDTO campaign, int id)
        {
            _campaignLogic.Put(campaign, id); //Update, Changes all fields in a Campaign in DB, except for the id
        }
        
        [HttpPut]
        [Route("campaigns/{id}/activate")]
        public void Activate(int id)
        {
            _campaignLogic.Activate(id); //Activate a campaign
        }
        
        [HttpPut]
        [Route("campaigns/{id}/deactivate")]
        public void Deactivate(int id)
        {
            _campaignLogic.Deactivate(id); //Activate a campaign
        }
        
        // DELETE: api/Campaign/5
        [HttpDelete]
        [Route("campaigns/{id}")]
        public void Delete(int id)
        {
            _campaignLogic.Delete(id); //Delete, Removes a campaign from DB
        }
    }
}
