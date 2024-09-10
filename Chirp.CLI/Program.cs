// See https://aka.ms/new-console-template for more information
using Chirp.CLI;
using DocoptNet; 


CheepManager manager = new CheepManager();
// code taken from slides : https://github.com/itu-bdsa/lecture_notes/blob/main/sessions/session_02/Slides.md
// code inpired by documentation link : https://docopt.github.io/docopt.net/dev/#api

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
// using Docopt to parse arguments and call cheep manager saveCheep() or readCheep() methodes 
var arguments = new Docopt().Apply(usage, args, version: "1.0", exit: true)!;
if (arguments["cheep"].IsTrue){
    manager.saveCheep(arguments["<message>"].ToString());
    }
if (arguments["read"].IsTrue){
    if (arguments["<limit>"] is not null)
    {
        manager.readCheep(Int32.Parse(arguments["<limit>"].ToString()));
    }
    else
    {
        manager.readCheep();
    }
}

else
{
    Console.WriteLine("Error: No arguments provided");
}
**/
  