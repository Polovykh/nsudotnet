using System;

namespace Enigma.Cryptors
{
	public interface ICryptor : IDisposable
	{
		void Encrypt(string sourceFileName, string resultFileName);

		void Decrypt(string cryptedFileName, string keysFileName, string resultFileName);
	}
}
