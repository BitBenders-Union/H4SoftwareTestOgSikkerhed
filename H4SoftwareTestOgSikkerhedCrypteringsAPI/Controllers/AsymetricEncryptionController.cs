using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace H4SoftwareTestOgSikkerhedCrypteringsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsymetricEncryptionController : ControllerBase
    {
        [HttpPost]
        public string Post([FromBody] string[] value)
        {
            string text = value[0];
            string publicKey = value[1];

            using (RSACryptoServiceProvider rsa = new())
            {
                rsa.FromXmlString(publicKey);
                byte[] dataBytes = Encoding.UTF8.GetBytes(text);
                byte[] encryptedBytes = rsa.Encrypt(dataBytes, false); // true = windows xp support
                return Convert.ToBase64String(encryptedBytes);
            }

        }
    

    }
}
