using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampaignModule.Database.Models;

namespace CampaignModule.Database
{
    public class CampaignTableDB : ICampaignTableDB
    {
        public List<Campaign> GetAll()
        {
            return new List<Campaign>()
            {
                new Campaign() {Name="Christmas Part I", Type="XMAS", Description="Before 24th December", Active=false},
                new Campaign() {Name="Christmas Part II", Type="XMAS", Description="After 25th December", Active=false},
                new Campaign() {Name="Easter", Type="SPRING", Description="Resurrection of Jesus Christ between March 21 and April 25", Active=true},
                new Campaign() {Name="Summer discounts", Type="SUMMER", Description="All July", Active=false},
                new Campaign() {Name="Black Friday discounts", Type="BFRIDAY", Description="Buy all you can", Active=false}
            };
        }
    }
}