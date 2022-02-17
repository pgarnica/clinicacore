using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Interfaces;
using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Shared.ViewModels;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaHumaita.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonService _personService;
        public UserService(IUserRepository userRepository,
                           IPersonService personService,
                           INotificationService notificationService) : base(notificationService)
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
                    ErrorNotification(HttpStatusCode.BadRequest, "The given username is already in use.");
                    return null;
                }

                var person = await _personService.Add(new PersonAddViewModel { Name = userAdd.Name, Email = userAdd.Email });
                var user = new User
                {
                    Password = MD5Hash(userAdd.Password),
                    PersonId = person.id.Value,
                    UserName = userAdd.Username,
                    Active = true,
                    Creation_Date = DateTime.UtcNow,
                };

                var createdUser =  await _userRepository.Add(user);
                createdUser.Password = "";
                return createdUser;

            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }
        public async Task<User> GetByUserName(string username)
        {
            try
            {
                return await _userRepository.GetByUserName(username);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<User> ValidateUser(string username, string password)
        {
            try 
            { 
                return await _userRepository.Login(username, MD5Hash(password));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<User> Update(UserUpdateViewModel userUpdate)
        {
            try
            {
                if (!userUpdate.Id.HasValue)
                {
                    ErrorNotification(HttpStatusCode.BadRequest, "The id field is required.");
                    return null;
                }

                if (await _userRepository.CheckExistingUserName(userUpdate.Username, userUpdate.Id))
                {
                    ErrorNotification(HttpStatusCode.BadRequest, "The given username is already in use.");
                    return null;
                }

                var user = await _userRepository.GetById(userUpdate.Id.Value);
                var person = await _personService.Update(new PersonUpdateViewModel 
                { 
                    Id = user.Person.id, 
                    Name = userUpdate.Name, 
                    Email = userUpdate.Email 
                });

                user.UserName = userUpdate.Username;
                user.Active = userUpdate.Active;

                var updatedUser = await _userRepository.Update(user);
                updatedUser.Password = "";
                return updatedUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<User> Delete(UserDeleteViewModel userDelete)
        {
            try
            {
                if (!userDelete.Id.HasValue)
                {
                    ErrorNotification(HttpStatusCode.BadRequest, "The id field is required.");
                    return null;
                }

                var user = await _userRepository.GetById(userDelete.Id.Value);
                if (user == null)
                {
                    ErrorNotification(HttpStatusCode.NotFound, "User not found.");
                    return null;
                }

                var deletedUser = await _userRepository.Delete(new User { Id = userDelete.Id.Value });
                deletedUser.Password = "";

                return deletedUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}
