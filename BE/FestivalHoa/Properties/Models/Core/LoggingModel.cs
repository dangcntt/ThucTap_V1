namespace FestivalHoa.Properties.Models.Core;

public class LoggingModel : Audit, TEntity<string>
{
    
    public string Content { get; set; }
    
    public string Action { get; set; }
    
    public string CaseName { get; set; }

    public LoggingModel(string content, string action , string casename)
    {
        this.Content = content;
        this.Action = action;
        this.CaseName = casename;
    }
}




