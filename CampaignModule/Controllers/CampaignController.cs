using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using CampaignModule.BusinessLogic;
using CampaignModule.Controllers.DTOModels;

namespace CampaignModule.Controllers
{
    [Route("api")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private ICampaignLogic _campaignLogic; //To be added
        private IConfiguration _configuration;

        public CampaignController(ICampaignLogic campaignLogic, IConfiguration configuration)
        {
            _campaignLogic = campaignLogic;
            _configuration = configuration;
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
        public CampaignDTO Post([FromBody] CampaignDTO campaign)
        {
            Console.WriteLine("from post => " + campaign.Id + " - " + campaign.Name + " - " + campaign.Type + " - " + campaign.Description);
            _campaignLogic.Post(campaign); //Create, Makes a new Campaign
            var dbServer = _configuration.GetSection("Database").GetSection("ServerName");
            campaign.Name = $"{campaign.Name} data from {dbServer.Value}";
            return campaign;
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