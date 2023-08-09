using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    public static byte[] GenerateRandomKey(int keySize)
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] key = new byte[keySize];
            rng.GetBytes(key);
            return key;
        }
    }

    public static byte[] GenerateRandomIV(int ivSize)
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] iv = new byte[ivSize];
            rng.GetBytes(iv);
            return iv;
        }
    }

    public static string EncryptToBase64(byte[] plaintext, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(plaintext, 0, plaintext.Length);
                }

                byte[] encryptedBytes = ms.ToArray();
                return Convert.ToBase64String(encryptedBytes);
            }
        }
    }

    public static byte[] Decrypt(byte[] ciphertext, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream(ciphertext))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (MemoryStream plaintextMs = new MemoryStream())
                    {
                        cs.CopyTo(plaintextMs);
                        return plaintextMs.ToArray();
                    }
                }
            }
        }
    }

    public static string EncryptStringToBase64(string plaintext, byte[] key, byte[] iv)
    {
        byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
        return EncryptToBase64(plaintextBytes, key, iv);
    }

    static void Main(string[] args)
    {
        // Generate a random key and IV
        int keySize = 256 / 8; // 256 bits key
        int ivSize = 128 / 8;  // 128 bits IV

        byte[] key = GenerateRandomKey(keySize);
        byte[] iv = GenerateRandomIV(ivSize);

        string keyBase64 = Convert.ToBase64String(key);
        string ivBase64 = Convert.ToBase64String(iv);

        Console.WriteLine($"\nBase64 encoded key: {keyBase64}");
        Console.WriteLine($"Base64 encoded IV: {ivBase64}\n");

        while (true)
        {
            Console.WriteLine("[+] 1: String\n");
            Console.WriteLine("[+] 2: Shellcode\n");
            string userInput = Console.ReadLine();

            if (userInput == "1")
            {
                Console.WriteLine("[*] String to Encrypt: ");
                string plaintext = Console.ReadLine();

                string encryptedStringBase64 = EncryptStringToBase64(plaintext, key, iv);

                Console.WriteLine("Encrypted String (Base64):");
                Console.WriteLine(encryptedStringBase64);
            }
            else if (userInput == "2")
            {
                Console.WriteLine("[*] Shellcode File Path: ");
                string inputFilePath = Console.ReadLine();
                byte[] shellcodeBytes = File.ReadAllBytes(inputFilePath);

                string encryptedShellcodeBase64 = EncryptToBase64(shellcodeBytes, key, iv);

                Console.WriteLine("Encrypted Shellcode (Base64):");
                Console.WriteLine(encryptedShellcodeBase64);
            }
        }

    }
}