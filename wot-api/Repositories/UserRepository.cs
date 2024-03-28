using Microsoft.EntityFrameworkCore;
using wot_api.Data;
using wot_api.Entities;
using wot_api.Repositories.Interfaces;

namespace wot_api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context) 
        {
            _context = context;
        }

        public async IEnumerable<Users> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Users GetById(int id) 
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        public async void Add(Users user) 
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async void Update(Users user) 
        { 
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async void Delete(int id) 
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
