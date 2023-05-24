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
    [Route("Agente")]
    [ApiController]
    public class AgenteController : Controller
    {
        private readonly IConfiguration _configuration;
        public AgenteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select NombreAgente, Tlf,Ubigeo,Direccion,latlng,estado from
                            dbo.Agente
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
        public JsonResult Post(Agente age)
        {
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

       
        [HttpPut]
        public JsonResult Put(Agente age)
        {
            string query = @"
                            update dbo.Agente
                            set NombreAgente=@NumeroUbigeo,
                            Tlf=@DepartamentoUbigeo,
                            Ubigeo=@ProvinciaUbigeo,
                            Direccion=@DistritoUbigeo,
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
