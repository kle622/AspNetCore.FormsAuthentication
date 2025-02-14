﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.FormsAuthentication.Tests
{
    [TestClass]
    public class CookieCompatSHA512Framework45Tests
    {
        // web.config : <machineKey validation="HMACSHA512" decryption="AES" ...
        private const string SHA512ValidationKey = "58703273357638792F423F4528472B4B6250655368566D597133743677397A24432646294A404D635166546A576E5A7234753778214125442A472D4B61506452";
        private const string SHA512DecryptionKey = "66556A586E3272357538782F413F442A472D4B6150645367566B597033733676";

        [TestMethod]
        public void Can_Decrypt_Forms_Authentication_45_Ticket_WithSha512()
        {
            // Arrange
            var encryptor = new FormsAuthenticationTicketEncryptor(
                SHA512DecryptionKey, 
                SHA512ValidationKey,
                DecryptionKeyAlgorithm.Aes,
                ValidationKeyAlgorithm.Sha512, 
                CompatibilityMode.Framework45);

            // Act
            // this cookie has been generated by legacy FormsAuthentication
            var encryptedText = "4155EDCD81DB4687336A024F636B54ADB352E25E6D8D89E393C407A041DE0F8DFCA382DF1B1135B89AE0C580CCCFEBBB497C609ECA0B1BDDB5875E166A5C230A547FDBF7B4BDCA6A67A55E4AFA8F24B2399EAA55B4C31C00E36239E897B78FA234BF3DAFCCDB85CCA205A21569A7F4A23A7D0A2AD7780C3B55720574E72461675B30453CB214576453BF9D27DD6F2DA78BF74183728B5196D6772BA6031366CBC38A289B171251E7AEC8132B00F39E80D37E4331D97EDFE825840954C7D1FC274C68617C1D1A4B5973E4B977905E38EDE616EEC7AE22C0C2393BEDF95126063A";

            FormsAuthenticationTicket result = encryptor.DecryptCookie(encryptedText);

            Assert.IsNotNull(result);

            Assert.AreEqual("/", result.CookiePath);
            Assert.AreEqual(false, result.IsPersistent);
            Assert.AreEqual("test@example.com", result.Name);
            Assert.AreEqual("84e456a0-dbae-4ef9-9828-1f80def0d749", result.UserData);
            Assert.AreEqual(3, result.Version);
            Assert.AreEqual(result.IssueDate, new DateTime(636971592103633638, DateTimeKind.Utc).ToLocalTime());
            Assert.AreEqual(result.Expiration, new DateTime(636971628103633638, DateTimeKind.Utc).ToLocalTime());
        }

        [TestMethod]
        public void Can_Encrypt_And_Decrypt_Forms_Authentication_45_Ticket_WithSha512()
        {
            // Arrange
            var issueDateUtc = DateTime.UtcNow;
            var expiryDateUtc = issueDateUtc.AddHours(1);
            var formsAuthenticationTicket = new FormsAuthenticationTicket(5, "someuser@example.com", issueDateUtc.ToLocalTime(), expiryDateUtc.ToLocalTime(), true, "my data", "/path/");

            var encryptor = new FormsAuthenticationTicketEncryptor(
                SHA512DecryptionKey, 
                SHA512ValidationKey,
                DecryptionKeyAlgorithm.Aes,
                ValidationKeyAlgorithm.Sha512, 
                CompatibilityMode.Framework45);

            // Act
            // We encrypt the forms auth cookie.
            var encryptedText = encryptor.Encrypt(formsAuthenticationTicket);

            Assert.IsNotNull(encryptedText);

            // We decrypt the encypted text back into a forms auth ticket, and compare it to the original ticket to make sure it
            // roundtripped successfully.
            FormsAuthenticationTicket decryptedFormsAuthenticationTicket = encryptor.DecryptCookie(encryptedText);

            Assert.IsNotNull(decryptedFormsAuthenticationTicket);

            Assert.AreEqual(formsAuthenticationTicket.CookiePath, decryptedFormsAuthenticationTicket.CookiePath);
            Assert.AreEqual(formsAuthenticationTicket.IsPersistent, decryptedFormsAuthenticationTicket.IsPersistent);
            Assert.AreEqual(formsAuthenticationTicket.UserData, decryptedFormsAuthenticationTicket.UserData);
            Assert.AreEqual(formsAuthenticationTicket.Version, decryptedFormsAuthenticationTicket.Version);
            Assert.AreEqual(formsAuthenticationTicket.Expired, decryptedFormsAuthenticationTicket.Expired);
            Assert.AreEqual(formsAuthenticationTicket.IsValid(), decryptedFormsAuthenticationTicket.IsValid());
            Assert.AreEqual(false, decryptedFormsAuthenticationTicket.Expired);
            Assert.AreEqual(true, decryptedFormsAuthenticationTicket.IsValid());
            Assert.AreEqual(formsAuthenticationTicket.Expiration, decryptedFormsAuthenticationTicket.Expiration);
            Assert.AreEqual(formsAuthenticationTicket.IssueDate, decryptedFormsAuthenticationTicket.IssueDate);
        }
    }
}
