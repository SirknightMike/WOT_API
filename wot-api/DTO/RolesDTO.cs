using wot_api.Entities;

namespace wot_api.DTO
{
    public class RolesDTO
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public ICollection<Participant> Participants
        {
            get; set;
        }
    }
}
