using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CampaignModule.Controllers.DTOModels;
using CampaignModule.Database;
using CampaignModule.Database.Models;


namespace CampaignModule.BusinessLogic
{
    public class CampaignLogic : ICampaignLogic
    {
        private ICampaignTableDB _campaignDB; // DB of campaign
        public List<Campaign> allCampaign; //Data of DB

        public CampaignLogic(ICampaignTableDB campaignDB)
        {
            _campaignDB = campaignDB;
            allCampaign = _campaignDB.GetAll();
        }


        /**********Main Functions****************/

        public List<CampaignDTO> Get() //Read, returns a list of all its members
        {
            UpdateLocalDB();
            List<CampaignDTO> Datos = new List<CampaignDTO>(); 
            foreach (Campaign camp in allCampaign)
            {
                Datos.Add(ConvDBtoDTO(camp));
            }
            return Datos;

        }

        public void Post(CampaignDTO campaign) //Creates a new Campaign
        {
            UpdateLocalDB();
            Campaign input = ConvDTOtoDB(campaign);
            if (allCampaign.Count == 0) //verifies if allCampaigns is empty
            {
                input.Id = 1; //if it is, its first member has id = 1
            }
            else
            {
                Campaign c = allCampaign.Last();
                input.Id = c.Id + 1; //if not, it is the last id + 1
            }

            SelectType(input);

            if (input.Active) //removes active campaign if any
            {
                Activate();
            }
             
            allCampaign.Add(input); //Creates Campaign in DataBase 

        }

        public void Put(CampaignDTO campaign) //Update, all fields in one
        {
            UpdateLocalDB();
            Campaign input = ConvDTOtoDB(campaign);
            foreach(Campaign c in allCampaign)
            {
                if (c.Id == input.Id)
                {
                    c.Name = input.Name;
                    SelectType(input);
                    c.Type = input.Type;
                    c.Description = input.Description;
                    if (input.Active) //removes active campaign if any
                    {
                        Activate();
                    }
                    c.Active = input.Active; //activates campaign
                    
                    _campaignDB.Update(input); //Updates Campaign in DataBase 
                    break;  

                } //if none found does nothing
            }
        }
        public void Delete(int id) // Delete
        {
            UpdateLocalDB();
            foreach (Campaign c in allCampaign)
            {
                if (c.Id == id)
                {
                    allCampaign.Remove(c);
                    _campaignDB.Delete(c); //Delete Campaign in DataBase 
                    break;
                }
                
            }
        }

        /**********Auxiliary Functions****************/
        public void UpdateLocalDB() //Updates the local list of elements used for the operations
        {
            allCampaign = _campaignDB.GetAll();
        }

        private void Activate() //Deactivates any active campaign present, it considers only one active at the time
        {
            foreach (Campaign c2 in allCampaign)
            {
                if (c2.Active)
                {
                    c2.Active = false;
                    _campaignDB.Update(c2);
                    break;
                }
            }
        }

        public void SelectType(Campaign input)
        {


            switch (input.Type) // assigns a tipe of campaign
            {
                case "Navidad":
                    input.Type = "XMAS";
                    break;
                case "Verano":
                    input.Type = "SUMMER";
                    break;
                case "Black Friday":
                    input.Type = "BFRIDAY";
                    break;
                case "Primavera":
                    input.Type = "SPRING";
                    break;
                default:
                    break;
            }
        }

        public Campaign ConvDTOtoDB(CampaignDTO old) //Converts a DTOCampaign to a DB Campaign
        {
            Campaign valid = new Campaign();

            valid.Id = old.Id;
            valid.Name = old.Name;
            valid.Description = old.Description;
            valid.Type = old.Type;
            valid.Active = old.Active;
            return valid;
        }

        public CampaignDTO ConvDBtoDTO(Campaign old) //Converts a DB Campaign to a DTOCampaign
        {
            CampaignDTO valid = new CampaignDTO();

            valid.Id = old.Id;
            valid.Name = old.Name;
            valid.Description = old.Description;
            valid.Type = old.Type;
            valid.Active = old.Active;
            return valid;
        }


    }
}
