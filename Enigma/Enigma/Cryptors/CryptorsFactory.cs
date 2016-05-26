using System.Security.Cryptography;

namespace Enigma.Cryptors
{
	public static class CryptorsFactory
	{
		public static ICryptor Create(string algorithmName)
		{
			SymmetricAlgorithm algorithm;
			switch (algorithmName.ToLower())
			{
				case "aes":
				{
					algorithm = new AesCryptoServiceProvider();
					break;
				}
				case "des":
				{
					algorithm = new DESCryptoServiceProvider();
					break;
				}
				case "rc2":
				{
					algorithm = new RC2CryptoServiceProvider();
					break;
				}
				case "rijndael":
				{
					algorithm = new RijndaelManaged();
					break;
				}
				default:
				{
					throw new UnknownAlgorithmException();
				}
			}

			return new IntegratedCryptor(algorithm);
		}
	}
}