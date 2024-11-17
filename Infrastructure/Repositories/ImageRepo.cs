using Application.Repositories;
using Domain;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal class ImageRepo : IImageRepo
    {
        private readonly ApplicationDbContext _context;

        public ImageRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddNewAsync(Images image)
        {
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Images image)
        {
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Images>> GetAllASync()
        {
            return await _context.Images
                .Include(i => i.Property)
                .ToListAsync();
        }

        public Task<Images> GetByIdAsync(int id)
        {
            return _context.Images
                .Include(i => i.Property)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Images image)
        {
            _context.Images.Update(image);
            await _context.SaveChangesAsync();
        }
    }
}
