namespace FestivalHoa.Properties.Models.Core;

public class ErrorModel
{
    public string Field { get; set; }
    
    public string Message { get; set; }
 
    public ErrorModel(string field , string message)
    {
        this.Field = field;
        this.Message = message;
    }
}