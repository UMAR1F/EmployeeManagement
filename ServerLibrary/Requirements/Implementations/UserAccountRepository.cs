using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using ServerLibrary.Data;
using ServerLibrary.Helper;
using ServerLibrary.Requirements.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Constants = ServerLibrary.Helper.Constants;

namespace ServerLibrary.Requirements.Implementations
{
    public class UserAccountRepository(IOptions<JwtSection> config, AppDbContext appDbContext) : IUserAccount
    {
        public async Task<GeneralResponse> CreateAsync(Register user)
        {
            if (user is null) return new GeneralResponse(false, "Model is Empty");
            var checkUser = await FindUserByEmail(user.Email!);
            if (checkUser != null) return new GeneralResponse(false, " User Registered Already");

            // save User
            var applicationUser = await AddtoDatabase(new AppUser()
            {
                FullName = user.FullName,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
            });

            //Check, Create, Asign role
            var cheackAdminrole = await appDbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Name!.Equals(Constants.Admin));
            if (cheackAdminrole is null)
            {
                var createAdminRole = AddtoDatabase(new SystemRole() { Name = Constants.Admin });   
                await AddtoDatabase(new UserRole() { RoleId=createAdminRole.Id, UserId=applicationUser.Id });
                return new GeneralResponse(true, "Account Created");
            }


            var  checkuseRole=await appDbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Name!.Equals(Constants.User));
            SystemRole response = new();

            if (checkuseRole is null)
            {
                response = await AddtoDatabase(new SystemRole() { Name = Constants.Admin });
                await AddtoDatabase(new UserRole() { RoleId = response.Id, UserId = applicationUser.Id });
            }
            else
            {
                await AddtoDatabase(new UserRole() { RoleId= checkUser.Id, UserId = applicationUser.Id });
            }
            return new GeneralResponse(true, "Account Created");
        }




      

        private async Task<AppUser> FindUserByEmail(string email) =>
        await appDbContext.ApplicationUser.FirstOrDefaultAsync(_ => _.Email!.ToLower()!.Equals(email!.ToLower()));

        private async Task<T> AddtoDatabase <T>(T model)
        {
            var result = appDbContext.Add(model!);
                await appDbContext.SaveChangesAsync();
            return (T)result.Entity;
        }




        public async Task<LoginResponse> SignInAsync(Login user)
        {
            if (user is null) return new LoginResponse(false, "Model is Empty");
            var applicationUser = await FindUserByEmail(user.Email!);
            if (applicationUser is null) return new LoginResponse(false, "User Not Found");

            //Verify Password
            if (!BCrypt.Net.BCrypt.Verify(user.Password, applicationUser.Password))
                return new LoginResponse(false, "Email or password Not Found");

            var getUserRole =await FindUserRole( applicationUser.Id);
            if (getUserRole is null) return new LoginResponse(false, "User Role Found Now");

            var getRoleName = await FindRoleName(getUserRole.RoleId);
            if (getUserRole is null) return new LoginResponse(false, "User Role Found Now");

            string jwtToken = GenerateToken(applicationUser, getRoleName!.Name!);
            string refreashToken = GenerateRefreshToken();

            //Save the refresh Token To the database
            var findUser = await appDbContext.RefreshTokenInfo.FirstOrDefaultAsync(_ => _.UserId == applicationUser.Id);
            if(findUser is not null)
            {
                findUser!.Token = refreashToken;
                await appDbContext.SaveChangesAsync();
            }
            else
            {
                await AddtoDatabase(new RefreshTokenInfo()
                {
                    Token = refreashToken,
                    UserId = applicationUser.Id
                });
                return new LoginResponse(true, "Login Sucessfully", jwtToken, refreashToken);
            }

            return new LoginResponse(true, "Login Sucessfully", jwtToken, refreashToken);

        }
        private string GenerateToken(AppUser user, string role)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.Key!));
            var creadencial=new SigningCredentials(securitykey,SecurityAlgorithms.HmacSha256);

            var userCliams = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role,role),

            };

            var token = new JwtSecurityToken(
                issuer: config.Value.Issuer,
                audience: config.Value.Audience,
                claims:userCliams,
                expires:DateAndTime.Now.AddDays(1),
                signingCredentials:creadencial
               
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private async Task<UserRole> FindUserRole(int userId) => await appDbContext.UserRoles.FirstOrDefaultAsync(_ => _.UserId == userId);
        private async Task<SystemRole> FindRoleName(int roleId) => await appDbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Id == roleId);
        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        public async Task<LoginResponse> RefreshToken(RefreshToken token)
        {
            if (token is null) return new LoginResponse(false, "Model Is Empty");

            var findtoken = await appDbContext.RefreshTokenInfo.FirstOrDefaultAsync(_=>_.Token!.Equals(token.Token));
            if (findtoken is null) return new LoginResponse(false, "Refresh Token Required");

            //Get user details
            var user=await appDbContext.ApplicationUser.FirstOrDefaultAsync(_=>_.Id== findtoken.UserId);
            if (user is null) return new LoginResponse(false, "Refresh Token Could Not Be Generated Because user is Found ");

            var userRole=await FindUserRole(user.Id);
            var roleName=await FindRoleName(userRole.RoleId);
            string jwtToken = GenerateToken(user, roleName.Name!);
            string refresToken = GenerateRefreshToken();


            var updateRefreshToken = await appDbContext.RefreshTokenInfo.FirstOrDefaultAsync(_ => _.UserId == user.Id);
            if (updateRefreshToken is null) return new LoginResponse(false, "Refresh Token Culd Not Be  Generated Because User Has Not SignIn");

            updateRefreshToken.Token = refresToken;
            await appDbContext.SaveChangesAsync();
            return new LoginResponse(true, "Token Refresh Successfully", jwtToken, refresToken);





        }
    }
}
