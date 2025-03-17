namespace RepositoryTutorial.Domain
{
    public class Product : BaseEntity<Guid>
    {
        public string Name { get; set; }
    
        public int Priority { get; set; }
    }
}
