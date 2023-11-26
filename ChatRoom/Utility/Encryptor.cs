using System.Text;
using System.Security.Cryptography;

namespace Utility
{
    public static class AesEncryption
	{


		public static byte[] Encrypt(string plaintext, byte[] key, byte[] iv)
		{
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = key;
				aesAlg.IV = iv;
				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
				byte[] encryptedBytes;
				using (var msEncrypt = new System.IO.MemoryStream())
				{
					using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
						csEncrypt.Write(plainBytes, 0, plainBytes.Length);
					}
					encryptedBytes = msEncrypt.ToArray();
				}
				return encryptedBytes;
			}
		}
		public static byte[] Encrypt(byte[] plainBytes, byte[] key, byte[] iv)
		{
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = key;
				aesAlg.IV = iv;
				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
				byte[] encryptedBytes;
				using (var msEncrypt = new System.IO.MemoryStream())
				{
					using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{ 
						csEncrypt.Write(plainBytes, 0, plainBytes.Length);
					}
					encryptedBytes = msEncrypt.ToArray();
				}
				return encryptedBytes;
			}
		}
		public static string Decrypt(byte[] ciphertext, byte[] key, byte[] iv)
		{
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = key;
				aesAlg.IV = iv;
				ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
				byte[] decryptedBytes;
				using (var msDecrypt = new System.IO.MemoryStream(ciphertext))
				{
					using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						using (var msPlain = new System.IO.MemoryStream())
						{
							csDecrypt.CopyTo(msPlain);
							decryptedBytes = msPlain.ToArray();
						}
					}
				}
				return Encoding.UTF8.GetString(decryptedBytes);
			}
		}
		public static void Decrypt(byte[] ciphertext ,byte[] key, byte[] iv, out string dec )
		{
			 string resualt= string.Empty;
			try
			{
				using (Aes aesAlg = Aes.Create())
				{
					aesAlg.Key = key;
					aesAlg.IV = iv;
					ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
					byte[] decryptedBytes;
					using (var msDecrypt = new System.IO.MemoryStream(ciphertext))
					{
						using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
						{
							using (var msPlain = new System.IO.MemoryStream())
							{
								csDecrypt.CopyTo(msPlain);
								decryptedBytes = msPlain.ToArray();
							}
						}
					}
					resualt = Encoding.UTF8.GetString(decryptedBytes);
				}
			}
			catch  (Exception ex)
			{

				Console.WriteLine(ex);	 
			}
			
			dec = resualt;
		}
		public static byte[]? DecryptDoc(byte[] ciphertext, byte[] key, byte[] iv)
		{
			 
			try
			{
				byte[] decryptedBytes;
				using (Aes aesAlg = Aes.Create())
				{
					aesAlg.Key = key;
					aesAlg.IV = iv;
					ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
					
					using (var msDecrypt = new System.IO.MemoryStream(ciphertext))
					{
						using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
						{
							using (var msPlain = new System.IO.MemoryStream())
							{
								csDecrypt.CopyTo(msPlain);
								decryptedBytes = msPlain.ToArray();
							}
						}
					}
					return decryptedBytes;
				}
				
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex);

				return null;
			}

			
		}


		public static byte[] GenerateRandomAESKey(int len)
		{



			var rnd = new Random();
			var b = new byte[len];
			rnd.NextBytes(b);
			return b;
		}
	    public static KeyValuePair<byte[], byte[]> ConfigEncriptor()
		{
			byte[] key = GenerateRandomAESKey(32); //new byte[32]; // 256-bit key
			byte[] iv = GenerateRandomAESKey(16);  // new byte[16]; // 128-bit IV
			 
			 
	       return new KeyValuePair<byte[], byte[]>(key, iv);


		}



	}
}