using System;
using System.Security.Cryptography;
using System.Text;
using wot_api.DTO;
using wot_api.Entities;


namespace wot_api.Classes
{
    public class DataProtection
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 10000;

        public PasswordEncryptionDTO HashPassword(Users user)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                
                var salt = new byte[SaltSize];
                rng.GetBytes(salt);
               
                using (var pbkdf2 = new Rfc2898DeriveBytes(user.Password, salt, Iterations, HashAlgorithmName.SHA256))
                {
                    var hash = pbkdf2.GetBytes(KeySize);
                    var result = new byte[SaltSize + KeySize];
                    Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
                    Buffer.BlockCopy(hash, 0, result, SaltSize, KeySize);

                    return new PasswordEncryptionDTO
                    {
                        HashPassword = Convert.ToBase64String(result),
                        Salt = salt,

                    };

                    
                }

            }
        }

        public string VerifyHashPassword(string Password, byte[] Salt )
        {
            using (var rng = new RNGCryptoServiceProvider())
            {

                var salt = Salt;

                using (var pbkdf2 = new Rfc2898DeriveBytes(Password, salt, Iterations, HashAlgorithmName.SHA256))
                {
                    var hash = pbkdf2.GetBytes(KeySize);
                    var result = new byte[SaltSize + KeySize];
                    Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
                    Buffer.BlockCopy(hash, 0, result, SaltSize, KeySize);

                    return Convert.ToBase64String(result);


                }

            }
        }


    }
}
