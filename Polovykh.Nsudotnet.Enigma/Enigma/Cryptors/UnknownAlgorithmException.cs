using System;

namespace Enigma.Cryptors
{
	public class UnknownAlgorithmException : ArgumentException
	{
		public UnknownAlgorithmException()
			: base("Required algorithm is not supported")
		{
		}
	}
}
