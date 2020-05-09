using CampaignModule.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Database.Models
{
    public class DBContext
    {
        public List<Campaign> campaigns { get; set; }
    }
}
