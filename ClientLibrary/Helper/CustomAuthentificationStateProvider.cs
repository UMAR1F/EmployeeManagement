using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Helper
{
    public class CustomAuthentificationStateProvider(LocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var stringToken = await localStorageService.GetToken();
            if(string.IsNullOrEmpty(stringToken)) return await Task.FromResult(new AuthenticationState(anonymous));

            var desirializeToken = Serialization.DeserializeJsonString  <UserSession>(stringToken);
           if (desirializeToken == null) return await Task.FromResult(new AuthenticationState(anonymous));

            var getUserClaims = DecryptToken(desirializeToken.Token!);
            if (getUserClaims == null) return await Task.FromResult(new AuthenticationState(anonymous));

            var claimPricipal = SetClaimPrincipal(getUserClaims);
            return await Task .FromResult(new AuthenticationState(claimPricipal));

        }

        public async Task UpdateAuthentificationState (UserSession userSession)
        {
            var claimPrincipal = new ClaimsPrincipal();
            if (userSession.Token != null || userSession.RefreshToken != null)
            {
                var serializeSesssion = Serialization.SerializeObj(userSession);
                await localStorageService.SetToken(serializeSesssion);
                var GetUsercalaims = DecryptToken(userSession.Token!);
                claimPrincipal=SetClaimPrincipal(GetUsercalaims);

            }
            else
            {
                await localStorageService.RemoveToken();
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimPrincipal)));
        }



        public static ClaimsPrincipal SetClaimPrincipal(CustomUserClaims claims)
        {
            if (claims.Email is null) return new ClaimsPrincipal();
            return new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                {
                new (ClaimTypes.NameIdentifier, claims.Id),
                 new (ClaimTypes.Name, claims.Name!),
                  new (ClaimTypes.Email, claims.Email!),
                   new (ClaimTypes.Role, claims.Role!),

                }, "jwtAuth"
                ));
        }



        private static CustomUserClaims DecryptToken(string jwtToken)
        {
            if(string.IsNullOrEmpty(jwtToken)) return new CustomUserClaims();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var userId = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier );
            var name = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name);
            var email = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Email);
            var role = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Role);
            return new CustomUserClaims(userId!.Value!, name!.Value, email!.Value, role!.Value);
        }




    }
}
