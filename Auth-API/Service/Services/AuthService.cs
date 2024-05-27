using Data.API;
using Data.DataBase.SecurityDAO;
using Domain.Interfaces;
using Domain.Models;
using Domain.Utils;
using Microsoft.IdentityModel.Tokens;
using Service.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Services
{
    public class AuthService : IAuthService
    {
        private ApiDefaultAccess _api;
        private JwtDAO _jwt;
        private Utils _utils;
        public AuthService(ApiDefaultAccess api, JwtDAO dao, Utils utils)
        {
            _api = api;
            _jwt = dao;
            _utils = utils;
        }

        public ValidatedJwtResponseModel VerifyToken(string Token)
        {
            Token = Token.Replace("Bearer ", "");
            string SecretKey = _jwt.GetKey() ?? throw new Exception("Secret key not found");
            string Issuer = _jwt.GetIssuer() ?? throw new Exception("Issuer not found");

            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var TokenHandler = new JwtSecurityTokenHandler();
            SecurityToken? ValidatedToken = null;
            try
            {
                TokenHandler.ValidateToken(Token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Issuer,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = SecurityKey
                }, out ValidatedToken);
            }
            catch (Exception ex)
            {
                return new ValidatedJwtResponseModel()
                {
                    Success = false,
                    Message = $"Token not valid, error: {ex.Message}",
                    Token = null,
                    Claims = null
                };
            }

            JwtSecurityToken JwtToken = (JwtSecurityToken)ValidatedToken;

            return new ValidatedJwtResponseModel()
            {
                Success = true,
                Message = "Token validated successfully",
                Token = Token,
                Claims = JwtToken.Claims
            };
        }

        public JwtResponseModel GenerateToken(string BasicAuth)
        {
            JwtResponseModel jwtResponseModel = new JwtResponseModel();
            try
            {
                jwtResponseModel = GenerateJwtToken(BasicAuth);
            }
            catch (Exception ex)
            {
                jwtResponseModel = new JwtResponseModel()
                {
                    Success = false,
                    Message = ex.Message,
                    Token = null,
                    Creation = null,
                    Expiration = null,
                    UserInfo = null
                };
            }
            return jwtResponseModel;
        }

        private UserInfoResponseModel ValidateUser(string BasicAuth)
        {
            string Auth = BasicAuth.Replace("Basic ", "");
            if (!Auth.IsBase64String())
            {
                throw new Exception("Invalid Authorization");
            }
            Auth = Auth.Base64ToString();
            string[] AuthArray = Auth.Split(":");
            if (AuthArray.Length != 2)
            {
                throw new Exception("Invalid Authorization");
            }

            string Login = AuthArray[0];
            string Password = AuthArray[1];

            var UserInformation = _jwt.GetInfo(Login, Password);

            if (UserInformation == null)
            {
                throw new Exception("Invalid User");
            }

            return UserInformation;
        }

        private List<UserPermissionResponseModel> GetUserPermissions(long UserID)
        {
            List<UserPermissionResponseModel>? Permissions = _jwt.GetPermisions(UserID);
            if (Permissions == null || Permissions.Count == 0)
            {
                throw new Exception("Invalid Permissions");
            }
            return Permissions;
        }

        private JwtResponseModel GenerateJwtToken(string BasicAuth)
        {
            UserInfoResponseModel User = ValidateUser(BasicAuth);

            List<UserPermissionResponseModel> Permissions = GetUserPermissions(User.UserID);

            string SecretKey = _jwt.GetKey() ?? throw new Exception("Secret key not found");
            string Issuer = _jwt.GetIssuer() ?? throw new Exception("Issuer not found");

            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> Claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, User.Login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserID", User.UserID.ToString()),
                new Claim("Name", User.Name),
                new Claim("Login", User.Login),
                new Claim("Active", User.Active.ToString()),
                new Claim("DateCreation", User.DateCreation.ToString("dd/MM/yyyy HH:mm")),
            };

            foreach (UserPermissionResponseModel Permission in Permissions)
            {
                Claims.Add(new Claim(Permission.Permission, Permission.Permitted.ToString() ?? false.ToString()));
            }

            JwtSecurityToken Token = new JwtSecurityToken(
                issuer: Issuer,
                audience: null,
                claims: Claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: Credentials
            );

            return new JwtResponseModel()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(Token),
                Creation = DateTime.Now,
                Expiration = Token.ValidTo,
                Success = true,
                Message = "Token generated successfully",
                UserInfo = User
            };
        }
    }
}
