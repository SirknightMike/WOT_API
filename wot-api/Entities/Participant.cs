using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace wot_api.Entities
{
    public class Participant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int UserID { get; set; } // Foreign key for User

        [Required]
        public int RoleID { get; set; } // Foreign key for Role
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime JoinedDate { get; set; } // Date the participant joined the competition

        public virtual Users Users { get; set; }
        public virtual Roles Roles { get; set; }
        public virtual Competition Competition { get; set; }
        public virtual ICollection<ParticipantScore> ParticipantScores { get; set; }


    }
}
