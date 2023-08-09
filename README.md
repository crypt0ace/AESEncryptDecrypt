# AESEncryptDecrypt
Quick and dirty C# code to create AES encrypted strings and shellcode files

# Usage
Can be used to encrypt strings in C# code and shellcode files that can be integrated in C# files. This will output a randomly generated key and IV.
```bash
.\AESEncryptDecrypt.exe

Base64 encoded key: /6vfJK9tsq6Ri23uoBcr2EJoSdOhKKEFlhp84f7V8rc=
Base64 encoded IV: +FF6fGiq8GBxBsgNh0l9Zg==

[+] 1: String

[+] 2: Shellcode

```

The following decryption C# code can be used in the binary. Taken from the [SharpInjector](https://github.com/Tw1sm/SharpInjector/blob/master/SharpInjector/Program.cs) project.
```C#
public static byte[] Dec(string ciphertext)
        {
            byte[] key = Convert.FromBase64String("KEY");
            byte[] iv = Convert.FromBase64String("IV");
            byte[] buffer = Convert.FromBase64String(ciphertext);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    using (CryptoStream cs = new CryptoStream((Stream)ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream decryptedMs = new MemoryStream())
                        {
                            cs.CopyTo(decryptedMs);
                            return decryptedMs.ToArray();
                        }
                    }
                }
            }
        }
```

In case of a shellcode file, it can simply be used as this.
```C#
byte[] buf = Dec("AES Encrypted Shellcode File");
```

In case of strings, it can be used as this.
```C#
byte[] byteString = Dec("AES Encrypted String");
string decryptedString = Encoding.UTF8.GetString(aesswitchURL);
```
