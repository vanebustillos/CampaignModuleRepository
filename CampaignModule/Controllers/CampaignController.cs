using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CampaignModule.BusinessLogic; //To be added

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
        public IEnumerable<string> GetAll()
        {
            return _campaignLogic.Get(); //To be added
        }

        // POST: api/Campaign
        [HttpPost]
        public void Post([FromBody] string value)
        {
            return _campaignLogic.Post(value); //To be added
        }

        // PUT: api/Campaign/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            return _campaignLogic.Put(id, value); //To be added
        }

        // DELETE: api/Campaign/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            return _campaignLogic.Delete(id); //To be added
        }
    }
}