namespace FestivalHoa.Properties.Interfaces.Core;

public interface IIdEntity<T> 
{
    T Id { get; set; }
    DateTime? CreatedAt { get; set; }
    DateTime? ModifiedAt { get; set; }
    string? CreatedBy { get; set; } 
    string? ModifiedBy { get; set; }
    
    string Name { get; set; }
    
    string Code { get; set; }
    bool IsDeleted { get; set; }
    int Sort { get; set; }

    string CollectionName { get; set; }
}