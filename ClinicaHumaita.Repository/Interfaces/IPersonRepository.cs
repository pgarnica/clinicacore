﻿using ClinicaHumaita.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicaHumaita.Data.Interfaces
{
    public interface IPersonRepository
    {
        Task<Person> Create(Person newItem);
        Task<Person> Edit(Person person);
        Task<bool> Remove(Person person);
        Task<Person> GetById(int id);
        Task<List<Person>> GetUsersPersons();
    }
}
