using ClinicaHumaita.Interfaces;
using ClinicaHumaita.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
        public async Task<Person> Create(Person newItem)
        {
            try
            {
                //salvar no banco
                var result = await _db.Person.AddAsync(newItem);
                await _db.SaveChangesAsync();
            }
            catch
            {
                // retorna uma exception em caso de falha na insercao
                throw new InvalidDataException();
            }
            //retornar o objeto que foi salvo
            return newItem;
        }

        public async Task<Person> Edit(Person person)
        {
            try
            {
                //salvar no banco
                _db.Entry(person).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            catch
            {
                // retorna uma exception em caso de falha na insercao
                throw new InvalidDataException();
            }
            //retornar o objeto que foi salvo
            return person;
        }

        public async Task<bool> Remove(Person person)
        {
            try
            {
                //salvar no banco
                var result = _db.Person.Remove(person);
                await _db.SaveChangesAsync();
            }
            catch
            {
                // retorna uma exception em caso de falha na insercao
                throw new InvalidDataException();
            }
            //retornar o objeto que foi salvo
            return true;
        }

        public async Task<Person> GetById(int id)
        {
            //retorna person pelo id
            return await _db.Person.Where(a => a.id == id).FirstOrDefaultAsync();
        }
        public async Task<List<Person>> Get()
        {
            //retorna lista sem as pessoas que sao usuarios
            var users = await _db.Users.Select(e => e.Person).ToListAsync();
            var persons = _db.Person.ToListAsync().Result.Except(users).ToList();

            //retorna uma lista de person
            return persons;
        }
    }
}
