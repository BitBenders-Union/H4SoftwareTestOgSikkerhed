using System.Security.Cryptography;
using System.Text;
namespace H4SoftwareTestOgSikkerhed.Components.Services;

public class AsymetricEncryptionService
{
    private string PrivateKey { get; }
    public string PublicKey { get; }
    public AsymetricEncryptionService()
    {
        using (RSACryptoServiceProvider rsa = new())
        {
            PrivateKey = rsa.ToXmlString(true);
            PublicKey = rsa.ToXmlString(false);
        }
    }

    public string Encrypt(string data)
    {
        using (RSACryptoServiceProvider rsa = new())
        {
            rsa.FromXmlString(PublicKey);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] encryptedBytes = rsa.Encrypt(dataBytes, false); // true = windows xp support
            return Convert.ToBase64String(encryptedBytes);
        }
    }

    public string Decrypt(string data)
    {
        using (RSACryptoServiceProvider rsa = new())
        {
            rsa.FromXmlString(PrivateKey);
            byte[] dataBytes = Convert.FromBase64String(data);
            byte[] decryptedBytes = rsa.Decrypt(dataBytes, false);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }

}
