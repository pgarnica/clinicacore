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

        [Display(Name ="Usuário")]
        [Required(ErrorMessage = "Favor preencher o campo Usuário.")]
        public string UserName { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Favor preencher o campo Senha.")]
        public string Password { get; set; }

        public DateTime Creation_Date { get; set; }

        public DateTime? Last_login { get; set; }

        public bool Active { get; set; }

        public virtual Person Person { get; set; }

    }
}
