using System.Security.Cryptography;
using System.Text;
namespace H4SoftwareTestOgSikkerhed.Components.Services;

public class AsymetricEncryptionService
{
    private readonly string apiUrl = "https://localhost:5001/api/Encryption";
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

    public async Task<string> EncryptAsymetricAsync(string data)
    {
        string[] buffer = [data, PublicKey];
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(buffer);
        StringContent payload = new(json, Encoding.UTF8, "application/json");

        using HttpClient httpClient = new();
        HttpResponseMessage response = await httpClient.PostAsync(apiUrl, payload);
        return response.Content.ReadAsStringAsync().Result;
    }
}