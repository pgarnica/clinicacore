using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Interfaces;
using ClinicaHumaita.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaHumaita.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> Create(User user)
        {
            try
            {
                //retornar o objeto que foi salvo
                return await _userRepository.Create(user);
            }
            catch(Exception ex) 
            {
                // retorna uma exception em caso de falha na insercao
                throw ex;
            }
        }
        public async Task<User> GetByUserName(string username)
        {
            try
            {
                //retornar o user pelo userName
                return await _userRepository.GetByUserName(username);
            }
            catch
            {
                // retorna uma exception em caso de falha na busca
                throw new Exception("Error while searching for user");
            }
        }
        public async Task<User> Login(string username, string password)
        {
            try 
            { 
                //retorn o usuario em caso de login com sucesso
                return await _userRepository.Login(username, MD5Hash(password));

            }catch
            {
                throw new InvalidDataException();
            }
        }
        public async Task<User> Edit(User user)
        {
            try
            {
                //criptografar o password antes de enviar para o repository
                user.Password = MD5Hash(user.Password);
                return await _userRepository.Edit(user);
            }
            catch
            {
                //retorna uma exception em caso de falha
                throw new InvalidDataException();
            }
        }
        private string MD5Hash(string text)
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
        public async Task<User> Remove(User user)
        {
            try
            {
                return await _userRepository.Remove(user);
            }
            catch
            {
                //retorna uma exception em caso de falha
                throw new InvalidOperationException();
            }
        }
    }
}
