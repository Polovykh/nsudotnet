using System;
using System.Globalization;
using Enigma.Cryptors;

namespace Enigma
{
    public class Program
    {
	    public static void Main(string[] args)
	    {
		    try
		    {
			    var parameters = ParseArguments(args);
			    using (var cryptor = CryptorsFactory.Create(parameters.AlgorithmName))
			    {
				    switch (parameters.Action)
				    {
					    case ActionType.Decrypt:
					    {
						    cryptor.Decrypt(parameters.InputFileName, parameters.KeysFileName, parameters.OutputFileName);
						    return;
					    }
					    case ActionType.Encrypt:
					    {
						    cryptor.Encrypt(parameters.InputFileName, parameters.OutputFileName);
						    return;
					    }
					    default:
					    {
						    throw new NotImplementedException("Required action is not supported");
					    }
				    }
			    }
		    }
		    catch (Exception exception)
		    {
			    Console.Out.WriteLine(exception.Message);
		    }
	    }

	    private static Parameters ParseArguments(string[] args)
	    {
		    var action = ParseAction(args[0]);

		    switch (args.Length)
		    {
			    case RequiredForEncrypt:
			    {
				    if (ActionType.Encrypt == action)
				    {
					    return new Parameters
					    {
						    Action = action,
						    AlgorithmName = args[2],
						    InputFileName = args[1],
						    OutputFileName = args[3]
					    };
				    }
				    throw new ArgumentException("Not enought arguments for decryption");
			    }
			    case RequiredForDecrypt:
			    {
				    if (ActionType.Decrypt == action)
				    {
					    return new Parameters
					    {
						    Action = action,
						    AlgorithmName = args[2],
						    InputFileName = args[1],
						    KeysFileName = args[3],
						    OutputFileName = args[4]
					    };
				    }
				    throw new ArgumentException("Too much arguments for encryption");
			    }
			    default:
			    {
				    throw new ArgumentException("Bad amount of arguments for anything");
			    }
		    }
	    }

	    private static ActionType ParseAction(string actionName)
	    {
		    var changedActionName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(actionName);

			ActionType result;
		    if (!Enum.TryParse(changedActionName, out result))
		    {
			    throw new ArgumentException("Unknown action");
		    }

		    return result;
	    }

		private const int RequiredForEncrypt = 4;
	    private const int RequiredForDecrypt = 5;
    }
}
