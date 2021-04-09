using Web.Core.Models;
using Web.Core.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Core.Services.Interfaces
{
    public interface IContacts
    {
     
        IEnumerable<ContactoDTO> GetAll();
        Task<ContactoPaginacionDTO> GetConPaginacion(int page = 1, int size = 10);
        ContactoDTO GetById(int id);
        Task<ContactoDTO> Create(ContactoDTO dto);
        ContactoDTO Update(ContactoDTO dto, int id);
        bool Delete(int id);
        Task<IEnumerable<Contacts>> GetAllModel();



    }
}
