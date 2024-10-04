
public class ChirpDBContext : DbContext

{
    public ChirpDBContext(DbContextOptions<DbContext> options)
        : base(options)
    {
        public DBSet <T> ChirpDBSet { get; set; }
    }

}