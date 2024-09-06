// See https://aka.ms/new-console-template for more information
using Chirp.CLI;
using DocoptNet; 
//using IDatabaseRepository;


CheepManager manager = new CheepManager();
//IdataRepository<Cheep> database = new CSVDatabase<Cheep>();


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
