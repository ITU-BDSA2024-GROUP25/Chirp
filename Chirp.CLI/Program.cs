// See https://aka.ms/new-console-template for more information
using System;
using System.IO;

string path = "chirp_cli_db.csv";

if (args[0] == "cheep")
{
    string userName = Environment.UserName;
    DateTimeOffset time = DateTimeOffset.Now;
    string message = args[1];
    
    using (StreamWriter sw = File.AppendText(path))
    {
        sw.WriteLine(userName + " @ " + time.ToUniversalTime() + ": " + message);
    }
}

else if (args[0] == "read")
{
    using (StreamReader sr = File.OpenText(path))
    {
        string s = "";
        while ((s = sr.ReadLine()) != null)
        {
            Console.WriteLine(s);
        }
    }
}

else
{
    Console.WriteLine(args[0] ?? "Unknown command");
}
