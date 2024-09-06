// See https://aka.ms/new-console-template for more information
using Chirp.CLI;
using DocoptNet; 
//using IDatabaseRepository;


CheepManager manager = new CheepManager();
// code taken from slides : https://github.com/itu-bdsa/lecture_notes/blob/main/sessions/session_02/Slides.md
const string usage = @"Chirp CLI version. 

Usage:
  chirp read [<limit>]
  chirp cheep <message>
  chirp (-h | --help)
  chirp --version

Options:
  -h --help     Show this screen.
  --version     Show version.
";
// code inpired by documentation link : https://docopt.github.io/docopt.net/dev/#api
var arguments = new Docopt().Apply(usage, args, version: "1.0", exit: true)!;
if (arguments["cheep"].IsTrue)
   // manager.SaveCheep();
    Console.WriteLine("works? cheep");
    Console.WriteLine(arguments["<message>"]); // use <> when its values 
if (arguments["read"].IsTrue)
    // manager.SaveCheep(arguments.cheep.message);
    Console.WriteLine("works? READINGGG");
/*
return Docopt.CreateParser(usage)
    .WithVersion("1.0")
    .Parse(args)
    .Match(Run,
        result => Cheep(manager.SaveCheep(result.Message)),
            result => Read(manager.ReadCheep()));

if (args.Length > 0)
{
    switch (args[0].ToLower())
    {
        case "cheep":
            if (args.Length < 3)
            {
                manager.SaveCheep(args[1]);
            }
            else
            {
                Console.WriteLine("Error: Cheep only accepts one argument");
            }

            break;

        case "read":
            manager.ReadCheep();
            break;

        default:
            Console.WriteLine("Error: Unknown operation");
            break;
    }
}

else
{
    Console.WriteLine("Error: No arguments provided");
}
**/
  