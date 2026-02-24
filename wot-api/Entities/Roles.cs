using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wot_api.Entities
{
    public class Roles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleID { get; set; }

        [Required]
        [StringLength(50)] // Role name should not exceed 50 characters
        public string RoleName { get; set; }

        [StringLength(250)] // Optional role description with a max length of 250 characters
        public string RoleDescription { get; set; }

        // Navigation property to link the Role with participants (many participants can have the same role)
        public ICollection<Participant> Participants { get; set; }

    }
}
