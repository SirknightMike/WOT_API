using wot_api.Entities;

namespace wot_api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Users GetById(int userId);
        IEnumerable<Users> GetAll();
        void Add(Users user);
        void Update(Users user);
        void Delete(int userId);
    }
}
