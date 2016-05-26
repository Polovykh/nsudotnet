using System.IO;
using System.Security.Cryptography;

namespace Enigma.Cryptors
{
	internal class IntegratedCryptor : AbstractCryptor
	{
		internal IntegratedCryptor(SymmetricAlgorithm algorithm)
		{
			this.Algorithm = algorithm;
		}

		public override void Dispose()
		{
			Algorithm.Dispose();
		}

		protected override Keys Encrypt(FileStream reader, FileStream writer)
		{
			using (var encryptor = Algorithm.CreateEncryptor())
			using (var cryptoStream = new CryptoStream(reader, encryptor, CryptoStreamMode.Read))
			{
				cryptoStream.CopyTo(writer);

				return new Keys(Algorithm.Key, Algorithm.IV);
			}
		}

		protected override void Decrypt(FileStream reader, FileStream writer, Keys keys)
		{
			using (var decryptor = Algorithm.CreateDecryptor(keys.Key, keys.IV))
			using (var cryptoStream = new CryptoStream(reader, decryptor, CryptoStreamMode.Read))
			{
				cryptoStream.CopyTo(writer);
			}
		}

		private SymmetricAlgorithm Algorithm { get; }
	}
}
