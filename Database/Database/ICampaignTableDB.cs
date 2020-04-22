﻿using System;
using System.Collections.Generic;
using CampaignModule.Database.Models;

namespace CampaignModule.Database
{
    public interface ICampaignTableDB
    {
        public List<Campaign> GetAll(); //Read all in Database

        public void CUD(List<Campaign> campaigns);
    }
}