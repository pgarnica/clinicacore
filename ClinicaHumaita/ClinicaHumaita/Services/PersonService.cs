using ClinicaHumaita.Interfaces;
using ClinicaHumaita.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaHumaita.Services
{
    public class PersonService : IPersonServices
    {
        private readonly ClinicaContext _db;
        public ClinicaContext DbContext { get; private set; }
        public PersonService(ClinicaContext db)
        {
            _db = db;
        }
        public async Task<Person> Add(Person newItem)
        {
            try
            {
                await _db.Person.AddAsync(newItem);
                await _db.SaveChangesAsync();
                return newItem;
            }catch
            {
                throw new InvalidDataException();
            }
        }
        public async Task<Person> GetById(int id)
        {
            return await _db.Person.Where(a => a.id == id).FirstOrDefaultAsync();
        }
        public async Task<List<Person>> Get()
        {
            return await _db.Person.ToListAsync();
        }
    }
}
