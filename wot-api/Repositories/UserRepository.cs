using wot_api.Entities;
using wot_api.Repositories.Interfaces;

namespace wot_api.Repositories
{
    public class UserRepository : IUserRepository
    {


        public UserRepository() { }

        public Users GetById(int id) 
        {
            var user = new Users();
            return user;
        }
        public void Add(Users user) 
        {
        
        }
        public void Update(Users user) 
        { 
        
        }
        public void Delete(int Id) 
        { 
        
        }
    }
}
