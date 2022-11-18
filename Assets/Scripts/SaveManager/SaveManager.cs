using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using Random = System.Random;

public class SaveManager<T>
{
    private string Savefolder => Path.Combine(Application.persistentDataPath, "SaveData");

    public void SaveData(T data,string fileName)
    {
       string jsondata= JsonConvert.SerializeObject(data);
       string encrypedData = Encrypt(jsondata);
       File.WriteAllText(Path.Combine(Savefolder,fileName),encrypedData);
    }

    public T LoadData(string fileName)
    {
        if (!File.Exists(Path.Combine(Savefolder, fileName)))
        {
            return default;
        }
        else
        {
            string encryptedjsondata = File.ReadAllText(Path.Combine(Savefolder, fileName));
            string decryptedjsondata = Decrypt(encryptedjsondata);
            return JsonConvert.DeserializeObject<T>(decryptedjsondata);
        }
    }

    private string Encrypt(string data)
    {
        byte[] clearBytes = Encoding.Unicode.GetBytes(data);
        using (Aes encryptor = Aes.Create())
        {
            Random rand=new Random();
            byte[] IV = new byte[15];
            rand.NextBytes(IV);
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes("EncryptionKey", IV);
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                data = Convert.ToBase64String(IV) + Convert.ToBase64String(ms.ToArray());
            }
        }
        return data;
    }

    private string Decrypt(string data)
    {
        byte[] IV = Convert.FromBase64String(data.Substring(0, 20));
        data = data.Substring(20).Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(data);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes("EncryptionKey", IV);
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                data = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return data;
    }

}
