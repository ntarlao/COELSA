using Web.Core.Models.Dto;
using Web.Core.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.Core.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
//Nicolas Tarlao 17/03/2020
namespace Web.Core.Services
{
    public class ContactsService: IContacts
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private ILogger<ContactsService> _log;

        public ContactsService(IConfiguration configuration, ILogger<ContactsService> log, IMemoryCache cache, ApplicationDbContext context)
        {
            _config = configuration;
            _log = log;
            _cache = cache;
            _context = context;
        }


        public async Task<ContactoPaginacionDTO> GetConPaginacion(int page = 1, int size = 10)
        {
            var contactos = await _context.Contacts
                           .AsNoTracking()
                           .OrderBy(p => p.LastName)
                           .Skip((page - 1) * size)
                            .Take(size)
                            .ToListAsync();

            return new ContactoPaginacionDTO
            {
                CurrentPage = page,
                TotalPages = size,
                TotalItems = contactos.Count(),
                Items = contactos.Select(p => new ContactoDTO
                {
                    id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Company = p.Company,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber
                }).ToList()
            };

        }

        public IEnumerable<ContactoDTO> GetAll()
        {
            var obj = JsonConvert.DeserializeObject<List<ContactoDTO>>(JsonConvert.SerializeObject(_context.Contacts.ToList()));
            
            return obj;
        }

        public async Task<IEnumerable<Contacts>> GetAllModel()
        {
            //var obj = JsonConvert.DeserializeObject<List<ContactoDTO>>(JsonConvert.SerializeObject(await _context.Contacts.ToListAsync()));
            var obj = await _context.Contacts.ToListAsync();

            return obj;
        }

      

        public ContactoDTO GetById(int id)
        {
            var contactos = _context.Contacts.FirstOrDefault(x => x.Id == id);

            if (contactos == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<ContactoDTO>(JsonConvert.SerializeObject(contactos));
        }

        public async Task<ContactoDTO> Create(ContactoDTO dto)
        {
            var contacto = JsonConvert.DeserializeObject<Contacts>(JsonConvert.SerializeObject(dto));

            if (_context.Contacts.Any(x => x.LastName == dto.LastName && x.Email == dto.Email)) throw new Exception("Ya existe el contacto ingresado");

            await _context.Contacts.AddAsync(contacto);
            await _context.SaveChangesAsync();

            return JsonConvert.DeserializeObject<ContactoDTO>(JsonConvert.SerializeObject(contacto)); 
        }

        public ContactoDTO Update(ContactoDTO dto, int id)
        {

            var contacto = JsonConvert.DeserializeObject<Contacts>(JsonConvert.SerializeObject(dto));

            if (contacto.Id != id)
                return null;

            _context.Contacts.Add(contacto);

            _context.Entry(contacto).State = EntityState.Modified;
            _context.SaveChanges();
            return JsonConvert.DeserializeObject<ContactoDTO>(JsonConvert.SerializeObject(contacto)); ;
        }

        public bool Delete(int id)
        {
            var usuario = _context.Contacts.FirstOrDefault(x => x.Id == id);

            if (usuario == null)
            {
                return false;
            }

            _context.Contacts.Remove(usuario);
            _context.SaveChanges();
            return true;
        }

    }
}
