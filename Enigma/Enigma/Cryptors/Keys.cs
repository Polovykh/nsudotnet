namespace Enigma.Cryptors
{
	internal class Keys
	{
		internal Keys(byte[] key, byte[] initialVector)
		{
			Key = key;
			IV = initialVector;
		}

		internal byte[] Key { get; }
		// ReSharper disable once InconsistentNaming
		internal byte[] IV { get; }
	}
}
