using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Core.Models;
using Microsoft.EntityFrameworkCore;
using Web.Core.Services.Interfaces;
using Web.Core.Models.Dto;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;

namespace Web.API.Controllers
{//Nicolas Tarlao 08/04/2021
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly IContacts serviceContacts;
        public ContactsController(IContacts servicio)
        {
            serviceContacts = servicio;
        }

       
        [Route("GetAll")]
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try {
                var result = serviceContacts.GetAll();
                if (result == null) return NotFound("No se encontraron datos");
                return Ok(result);
            } catch(Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetMock")]
        [HttpGet("GetMock")]
        public async Task<IEnumerable<Contacts>> GetMock()
        {
            try
            {
                var result = await serviceContacts.GetAllModel();
                if (result == null)  throw new Exception("No se encontraron datos");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
          
        }

        [Route("GetConPaginacion")]
        [HttpGet("GetConPaginacion/")]
        public IActionResult GetConPaginacion(int page = 1, int size = 10)
        {
            try {
                var result = serviceContacts.GetConPaginacion(page, size);
                if (result == null) return NotFound("No se encontraron datos");
                return Ok(result);
            } catch (Exception ex) { return BadRequest(ex.Message); }
        }


        [Route("GetById")]
        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            try {
                if (id == 0) return BadRequest("Debe ingresar el ID");
                return Ok(serviceContacts.GetById(id));
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }


        [Route("Crear")]
        [HttpPost("Crear")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> Crear([FromBody]ContactoDTO dto)
        {
            try
            {
                
                var result = await serviceContacts.Create(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [Route("Actualizar")]
        [HttpPost("Actualizar/{id}")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Actualizar([FromRoute]int id,[FromBody]ContactoDTO dto)
        {
            try
            {
                var result = serviceContacts.Update(dto, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("Borrar")]
        [HttpPost("Borrar/{id}")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Borrar([FromRoute]int id)
        {
            try
            {
                if (id == 0) return BadRequest("Debe ingresar el ID del registro");
                var result = serviceContacts.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        


    }
}