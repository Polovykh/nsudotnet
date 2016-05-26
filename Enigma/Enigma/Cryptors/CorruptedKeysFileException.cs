using System;

namespace Enigma.Cryptors
{
	public class CorruptedKeysFileException : Exception
	{
		public CorruptedKeysFileException()
			: base("File with keys is corrupted")
		{
		}
	}
}
