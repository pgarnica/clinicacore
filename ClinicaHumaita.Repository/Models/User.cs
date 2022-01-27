using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaHumaita.Data.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        public int PersonId { get; set; }

        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public DateTime Creation_Date { get; set; }

        public DateTime? Last_login { get; set; }

        public bool Active { get; set; }

        public virtual Person Person { get; set; }

    }
}
