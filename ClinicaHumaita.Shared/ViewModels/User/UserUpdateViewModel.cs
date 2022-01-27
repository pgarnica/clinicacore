using System;

namespace ClinicaHumaita.Shared.ViewModels
{
    public class UserUpdateViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public bool Active { get; set; }
    }
}
