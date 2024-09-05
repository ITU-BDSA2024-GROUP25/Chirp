namespace Chirp.CLI;

public class CheepManager
{
    public record Cheep(string Author, string Message, long Timestamp);
    
    private readonly string _path = "chirp_cli_db.csv";
    
    public CheepManager()
    { }

    public void SaveCheep(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            Console.WriteLine("Error: Empty cheep message");
            return;
        }

        Cheep cheep = new Cheep(Environment.UserName, message, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        
        using (StreamWriter sw = new StreamWriter(_path, true))
        {
            sw.WriteLine($"{cheep.Author},{cheep.Timestamp},{cheep.Message}");
        }
    }

    public void ReadCheep()
    {
        using (StreamReader sr = File.OpenText(_path))
        {
            string line;
            
            // Takes header line out of table
            sr.ReadLine();
            
            while ((line = sr.ReadLine()) != null)
            {
                var parts = line.Split(',');
                
                DateTime timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(parts[1])).UtcDateTime;
                
                string output = $"{parts[0]} @ {timestamp}: {parts[2]}";
                
                Console.WriteLine(output);
            }
        }
    }
}