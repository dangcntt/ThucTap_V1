using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FestivalHoa.Properties.Extensions;

public class TimeConvertExtenstion : DateTimeConverterBase
{
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.Value != null && !reader.Value.Equals("01/01/0001 12:00:00 AM"))
        {
            try
            {
                return DateTime.ParseExact(reader.Value.ToString(), "yyyy/MM/dd", CultureInfo.InvariantCulture);

            }
            catch (Exception e)
            {
                    
            }
                
            try
            {
                return DateTime.ParseExact(reader.Value.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            }
            catch (Exception e)
            {
                    
            }

            try
            {
                var date = reader.Value.ToString().Split(" ");

                return DateTime.ParseExact(date[0], "dd-MM-yyyy", CultureInfo.InvariantCulture);

            }
            catch (Exception e)
            {

            }
            try
            {
                var date = reader.Value.ToString().Split(" ");

                return DateTime.ParseExact(date[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);

            }
            catch (Exception e)
            {

            }
        }

        return null;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if(value != null)
        {
            var date = (DateTime)value;
            writer.WriteValue(date.ToLocalTime().ToString("yyyy-MM-dd") );
        }
         
    }
}