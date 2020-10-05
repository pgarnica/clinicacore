using ClinicaHumaita.Interfaces;
using ClinicaHumaita.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaHumaita.Services
{
    public class UsersServices : IUsersServices
    {

        private readonly ClinicaContext _db;
        public ClinicaContext DbContext { get; private set; }
        public UsersServices(ClinicaContext db)
        {
            _db = db;
        }
        public async Task<Users> Create(Users user)
        {
            try
            {
                //preencher dados internos
                user.Active = true;
                user.Creation_Date = DateTime.Now;
                //criptografar a senha
                user.Password = MD5Hash(user.Password);
                //salvar no banco
                var result = await _db.User.AddAsync(user);
                await _db.SaveChangesAsync();
            }
            catch  
            {
                // retorna uma exception em caso de falha na insercao
                throw new InvalidDataException();
            }
            //retornar o objeto que foi salvo
            return user;
        }
        public async Task<Users> GetByUserName(string username)
        {
            //include para retornar os dados de person dentro do user
            var user = await _db.User.Include(x=>x.Person).FirstOrDefaultAsync(x => x.UserName == username);
            return user;
        }
        public async Task<Users> Login(string username, string password)
        {
            //include para retornar os dados de person dentro do user
            var user = await _db.User.Include(x=>x.Person).FirstOrDefaultAsync(x => x.UserName == username && x.Active == true);

            if(user != null)
            {
                //valida senha criptografada
               if (user.Password == MD5Hash(password))
                {
                    //atualiza o last login
                    user.Last_login = DateTime.Now;
                     _db.Entry(user).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                    //retorna o usuario
                    return user;
                }
            }

            return null;
        }
        public async Task<Users> Edit(Users user)
        {
            try
            {
                //criptografar a senha
                user.Password = MD5Hash(user.Password);
                //salvar no banco
                _db.Entry(user).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                //retorna o usuario
                return user;
            }
            catch
            {
                //retorna uma exception em caso de falha
                throw new InvalidDataException();
            }
        }

        public string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

    }
}
