using H4SoftwareTestOgSikkerhed.Components.Interfaces;
using Microsoft.AspNetCore.DataProtection;

namespace H4SoftwareTestOgSikkerhed.Components.Services
{
    public class SymetricEncryptionService
    {
        private readonly IDataProtector _key;

        public SymetricEncryptionService(IDataProtectionProvider dataProtectionProvider)
        {
            _key = dataProtectionProvider.CreateProtector("æøå");
        }


        // symetric encryption

        public string Encrypt(string data) => _key.Protect(data);
        public string Decrypt(string data) => _key.Unprotect(data);


    }
}
