namespace FestivalHoa.Properties.Constants
{
    public class FormatTime
    {
        public const string FORMAT_DATE_CORE = "dd/MM/yyyy";
        public const string FORMAT_DATE = "dd/MM/yyyy HH:mm:ss";
        public const string FORMAT_SUBMIT_DATE = "yyyy/MM/dd HH:mm:ss";
        
        
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}