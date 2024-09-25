// See https://aka.ms/new-console-template for more information
using Chirp.CLI;
using DocoptNet; 

// this is a comment used to test pull request git 

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
    await manager.saveCheep(arguments["<message>"].ToString());
    
    }

if (arguments["read"].IsTrue){
    // got the idea for moving the .Wait() method from here:  
    // https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/task-based-asynchronous-programming
    try
    {
        if (string.IsNullOrWhiteSpace(arguments["<limit>"].ToString()))
        {
            Task read = Task.Run(() => manager.readCheep());
            read.Wait();
        }
        else
        {
            manager.readCheep(Int32.Parse(arguments["<limit>"].ToString()));
        }
    }
    
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}


  