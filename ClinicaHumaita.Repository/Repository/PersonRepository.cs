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
        public async Task<Person> Add(Person newPerson)
        {
            try
            {
                //salvar no banco
                await _db.Person.AddAsync(newPerson);
                await _db.SaveChangesAsync();
                //retornar o objeto que foi salvo
                return newPerson;
            }
            catch(Exception ex)
            {
                // retorna uma exception em caso de falha na insercao
                throw ex;
            }
        }
        public async Task<Person> Update(Person personView)
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
        public async Task<bool> Delete(Person personView)
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
            return await _db.Person.Where(a => a.id.Equals(id))
                                   .FirstOrDefaultAsync();
        }
        public async Task<bool> ValidateUniqueEmail(Person person)
        {
            return await _db.Person.AnyAsync(a => (!person.id.HasValue || !a.id.Equals(person.id.Value))
                                               && a.email.Equals(person.email));
        }
        public async Task<List<Person>> GetPersons()
        {
            try
            {
                return await _db.Person.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}
