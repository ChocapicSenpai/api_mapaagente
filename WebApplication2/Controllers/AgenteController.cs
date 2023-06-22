    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Data;
    using System.Data.SqlClient;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using WebApplication2.Models;
    using System.Net;
    using static WebApplication2.Controllers.AgenteController;
    using Newtonsoft.Json;
    using WebApplication2.Modelos;
    using Microsoft.Extensions.Options;
    using Microsoft.AspNetCore.Authorization;

namespace WebApplication2.Controllers

    {
        [Route("Agente")]
        [ApiController]
        public class AgenteController : ControllerBase
        {
            private readonly IConfiguration _configuration;
            public AgenteController(IConfiguration configuration)
            {
                _configuration = configuration;

            }




            /*private readonly string _recaptchaSecretKey = "6Lfeqq8mAAAAADtUOnfc4FIS-tEMZ4GsfVWqU79N";*/

            //// Reemplaza esto con tu llave secreta de reCAPTCHA

            ////private readonly IConfiguration _configuration;
            ////private readonly RecaptchaSettings _recaptchaSettings;


            ////public AgenteController(IConfiguration configuration, IOptions<RecaptchaSettings> recaptchaSettings)
            ////{
            ////    _configuration = configuration;
            ////    _recaptchaSettings = recaptchaSettings.Value;
            ////}



            [HttpGet]
            public JsonResult Get()
            {
                string query = @"
                                SELECT A.AgenteId, A.NombreAgente, A.Tlf,A.Ubigeo,U.NumeroUbigeo, A.Direccion, A.latlng, A.estado
                                FROM dbo.Agente A
                                INNER JOIN dbo.Ubigeo U ON A.Ubigeo = U.UbigeoId
                            
                                ";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("AgenteAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult(table);
            }

            /////
            //// Método para validar el token de reCAPTCHA
            //private async Task<bool> ValidateCaptcha(string captcha)
            //{
            //    using (HttpClient httpClient = new HttpClient())
            //    {
            //        string url = $"https://www.google.com/recaptcha/api/siteverify?secret={_recaptchaSettings.SecretKey}&response={captcha}";

            //        HttpResponseMessage response = await httpClient.GetAsync(url);

            //        if (response.IsSuccessStatusCode)
            //        {
            //            var resultString = await response.Content.ReadAsStringAsync();
            //            var result = JsonConvert.DeserializeObject<RecaptchaResponse>(resultString);
            //            return result.success;
            //        }

            //        return false;
            //    }
            //}
            //public class RecaptchaResponse
            //{
            //    public bool success { get; set; }
            //    public DateTime challenge_ts { get; set; }
            //    public string hostname { get; set; }
            //    [JsonProperty("error-codes")]
            //    public string[] error_codes { get; set; }
            //}

            [HttpPost]
            [Authorize]
            ////public async Task<IActionResult> Post(Agente age)
            public JsonResult Post(Agente age)
            {
            if (
                string.IsNullOrEmpty(age.NombreAgente) ||
                string.IsNullOrEmpty(age.Tlf) ||
                string.IsNullOrEmpty(age.Ubigeo) ||
                string.IsNullOrEmpty(age.Direccion)||
                string.IsNullOrEmpty(age.latlng) ||
                string.IsNullOrEmpty(age.estado))
            {
                return new JsonResult("Error: Todos los campos son requeridos");
            }


            ////string captcha = age.captcha;

            ////if (await ValidateCaptcha(captcha))
            ////{ 

            string query = @"
                                insert into dbo.Agente
                                values(@NombreAgente,@Tlf,@Ubigeo,@Direccion,@latlng,@estado)
                                ";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("AgenteAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@NombreAgente", age.NombreAgente);
                        myCommand.Parameters.AddWithValue("@Tlf", age.Tlf);
                        myCommand.Parameters.AddWithValue("@Ubigeo", age.Ubigeo);
                        myCommand.Parameters.AddWithValue("@Direccion", age.Direccion);
                        myCommand.Parameters.AddWithValue("@latlng", age.latlng);
                        myCommand.Parameters.AddWithValue("@estado", age.estado);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult("Se añadio correctamente");
            }
            ////    else
            ////    {
            ////        return BadRequest("Actividades del captcha no completados");
            ////    }
            ////}



            [HttpPut]
            [Authorize]

            public JsonResult Put(Agente age)
            {
            if (
                string.IsNullOrEmpty(age.NombreAgente) ||
                string.IsNullOrEmpty(age.Tlf) ||
                string.IsNullOrEmpty(age.Ubigeo) ||
                string.IsNullOrEmpty(age.Direccion) ||
                string.IsNullOrEmpty(age.latlng) ||
                string.IsNullOrEmpty(age.estado))
            {
                return new JsonResult("Error: Todos los campos son requeridos");
            }
            string query = @"
                                update dbo.Agente
                                set NombreAgente=@NombreAgente,
                                Tlf=@Tlf,
                                Ubigeo=@Ubigeo,
                                Direccion=@Direccion,
                                latlng=@latlng,
                                estado=@estado
                                where AgenteId=@AgenteId
                                ";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("AgenteAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@AgenteId", age.AgenteId);
                        myCommand.Parameters.AddWithValue("@NombreAgente", age.NombreAgente);
                        myCommand.Parameters.AddWithValue("@Tlf", age.Tlf);
                        myCommand.Parameters.AddWithValue("@Ubigeo", age.Ubigeo);
                        myCommand.Parameters.AddWithValue("@Direccion", age.Direccion);
                        myCommand.Parameters.AddWithValue("@latlng", age.latlng);
                        myCommand.Parameters.AddWithValue("@estado", age.estado);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult("Se modifico correctamente");
            }
            [HttpDelete("{id}")]
            [Authorize]
            public JsonResult Delete(int id)
            {
                string query = @"
                                delete from dbo.Agente
                                where AgenteId=@AgenteId
                                ";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("AgenteAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@AgenteId", id);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult("Se elimino correctamente");
            }
        }


    }