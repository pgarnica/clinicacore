 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaHumaita.Models
{
    [Table("Person")]
    public class Person
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Nome completo")]
        [Required(ErrorMessage = "Favor preencher o campo Nome Completo.")]
        public string name { get; set; }
        [Display(Name = "E-Mail")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Favor preencher o campo E-Mail.")]
        public string email { get; set; }
    }
}
