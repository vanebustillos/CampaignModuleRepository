using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
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
            Log.Logger.Information("Client Asked for Campaign list");
            return Datos;

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
                        input.Active = false;//input campaign can´t be activate
                        Log.Logger.Information("New Campaign cannot be set as active, there is one already activated");
                    }
                }
                Log.Logger.Information("Client Created a new Campaign: " + input.Id);
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

                        Activate(id);//activate campaign
                    }
                    else
                    {
                        Deactivate(id); //deactivate campaign
                    }
                    Log.Logger.Information("Client Updated Campaign: " + id);
                    _campaignDB.Update(input); //Updates Campaign in DataBase 
                    break;

                } //if none found does nothing

            }
        }
        public void Delete(string id) // Delete
        {
            UpdateLocalDB();
            foreach (Campaign c in allCampaign)
            {
                if (c.Id == id.Trim().ToUpper())
                {
                    Log.Logger.Information("Client Deleted Campaign: " + id);
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

        public bool VerifyFields(CampaignDTO campaign) //Reviews all the input fields of type string to verify its correctnes, returns false if an error is found
        {
            try
            {
                if (String.IsNullOrEmpty(campaign.Name.Trim())) //Verify if name is null or empty
                {
                    //Log.Logger.Information("Error: Missing Name Value, Operation Aborted");

                    //Console.WriteLine("Ingrese un nombre");
                    return false;
                    // throw new BusinessLogic_Exceptions("Error: Missing Name Value");
                }
                if (String.IsNullOrEmpty(campaign.Description.Trim())) //Verify if description is null or empty
                {
                    //Log.Logger.Information("Error: Missing Description Value, Operation Aborted");

                    //Console.WriteLine("Ingrese una descripcion");
                    return false;
                    // throw new BusinessLogic_Exceptions("Error: Missing Description Value");
                }
                if (String.IsNullOrEmpty(campaign.Type.Trim()) || VerifyType(campaign.Type)) //Verify if type is null or invalid
                {
                    //Log.Logger.Information("Error: Incorrect Type Value, Operation Aborted");

                    //Console.WriteLine("Ingrese un Tipo Valido");
                    return false;
                    // throw new BusinessLogic_Exceptions("Error: Incorrect Type Value, just accept verano, navidad, black friday");
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
            foreach (string tipe in ValidTypes)
            {
                if (tipoMinuscula == tipe)
                {
                    //Log.Logger.Information("Error Ocurred: Incorrect value of Type , Operation Aborted");
                    return false;
                    //throw new BusinessLogic_Exceptions("Error: Valor de Tipo erróneo, sólo se aceptan navidad, black friday o verano.");
                }
            }

            return true;
            // throw new BusinessLogic_Exceptions("Error: Valor de Tipo erróneo, sólo se aceptan navidad, black friday o verano.");
        }

        public void Activate(string id) //Deactivates any active campaign present, it considers only one active at the time
        {
            UpdateLocalDB();
            foreach (Campaign c2 in allCampaign)
            {
                if (c2.Id == id.Trim())
                {
                    if (!_campaignDB.OneCampaignActive())//if no campaign is active
                    {
                        Log.Logger.Information("Client Activated Campaign: " + id);
                        c2.Active = true; //input campaign is activate
                        _campaignDB.Update(c2);

                    }
                    break;
                }
            }
        }
        public void Deactivate(string id)
        {
            UpdateLocalDB();
            foreach (Campaign c2 in allCampaign)
            {
                if (c2.Id == id)
                {
                    if (c2.Active)
                    {
                        c2.Active = false;
                        Log.Logger.Information("Client Deactivated Campaign: " + id);
                        _campaignDB.Update(c2);
                        break;
                    }
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
                    break;
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