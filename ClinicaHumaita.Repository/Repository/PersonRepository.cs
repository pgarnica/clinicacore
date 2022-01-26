using ClinicaHumaita.Data.Context;
using ClinicaHumaita.Data.Interfaces;
using ClinicaHumaita.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaHumaita.Data.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ClinicaContext _db;
        public ClinicaContext DbContext { get; private set; }
        public PersonRepository(ClinicaContext db)
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
                //retornar o objeto que foi salvo
                return newItem;
            }
            catch
            {
                // retorna uma exception em caso de falha na insercao
                throw new InvalidDataException();
            }
        }
        public async Task<Person> Edit(Person personView)
        {
            try
            {
                var person = await _db.Person.FirstOrDefaultAsync(x=>x.id.Equals(personView.id));
                person.name = personView.name;
                person.email = personView.email;
                await _db.SaveChangesAsync();
                //retornar o objeto que foi salvo
                return person;
            }
            catch (Exception ex)
            {
                // retorna uma exception em caso de falha na alteracao
                throw ex;
            }
        }
        public async Task<bool> Remove(Person personView)
        {
            try
            {
                //salvar no banco
                var person = await _db.Person.FirstOrDefaultAsync(x => x.id.Equals(personView.id));
                var result = _db.Person.Remove(person);
                await _db.SaveChangesAsync();
                //retornar o objeto que foi salvo
                return true;
            }
            catch (Exception ex)
            {
                // retorna uma exception em caso de falha na remocao
                throw ex;
            }
        }
        public async Task<Person> GetById(int id)
        {
            //retorna person pelo id
            return await _db.Person.Where(a => a.id == id).Select(x=> new Person
            {
                id = x.id,
                name = x.name,
                email = x.email,
            }).FirstOrDefaultAsync();
        }
        public async Task<List<Person>> GetUsersPersons()
        {
            //retorna lista sem as pessoas que sao usuarios
            var users = await _db.Users.Select(e => e.Person).ToListAsync();
            var persons = _db.Person.ToListAsync().Result.Except(users).ToList();

            //retorna uma lista de person
            return persons;
        }
    }
}
