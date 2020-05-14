using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Exceptions;
using CampaignModule.Controllers.DTOModels;
using CampaignModule.Database;
using CampaignModule.Database.Models;
using Serilog;

namespace CampaignModule.BusinessLogic
{
    public class CampaignLogic : ICampaignLogic
    {
        private ICampaignTableDB _campaignDB; // DB of campaign
        public List<Campaign> allCampaign; //Data of DB
        public List<string> ValidTypes = new List<string> { "navidad", "verano", "black friday" };

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
            Log.Logger.Information("Succesfull");
            return Datos;

        }
        public CampaignDTO GetActive()
        {
            UpdateLocalDB();
            CampaignDTO active = new CampaignDTO();
            var exists = false;
            foreach (Campaign camp in allCampaign)
            {
                if (camp.Active)
                {
                    active = ConvDBtoDTO(camp);
                    exists = true;
                }

            }
            if (!exists)
            {
                Log.Logger.Information("Error: No active Campaign found");
                throw new BusinessLogic_Exceptions("Error: No active Campaign found");
            }

            Log.Logger.Information("Succesfull");
            return active;

        }

        public CampaignDTO Post(CampaignDTO campaign) //Creates a new Campaign
        {
            if (VerifyFields(campaign))
            {
                UpdateLocalDB();
                Campaign input = ConvDTOtoDB(campaign);

                if (allCampaign.Count == 0) //verifies if allCampaigns is empty
                {
                    input.Id = "CAMPAIGN-1"; //if it is, its first member has id = 1
                }
                else
                {
                    Campaign c = allCampaign.Last();
                    string[] fracment = c.Id.Split("-");
                    int lastId = Int32.Parse(fracment[1]) + 1;
                    input.Id = "CAMPAIGN-" + lastId; //if not, it is the last id + 1
                }
                SelectType(input);
                if (input.Active)
                {
                    if (_campaignDB.OneCampaignActive()) //if a campaign is already active
                    {
                        input.Active = false; //input campaign can´t be activate
                        Log.Logger.Information("Error: New Campaign cannot be set as active, there is one already activated");
                        throw new BusinessLogic_Exceptions("Error: There's already an active campaign");
                    }
                }
                Log.Logger.Information("Succesfull");
                _campaignDB.Create(input); //Creates Campaign in DataBase
                return campaign;
            }
            else
            {
                Log.Logger.Information("Error Ocurred: Missing Values on Post");
                throw new BusinessLogic_Exceptions("Error: Incorrect Values on Post: NullReferenceException");
            }

        }

        public void Put(CampaignDTO campaign, string id) //Update, all fields in one
        {
            UpdateLocalDB();

            foreach (Campaign c in allCampaign)
            {
                if (c.Id == id.Trim().ToUpper())
                {
                    Campaign input = ConvDTOtoDB(campaign);
                    if (input.Name != null)
                        c.Name = input.Name;
                    if (input.Type != null)
                    {
                        SelectType(input);
                        c.Type = input.Type;
                    }
                    if (input.Description != null)
                        c.Description = input.Description;
                    if (input.Active) //removes active campaign if any
                    {
                        ActivateUpdate(id);//activate campaign
                    }
                    else
                    {
                        DeactivateUpdate(id); //deactivate campaign
                    }
                    Log.Logger.Information("Succesfull");
                    _campaignDB.Update(input); //Updates Campaign in DataBase 
                    break;
                } //if none found does nothing
            }
        }

        public void Delete(string id) // Delete
        {
            bool wasDeleted = false;
            UpdateLocalDB();
            foreach (Campaign c in allCampaign)
            {
                if (c.Id == id.Trim().ToUpper())
                {
                    wasDeleted = true;
                    Log.Logger.Information("Succesfull");
                    _campaignDB.Delete(c); //Delete Campaign in DataBase 
                    break;
                }
            }
            if (wasDeleted == false)
            {
                Log.Logger.Information("Error: That Campaign doesn't Exist");
                throw new BusinessLogic_Exceptions("Error: That campaign doesn't exist");
            }
        }

        /**********Auxiliary Functions****************/
        public void UpdateLocalDB() //Updates the local list of elements used for the operations
        {
            allCampaign = _campaignDB.GetAll();
        }

        public bool VerifyFields(CampaignDTO campaign) //Reviews all the input fields of type string to verify its correctnes, returns false if an error is found
        {
            try
            {
                if (String.IsNullOrEmpty(campaign.Name.Trim())) //Verify if name is null or empty
                {
                    Log.Logger.Information("Error: Missing Name Value");
                    return false;
                }
                if (String.IsNullOrEmpty(campaign.Description.Trim())) //Verify if description is null or empty
                {
                    Log.Logger.Information("Error: Missing Description Value");
                    return false;
                }
                if (String.IsNullOrEmpty(campaign.Type.Trim()) || VerifyType(campaign.Type)) //Verify if type is null or invalid
                {
                    Log.Logger.Information("Error: Incorrect Type Value");
                    return false;
                }
                return true;
            }
            catch (NullReferenceException ex)
            {

                Console.WriteLine("Error: Incorrect Values on Post");
                return false;
            }
        }

        public bool VerifyType(string tipo) //verifies the type, returns false if it isnt incorrect, else it returns true or error
        {
            string tipoMinuscula = tipo.ToLower().Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Trim();
            foreach (string type in ValidTypes)
            {
                if (tipoMinuscula == type)
                {
                    return false;
                }
            }

            return true;

        }

        public void Activate(string id) //Deactivates any active campaign present, it considers only one active at the time
        {
            bool flagActivated = false;
            UpdateLocalDB();
            foreach (Campaign c2 in allCampaign)
            {
                if (c2.Id == id.Trim())
                {
                    if (!_campaignDB.OneCampaignActive())//if no campaign is active
                    {
                        flagActivated = true;
                        //Activate campaign logic
                        Log.Logger.Information("Succesfull");
                        c2.Active = true; //input campaign is activate
                        _campaignDB.Update(c2);
                    }
                    break;
                }
            }
            if (flagActivated == false)
            {
                Log.Logger.Information("Error: There's Already an Active Campaign");
                throw new BusinessLogic_Exceptions("Error: There's already an active campaign");
            }
        }

        public void Deactivate(string id)
        {
            bool flagDeactivated = false;
            UpdateLocalDB();
            foreach (Campaign c2 in allCampaign)
            {
                if (c2.Id == id)
                {
                    if (c2.Active)
                    {
                        flagDeactivated = true;
                        //Deactivate campaign logic
                        c2.Active = false;
                        Log.Logger.Information("Succesfull");
                        _campaignDB.Update(c2);
                        break;
                    }
                }
            }
            if (flagDeactivated == false)
            {
                Log.Logger.Information("Error: That Campaign isn't Active");
                throw new BusinessLogic_Exceptions("Error: That campaign isn't active");
            }
        }

        public void ActivateUpdate(string id) //Deactivates any active campaign present, it considers only one active at the time
        {
            bool flagActivated = false;
            UpdateLocalDB();
            foreach (Campaign c2 in allCampaign)
            {
                if (c2.Id == id.Trim())
                {
                    if (c2.Active || !_campaignDB.OneCampaignActive())//if the same campaign is active or there isn't an active campaign
                    {
                        flagActivated = true;
                        //Activate campaign logic
                        Log.Logger.Information("Succesfull");
                        c2.Active = true; //input campaign is activate
                        _campaignDB.Update(c2);
                    }
                    break;
                }
            }
            if (flagActivated == false)
            {
                Log.Logger.Information("Error: There's Already an Active Campaign");
                throw new BusinessLogic_Exceptions("Error: There's already an active campaign");
            }
        }

        public void DeactivateUpdate(string id)
        {
            UpdateLocalDB();
            foreach (Campaign c2 in allCampaign)
            {
                if (c2.Id == id)
                {
                    //Deactivate campaign logic
                    c2.Active = false;
                    Log.Logger.Information("Succesfull");
                    _campaignDB.Update(c2);
                    break;
                }
            }
        }

        public void SelectType(Campaign input)
        {
            switch (input.Type.Trim().ToLower()) // assigns a tipe of campaign
            {
                case "navidad":
                    input.Type = "XMAS";
                    break;
                case "verano":
                    input.Type = "SUMMER";
                    break;
                case "black friday":
                    input.Type = "BFRIDAY";
                    break;
                default:
                    Log.Logger.Information("Error: Incorrect Type Value");
                    throw new BusinessLogic_Exceptions("Error: Incorrect Type Value");
                    //break;
            }
        }

        public Campaign ConvDTOtoDB(CampaignDTO old) //Converts a DTOCampaign to a DB Campaign
        {
            Campaign valid = new Campaign();
            if (old.Id != "CAMPAIGN-0")
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