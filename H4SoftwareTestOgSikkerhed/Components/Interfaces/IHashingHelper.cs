namespace H4SoftwareTestOgSikkerhed.Components.Interfaces;

public interface IHashingHelper
{
    dynamic HashSHA2(dynamic input);
    dynamic HashHMAC(dynamic input);
    dynamic HashPBKDF2(dynamic input);
    string HashBcrypt(string input);
}
