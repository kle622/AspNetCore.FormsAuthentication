﻿using System.Security.Cryptography;

namespace AspNetCore.FormsAuthentication
{
    class Sha512HashProvider : HashProvider
    {
        const int Sha1HashSize = 64;
        const int Sha1KeySize = 512;

        public Sha512HashProvider(byte[] validationKey)
            : base(validationKey, Sha1HashSize, Sha1KeySize)
        {
        }

        protected override HMAC CreateHasher(byte[] key) => new HMACSHA512(key);
    }
}
