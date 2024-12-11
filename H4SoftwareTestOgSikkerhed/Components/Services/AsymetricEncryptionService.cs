using Org.BouncyCastle.Crypto;
using System.Security.Cryptography;
using System.Text;
namespace H4SoftwareTestOgSikkerhed.Components.Services;

public class AsymetricEncryptionService
{
    private readonly string apiUrl = "https://localhost:7020/api/AsymetricEncryption";
    private string PrivateKey { get; }
    public string PublicKey { get; }

    public AsymetricEncryptionService()
    {
        string privateKeyPath = "privateKey.txt";
        string publicKeyPath = "publicKey.txt";

        RSACryptoServiceProvider rsa = new();

        //if (!File.Exists(privateKeyPath) || !File.Exists(publicKeyPath))
        if (!File.Exists(privateKeyPath))
        {

                PrivateKey = rsa.ToXmlString(true);
                PublicKey = rsa.ToXmlString(false);

                SaveKey(privateKeyPath, PrivateKey);
                SaveKey(publicKeyPath, PublicKey);

        }
        else
        {
            PrivateKey = LoadKey(privateKeyPath);
            PublicKey = LoadKey(publicKeyPath);
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

    public string DecryptAsymetric(string valueToDecrypt)
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(PrivateKey);

            byte[] valueToEncryptAsBytes = Convert.FromBase64String(valueToDecrypt);
            byte[] encryptedValue = rsa.Decrypt(valueToEncryptAsBytes, false);
            return System.Text.Encoding.UTF8.GetString(encryptedValue);
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

    // store my keys
    public static void SaveKey(string filename, string content)
    {
        var bytes = Encoding.ASCII.GetBytes(content);
        using var fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
        fs.Write(bytes, 0, bytes.Length);
    }

    // load key
    public static string LoadKey(string filename)
    {
        var bytes = File.ReadAllBytes(filename);
        return Encoding.ASCII.GetString(bytes);
    }
}