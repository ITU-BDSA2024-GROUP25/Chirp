// See https://aka.ms/new-console-template for more information
using Chirp.CLI;
using DocoptNet; 


CheepManager manager = new CheepManager();
// code taken from slides : https://github.com/itu-bdsa/lecture_notes/blob/main/sessions/session_02/Slides.md
// code inpired by documentation link : https://docopt.github.io/docopt.net/dev/#api
// Using this comment to test if our new workflow runs 

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
	// this sections needs to catch the exception that occurs
	// if the input "read five" turns up.
	// currently it does not know if someone feeds it a non null/int
    if (string.IsNullOrWhiteSpace(arguments["<limit>"].ToString() ))
    {
        manager.readCheep();
    }
    else
    {
        manager.readCheep(Int32.Parse(arguments["<limit>"].ToString()));
        
    }
}


  