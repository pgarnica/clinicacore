using ClinicaHumaita.Data.Context;
using ClinicaHumaita.Data.Interfaces;
using ClinicaHumaita.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaHumaita.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ClinicaContext _db;
        public ClinicaContext DbContext { get; private set; }
        public UserRepository(ClinicaContext db)
        {
            _db = db;
        }
        public async Task<User> Create(User user)
        {
            try
            {
                //preencher dados internos
                user.Active = true;
                user.Creation_Date = DateTime.Now;
                //criptografar a senha
                user.Password = user.Password;
                //salvar no banco
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();

                //retornar o objeto que foi salvo
                return user;
            }
            catch (Exception ex) 
            {
                // retorna uma exception em caso de falha na insercao
                throw ex;
            }
        }
        public async Task<User> GetByUserName(string username)
        {
            //include para retornar os dados de person dentro do user
            var user = await _db.Users.Include(x=>x.Person).FirstOrDefaultAsync(x => x.UserName == username && x.Active);
            return user;
        }
        public async Task<User> Login(string username, string password)
        {
            try
            {
                //include para retornar os dados de person dentro do user
                return await _db.Users.Include(x => x.Person)
                                      .FirstOrDefaultAsync(x => x.UserName.Equals(username)
                                                             && x.Active
                                                             && x.Password.Equals(password));
            }
            catch
            {
                throw new InvalidDataException();
            }
        }

        public async Task<User> Edit(User user)
        {
            try
            {
                 
                //atualizar o user
                var entryUser = _db.Users.FirstOrDefault(e => e.Id == user.Id);
                _db.Entry(entryUser).CurrentValues.SetValues(user);
                 

                //atualizar a person
                var entryPerson = _db.Person.FirstOrDefault(x => x.id == user.PersonId);
                entryPerson.name = user.Person.name;
                entryPerson.email = user.Person.email;
                _db.Entry(entryPerson).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                //buscar o user atualizado com suas dependecias 
                entryUser = _db.Users.Include(x=>x.Person).FirstOrDefault(e => e.Id == user.Id);
                //retorna o usuario
                return entryUser;
            }
            catch
            {
                //retorna uma exception em caso de falha
                throw new InvalidDataException();
            }
        }
      
        public async Task<User> Remove(User user)
        {
            try
            {
                // exclusao logica do user
                user.Active = false;
                //atualizar o user
                var entryUser = _db.Users.FirstOrDefault(e => e.Id == user.Id);
                _db.Entry(entryUser).CurrentValues.SetValues(user);
                await _db.SaveChangesAsync();

                //buscar o user atualizado com suas dependecias 
                entryUser = _db.Users.Include(x => x.Person).FirstOrDefault(e => e.Id == user.Id);
                //retorna o usuario
                return entryUser;
            }
            catch (Exception ex)
            {
                //retorna uma exception em caso de falha
                throw ex;
            }
        }

        public async Task<bool> PersonIsUser(int personId)
        {
            return  await _db.Users.AnyAsync(x => x.Person.id.Equals(personId));
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}
