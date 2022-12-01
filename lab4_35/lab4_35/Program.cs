using LabsLibrary;
using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.InteropServices;

[Command(Name = "sl-lab4", Description = "Lab4"),
Subcommand(typeof(Run), typeof(Version), typeof(SetPath))]
class Lab4
{
	private const string LAB_PATH = "LAB_PATH";
	public static void Main(string[] args) => CommandLineApplication.Execute<Lab4>(args);
	private int OnExecute(CommandLineApplication commandLineApp, IConsole activeConsole)
	{
		activeConsole.WriteLine("Commands");

		activeConsole.WriteLine($"Platform - " + Environment.OSVersion.Platform);
		commandLineApp.ShowHelp();
		return 1;
	}

	[Command("version", Description = "Version")]
	private class Version
	{
		private int OnExecute(IConsole console)
		{
			Console.WriteLine("Made by Serhii Lytvynenko IPZ-43");
			var actualVersion = Assembly.GetExecutingAssembly().GetName().Version;
			console.WriteLine($"Version: {actualVersion}");

			var labsPath = SetPath.GetLabPathEnv();

			if (string.IsNullOrWhiteSpace(labsPath))
			{
				console.WriteLine("Variable LAB_PATH: not set");
			}

			console.WriteLine($"Variable LAB_PATH: {(string.IsNullOrWhiteSpace(labsPath) ? "not set" : labsPath)}");
			return 1;
		}
	}

	[Command("run", Description = "Start lab"), Subcommand(typeof(Lab1Presenter)), Subcommand(typeof(Lab2Presenter)), Subcommand(typeof(Lab3Presenter))]
	private class Run
	{
		private int OnExecute(IConsole console)
		{
			console.Error.WriteLine("Choose lab");
			return 1;
		}

		private abstract class LabsLibrary
		{
			[Option("--input -i", Description = "Specify input file")]
			public string Input { get; } = null!;

			[Option("--output -o", Description = "Specify output file")]
			public string Output { get; } = null!;

			protected string GetInputPath()
			{
				if (File.Exists(Input))
				{
					return Input;
				}

				var labsPath = SetPath.GetLabPathEnv();

				if (string.IsNullOrWhiteSpace(labsPath))
				{
					labsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				}
				labsPath = Path.Combine(labsPath, "INPUT.TXT");

				if (File.Exists(labsPath))
				{
					return labsPath;
				}

				return string.Empty;
			}
			protected string GetOutputPath()
			{
				if (!string.IsNullOrWhiteSpace(Output))
				{
					return Output;
				}

				var labsPath = SetPath.GetLabPathEnv();

				if (string.IsNullOrWhiteSpace(labsPath))
				{
					labsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				}

				if (Directory.Exists(labsPath))
				{
					return Path.Combine(labsPath, "OUTPUT.TXT");
				}

				return "OUTPUT.TXT";
			}

			protected string ReadInputFile()
			{
				string inputPath = GetInputPath();

				if (string.IsNullOrWhiteSpace(inputPath))
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine($"Error: INPUT.TXT not found.");
					Console.ForegroundColor = ConsoleColor.White;
					return string.Empty;
				}

				Console.WriteLine($"Read input file: {inputPath}");

				return File.ReadAllText(inputPath);
			}
			protected void WriteOuputFile(string outputData)
			{
				string outputPath = GetOutputPath();

				Console.WriteLine($"Write output file: {outputPath}");

				File.WriteAllText(outputPath, outputData);
			}

			protected virtual int OnExecute(IConsole console)
			{
				console.WriteLine("Input: " + Input + "Output: " + Output);
				return 1;
			}
		}

		[Command("lab1", Description = "Start lab1")]
		private class Lab1Presenter : LabsLibrary
		{
			protected override int OnExecute(IConsole console)
			{
				string inputData = ReadInputFile();
				if (string.IsNullOrWhiteSpace(inputData))
				{
					return -1;
				}
				console.WriteLine($"Starting lab1");

				var result = Lab1.StartLab(Input);


				console.WriteLine($"Output: " + result);

				WriteOuputFile(result.ToString());
				return 1;
			}
		}

		[Command("lab2", Description = "Start lab2")]
		private class Lab2Presenter : LabsLibrary
		{
			protected override int OnExecute(IConsole console)
			{
				string inputData = ReadInputFile();
				if (string.IsNullOrWhiteSpace(inputData))
				{
					return -1;
				}

				console.WriteLine($"Starting lab2");

				var result = Lab2.StartLab(Input);
				console.WriteLine($"Output: " + result);

				WriteOuputFile(result.ToString());
				return 1;
			}
		}

		[Command("lab3", Description = "Start lab3")]
		private class Lab3Presenter : LabsLibrary
		{
			protected override int OnExecute(IConsole console)
			{
				string inputData = ReadInputFile();
				if (string.IsNullOrWhiteSpace(inputData))
				{
					return -1;
				}

				console.WriteLine($"Starting lab3");
				var result = Lab3.StartLab(Input);
				console.WriteLine($"Output: " + result);

				WriteOuputFile(result.ToString());
				return 1;
			}
		}
	}

	[Command("set-path", Description = "Set LAB_PATH")]
	private class SetPath
	{
		[Option("--path -p", Description = "Set path to file")]
		[Required(ErrorMessage = "Choose path to INPUT/OUTPUT file")]
		public string Path { get; } = null!;
		private int OnExecute(IConsole console)
		{
			Environment.SetEnvironmentVariable(LAB_PATH, Path);

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				File.WriteAllText(".env", $"{LAB_PATH}={Path}");
			}

			return 1;
		}
		public static string GetLabPathEnv()
		{
			var path = Environment.GetEnvironmentVariable(LAB_PATH);
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				var text = File.ReadAllText(".env");
				var keyValue = text.Split('=');
				if (keyValue.Length == 2)
					path = keyValue[1];
			}
			return path ?? "";
		}
	}
}