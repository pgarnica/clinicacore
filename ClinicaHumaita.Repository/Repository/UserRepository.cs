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
        public async Task<User> Add(User user)
        {
            try
            {
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
                return user;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
        public async Task<User> GetByUserName(string username)
        {
            return await _db.Users.Include(x=>x.Person).FirstOrDefaultAsync(x => x.UserName == username && x.Active);
        }
        public async Task<User> GetById(int id)
        {
            return await _db.Users.Include(x => x.Person).FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        public async Task<bool> CheckExistingUserName(string username, int? id)
        {
            return await _db.Users.AnyAsync(x => (!id.HasValue || !x.Id.Equals(id.Value)) 
                                              && x.UserName.Equals(username));
        }
        public async Task<User> Login(string username, string password)
        {
            try
            {
                return await _db.Users.Include(x => x.Person)
                                      .FirstOrDefaultAsync(x => x.UserName.Equals(username)
                                                             && x.Active
                                                             && x.Password.Equals(password));
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<User> Update(User user)
        {
            try
            {
                var entryUser = _db.Users.FirstOrDefault(e => e.Id == user.Id);
  
                entryUser.UserName = user.UserName;
                entryUser.Person.name = user.Person.name;
                entryUser.Person.email = user.Person.email;
                _db.Entry(entryUser).State = EntityState.Modified;

                await _db.SaveChangesAsync();

                return entryUser;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<User> Delete(User user)
        {
            try
            {
                var entryUser = _db.Users.FirstOrDefault(e => e.Id == user.Id);

                entryUser.Active = false;
                _db.Entry(entryUser).State = EntityState.Modified;

                await _db.SaveChangesAsync();

                return entryUser;
            }
            catch (Exception ex)
            {
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
