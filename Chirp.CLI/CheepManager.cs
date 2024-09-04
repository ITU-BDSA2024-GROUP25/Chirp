namespace Chirp.CLI;

public class CheepManager
{
    private readonly string _path = "chirp_cli_db.csv";
    public string userName { get; set; }
    public DateTime time { get; set; }
    
    public CheepManager()
    { }

    public void SaveCheep(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            Console.WriteLine("Error: Empty cheep message");
            return;
        }
        
        userName = Environment.UserName;
        time = DateTime.Now;
        
        using (StreamWriter sw = new StreamWriter(_path, true))
        {
            sw.WriteLine($"{userName},{time},{message}");
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

                string output = $"{parts[0]} @ {parts[1]}: {parts[2]}";
                
                Console.WriteLine(output);
            }
        }
    }
}