using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IImageRepo
    {
        Task AddNewAsync(Images image);
        Task DeleteAsync(Images image);
        Task<List<Images>> GetAllASync();
        Task UpdateAsync(Images image);
        Task<Images> GetByIdAsync(int id);
    }
}
