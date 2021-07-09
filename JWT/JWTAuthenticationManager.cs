using Microsoft.IdentityModel.Tokens;
using MttoApi.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MttoApi
{
    public interface IJWTAuthenticationManager
    {
        string Authenticate(LogInRequest request);
    }

    public class JWTAuthenticationManager : IJWTAuthenticationManager
    {
        //CLAVE SECRETA PARA LA ENCRIPTACION
        private readonly string key = string.Empty;

        //CONSTRUCTOR DE LA CLASE
        public JWTAuthenticationManager(string secretkey)
        {
            this.key = secretkey;
        }

        //METODO DE AUTENTICACION
        public string Authenticate(LogInRequest request)
        {
            //CREAMOS Y DEFINIMOS LAS VARIABLES QUE FUNCIONARAN PARA CONFIGURAR LA INFORMACION DEL TOKEN A GENERAR
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescription = new SecurityTokenDescriptor
            {
                //SUBJECT: GETs OR SETs THE "ClaimsIdentity"
                //---------------------------------------------------------------------------------
                //LA CLASE "Claims Identity" (ClaimsIdentity) ES UNA IMPLEMENTACION CONCRETA DE
                //LAS IDENTIDADES BASADAS EN "Claims" O "Demandas", QUE ES, UNA IDENTIDAD DESCRITA
                //POR UNA COLECCION DE "Claims".
                //---------------------------------------------------------------------------------
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                 {
                         // Los "Claims" CONTENIDOS EN UNA CLASE "ClaimsIdentity" DESCRIBEN LA ENTIDAD
                         // CORRESPONDIENTE A LAS IDENTIDADES REPRESENTADAS, Y SON USADAS PARA TOMAR
                         // DESICIONES DE AUTORIZACION Y DE AUTENTICACION
                         new Claim(ClaimTypes.Name, request.Username),
                 }),
                //EXPIRES: GETs OR SETs THE VALUE OF THE "expiration" claim.
                Expires = DateTime.UtcNow.AddHours(1),

                //SIGNINGCREDENTIALS: GETs OR SETs THE "SigningCredentials" USED TO CREATE
                //A SECURITY TOKEN.
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                 SecurityAlgorithms.HmacSha256),
            };

            //CREAMOS EL TOKEN CON LA CONFIGURACION PROPORCIONADA EN EL "TokenDescriptor"
            var token = tokenHandler.CreateToken(tokenDescription);

            //RETORNAMOS EL TOKEN EN FOMA DE STRING.
            return tokenHandler.WriteToken(token);
        }
    }
}