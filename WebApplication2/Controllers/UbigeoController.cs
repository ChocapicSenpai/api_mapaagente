using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("Ubigeo")]
    [ApiController]
    public class UbigeoController : Controller
    {
        private readonly IConfiguration _configuration;
        public UbigeoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()

        {
            string query = @"
                            select NumeroUbigeo, DepartamentoUbigeo,ProvinciaUbigeo,DistritoUbigeo from
                            dbo.Ubigeo
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

        [HttpPost]
        public JsonResult Post(Ubigeo ubi)
        {
            string query = @"
                            insert into dbo.Ubigeo
                            values(@NumeroUbigeo,@DepartamentoUbigeo,@ProvinciaUbigeo,@DistritoUbigeo)
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AgenteAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@NumeroUbigeo", ubi.NumeroUbigeo);
                    myCommand.Parameters.AddWithValue("@DepartamentoUbigeo", ubi.DepartamentoUbigeo);
                    myCommand.Parameters.AddWithValue("@ProvinciaUbigeo", ubi.ProvinciaUbigeo);
                    myCommand.Parameters.AddWithValue("@DistritoUbigeo", ubi.DistritoUbigeo);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Se añadio correctamente");
        }


        [HttpPut]
        public JsonResult Put(Ubigeo ubi)
        {
            string query = @"
                            update dbo.Ubigeo
                            set NumeroUbigeo=@NumeroUbigeo,
                            DepartamentoUbigeo=@DepartamentoUbigeo,
                            ProvinciaUbigeo=@ProvinciaUbigeo,
                            DistritoUbigeo=@DistritoUbigeo
                            where UbigeoId=@UbigeoId
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AgenteAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@UbigeoId", ubi.UbigeoId);
                    myCommand.Parameters.AddWithValue("@NumeroUbigeo", ubi.NumeroUbigeo);
                    myCommand.Parameters.AddWithValue("@DepartamentoUbigeo", ubi.DepartamentoUbigeo);
                    myCommand.Parameters.AddWithValue("@ProvinciaUbigeo", ubi.ProvinciaUbigeo);
                    myCommand.Parameters.AddWithValue("@DistritoUbigeo", ubi.DistritoUbigeo);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Se modifico correctamente");
        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                            delete from dbo.Ubigeo
                            where UbigeoId=@UbigeoId
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AgenteAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@UbigeoId", id);
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
