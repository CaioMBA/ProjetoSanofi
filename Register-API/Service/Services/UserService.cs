using AutoMapper;
using Data.API;
using Data.DataBase.SqlServerDAO;
using Domain.Interfaces;
using Domain.Models;
using Domain.Utils;
using Service.Extensions;
using System.Text.RegularExpressions;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private ApiDefaultAccess _api;
        private UserDAO _dao;
        private Utils _utils;
        private IMapper _mapper;
        public UserService(ApiDefaultAccess api, UserDAO dao, Utils utils, IMapper mapper)
        {
            _api = api;
            _dao = dao;
            _utils = utils;
            _mapper = mapper;
        }

        #region Auxiliar Methods
        private bool ValidateEmail(string? email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
        private bool ValidatePhone(string? Phonenumber)
        {
            Phonenumber = Regex.Replace(Phonenumber, @"[()\-\+]", "");
            if (long.TryParse(Phonenumber, out long number) && Phonenumber.Length <= 20 && Phonenumber.Length >= 8)
            {
                return true;
            }
            return false;
        }
        private bool ValidateCPF(string? cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11)
                return false;

            int[] digits = cpf.Select(c => int.Parse(c.ToString())).ToArray();

            if (digits.All(d => d == digits[0]))
                return false;

            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += digits[i] * (10 - i);

            int firstVerificationDigit = (sum * 10) % 11;
            if (firstVerificationDigit == 10)
                firstVerificationDigit = 0;

            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += digits[i] * (11 - i);

            int secondVerificationDigit = (sum * 10) % 11;
            if (secondVerificationDigit == 10)
                secondVerificationDigit = 0;

            return firstVerificationDigit == digits[9] && secondVerificationDigit == digits[10];
        }
        private bool ValidateCNPJ(string? cnpj)
        {
            string numericCNPJ = Regex.Replace(cnpj, "[^0-9]", "");

            if (numericCNPJ.Length != 14)
            {
                return false;
            }

            int[] multiplier1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                sum += int.Parse(numericCNPJ[i].ToString()) * multiplier1[i];
            }

            int remainder = sum % 11;
            int digit1 = remainder < 2 ? 0 : 11 - remainder;

            sum = 0;
            for (int i = 0; i < 13; i++)
            {
                sum += int.Parse(numericCNPJ[i].ToString()) * multiplier2[i];
            }

            remainder = sum % 11;
            int digit2 = remainder < 2 ? 0 : 11 - remainder;

            return numericCNPJ.EndsWith(digit1.ToString() + digit2.ToString());
        }
        private DefaultResponseModel ValidatePassword(string? password)
        {
            DefaultResponseModel ValidationObject = new DefaultResponseModel();

            ValidationObject.Success = false;
            ValidationObject.Message = "";
            if (password == null)
            {
                ValidationObject.Message += "|Password is a required field|";
                return ValidationObject;
            }
            if (password.Length < 8)
            {
                ValidationObject.Message += "|Password must be at least 8 characters long|";
            }
            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                ValidationObject.Message += "|Password must have at least 1 lower case letter|";
            }
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                ValidationObject.Message += "|Password must have at least 1 upper case letter|";
            }
            if (!Regex.IsMatch(password, @"[0-9]"))
            {
                ValidationObject.Message += "|Password must have at least 1 number|";
            }
            if (!Regex.IsMatch(password, @"[!@#$%^&*()\-_+=[\]{}|;:,.<>?]"))
            {
                ValidationObject.Message += "|Password must have at least 1 special character|";
            }

            if (ValidationObject.Message.Length == 0)
            {
                ValidationObject.Success = true;
                ValidationObject.Message += "|Senha válida!|";
            }
            return ValidationObject;
        }
        #endregion

        public DefaultResponseModel Create(CreateUserRequestModel request)
        {
            DefaultResponseModel? passwordValidation = ValidatePassword(request.Password);
            if (!passwordValidation.Success)
            {
                return passwordValidation;
            }

            passwordValidation = null;

            request.Password = request.Password.ToSHA256();

            if (!ValidateCNPJ(request.Document) && !ValidateCPF(request.Document))
            {
                return new DefaultResponseModel
                {
                    Success = false,
                    Message = "|Invalid Document|"
                };
            }
            if (!ValidateEmail(request.Email))
            {
                return new DefaultResponseModel
                {
                    Success = false,
                    Message = "|Invalid Email|"
                };
            }
            if (!String.IsNullOrEmpty(request.Phone))
            {
                if (!ValidatePhone(request.Phone))
                {
                    return new DefaultResponseModel
                    {
                        Success = false,
                        Message = "|Invalid Phone|"
                    };
                }
                request.Phone = Regex.Replace(request.Phone, @"[()\-\s]", "");
            }
            if (!String.IsNullOrEmpty(request.Photo) && !request.Photo.IsBase64String())
            {
                return new DefaultResponseModel
                {
                    Success = false,
                    Message = "|Invalid Photo|"
                };
            }

            var response = _dao.Create(request);
            if (response == null)
            {
                return new DefaultResponseModel
                {
                    Success = false,
                    Message = "|Error on creating user|"
                };
            }
            return response;
        }

        public DefaultResponseModel Alter(AlterUserRequestModel request)
        {
            if (!String.IsNullOrEmpty(request.Document) && !ValidateCNPJ(request.Document) && !ValidateCPF(request.Document))
            {
                return new DefaultResponseModel()
                {
                    Success = false,
                    Message = "|Invalid Document|"
                };
            }
            if (!String.IsNullOrEmpty(request.Email) && !ValidateEmail(request.Email))
            {
                return new DefaultResponseModel()
                {
                    Success = false,
                    Message = "|Invalid Email|"
                };
            }
            if (!String.IsNullOrEmpty(request.Phone))
            {
                if (!ValidatePhone(request.Phone))
                {
                    return new DefaultResponseModel()
                    {
                        Success = false,
                        Message = "|Invalid Phone|"
                    };
                }
                request.Phone = Regex.Replace(request.Phone, @"[()\-\s]", "");
            }

            var response = _dao.Alter(request);

            if (response == null)
            {
                return new DefaultResponseModel
                {
                    Success = false,
                    Message = "|Error altering user|"
                };
            }
            return response;
        }

    }
}
