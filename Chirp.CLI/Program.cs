// See https://aka.ms/new-console-template for more information
using Chirp.CLI;

ChirpManager manager = new ChirpManager();

if (args.Length > 0)
{
    switch (args[0].ToLower())
    {
        case "chirp":
            if (args.Length > 1)
            {
                manager.SaveChirp(args[1]);
            }
            else
            {
                Console.WriteLine("Error: Chirp only accepts one argument");
            }

            break;

        case "read":
            manager.ReadChirp();
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
