using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Interfaces;
using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Shared.ViewModels;
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
        private readonly IPersonService _personService;
        public UserService(IUserRepository userRepository,
                           IPersonService personService)
        {
            _userRepository = userRepository;
            _personService = personService;
        }
        public async Task<User> Add(UserAddViewModel userAdd)
        {
            try
            {
                if (await _userRepository.CheckExistingUserName(userAdd.Username, null))
                {
                    throw new Exception("The given username is already in use.");
                }

                var person = await _personService.Add(new PersonAddViewModel { Name = userAdd.Name, Email = userAdd.Email });
                var user = new User
                {
                    Password = MD5Hash(userAdd.Password),
                    PersonId = person.id.Value,
                    UserName = userAdd.Username,
                };

                //retornar o objeto que foi salvo
                var createdUser =  await _userRepository.Add(user);
                createdUser.Password = "";
                return createdUser;

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
            catch (Exception ex)
            {
                // retorna uma exception em caso de falha na insercao
                throw ex;
            }
        }
        public async Task<User> ValidateUser(string username, string password)
        {
            try 
            { 
                //retorn o usuario em caso de login com sucesso
                return await _userRepository.Login(username, MD5Hash(password));

            }
            catch (Exception ex)
            {
                // retorna uma exception em caso de falha na insercao
                throw ex;
            }
        }
        public async Task<User> Update(UserUpdateViewModel userUpdate)
        {
            try
            {
                if (!userUpdate.Id.HasValue)
                {
                    throw new Exception("The id field is required.");
                }

                if (await _userRepository.CheckExistingUserName(userUpdate.Username, userUpdate.Id))
                {
                    throw new Exception("The given username is already in use.");
                }

                var user = await _userRepository.GetById(userUpdate.Id.Value);
                var person = await _personService.Update(new PersonUpdateViewModel 
                { 
                    Id = user.Person.id, 
                    Name = userUpdate.Name, 
                    Email = userUpdate.Email 
                });

                user.UserName = userUpdate.Name;
                user.Active = userUpdate.Active;

                var updatedUser = await _userRepository.Update(user);
                updatedUser.Password = "";
                return updatedUser;
            }
            catch (Exception ex)
            {
                // retorna uma exception em caso de falha na insercao
                throw ex;
            }
        }
        public async Task<User> Delete(UserDeleteViewModel userDelete)
        {
            try
            {
                if (!userDelete.Id.HasValue)
                {
                    throw new Exception("The id field is required.");
                }

                var user = await _userRepository.GetById(userDelete.Id.Value);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                var deletedUser = await _userRepository.Delete(new User { Id = userDelete.Id.Value });
                deletedUser.Password = "";

                return deletedUser;
            }
            catch (Exception ex)
            {
                // retorna uma exception em caso de falha na insercao
                throw ex;
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
    }
}
