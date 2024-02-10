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

        public IEnumerable<Users> GetAll()
        {
            return _context.Users.ToList();
        }

        public Users GetById(int id) 
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
        public void Add(Users user) 
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public void Update(Users user) 
        { 
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public void Delete(int id) 
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
