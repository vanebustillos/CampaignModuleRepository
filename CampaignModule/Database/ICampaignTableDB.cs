﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampaignModule.Database.Models;

namespace CampaignModule.Database
{
    interface ICampaignTableDB
    {
        public List<Campaign> GetAll();
    }
}
