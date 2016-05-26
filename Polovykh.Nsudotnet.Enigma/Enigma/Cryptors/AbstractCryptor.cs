using System;
using System.IO;

namespace Enigma.Cryptors
{
	internal abstract class AbstractCryptor : ICryptor
	{
		public void Encrypt(string sourceFileName, string resultFileName)
		{
			OpenFilesAndCall(sourceFileName, resultFileName, (reader, writer) =>
			{
				var keys = Encrypt(reader, writer);
				SaveKeys(sourceFileName, keys);
			});
		}

		public void Decrypt(string cryptedFileName, string keysFileName, string resultFileName)
		{
			OpenFilesAndCall(cryptedFileName, resultFileName, (reader, writer) =>
			{
				var keys = LoadKeys(keysFileName);
				Decrypt(reader, writer, keys);
			});
		}

		public abstract void Dispose();

		#region Overloading members

		// Don't forget initial Keys property (sorry for this)
		protected abstract Keys Encrypt(FileStream reader, FileStream writer);

		protected abstract void Decrypt(FileStream reader, FileStream writer, Keys keys);

		#endregion

		#region Private members

		private static void SaveKeys(string sourceFileName, Keys keys)
		{
			var keysFileName = GenerateKeysFileName(sourceFileName);

			using (var writer = new StreamWriter(File.OpenWrite(keysFileName)))
			{
				var keyInBase64 = Convert.ToBase64String(keys.Key);
				var initialVectorInBase64 = Convert.ToBase64String(keys.IV);

				writer.Write("{0}\n{1}\n", keyInBase64, initialVectorInBase64);
			}
		}

		private static Keys LoadKeys(string keysFileName)
		{
			using (var reader = File.OpenText(keysFileName))
			{
				var keyInBase64 = reader.ReadLine();
				var initialVectorInBase64 = reader.ReadLine();

				if (string.IsNullOrEmpty(initialVectorInBase64) ||
				    string.IsNullOrEmpty(keyInBase64))
				{
					throw new CorruptedKeysFileException();
				}

				var key = Convert.FromBase64String(keyInBase64);
				var initialVector = Convert.FromBase64String(initialVectorInBase64);

				return new Keys(key, initialVector);
			}
		}

		private static string GenerateKeysFileName(string sourceFileName)
		{
			var extensionSeparatorIndex = sourceFileName.LastIndexOf('.');
			// ReSharper disable once ConvertIfStatementToReturnStatement
			if (-1 == extensionSeparatorIndex) // If source file has no extension => keysFileName = [sourceFileName].key
			{
				return string.Concat(sourceFileName, KeyFilePostfix);
			}

			return sourceFileName.Insert(extensionSeparatorIndex, KeyFilePostfix);
		}

		private static void OpenFilesAndCall(string inputFileName, string outputFileName,
			Action<FileStream, FileStream> action)
		{
			using (var reader = File.OpenRead(inputFileName))
			using (var writer = File.OpenWrite(outputFileName))
			{
				action.Invoke(reader, writer);
			}
		}

		private const string KeyFilePostfix = ".key";

		#endregion
	}
}