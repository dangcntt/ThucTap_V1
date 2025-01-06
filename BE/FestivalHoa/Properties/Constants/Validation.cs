using System.Globalization;
using System.Text.RegularExpressions;
using FestivalHoa.Properties.Models.Core;

namespace FestivalHoa.Properties.Constants
{
    public static class Validation
    {
        
        
        internal static bool  IsCccd_Cmnd(string pText)
        {
            if (pText == null)
                return false;
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            if (!regex.IsMatch(pText) || pText.Length < 8 || pText.Length > 12 )
            {
                return false;
            }
            return true;
        }
        
        
        internal static bool  Is_Number(string pText, int length)
        {
            if (pText == null)
                return false;
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            if (!regex.IsMatch(pText) || pText.Length != length)
            {
                return false;
            }
            return true;
        }
        
        
        internal static bool IsDateValid(DateTime? date)
        {
            DateTime result;
            if (date == null) return false;
            if (date?.ToLocalTime().Year < 1850)
            {
                return false; 
            }
           return  DateTime.TryParseExact(date?.ToLocalTime().ToString(FormatTime.FORMAT_DATE), FormatTime.FORMAT_DATE, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);

        }
        
        
        
        internal static bool IsCommonVaild(CommonModelShort model)
        {
            if (model.Id != null && !model.Id.Equals("") && model.Name != null && !model.Name.Equals("") &&
                model.Code != null && !model.Code.Equals(""))
                return true;

            return false;

        }
    
        internal static bool IsListConmonValid(List<CommonModelShort> models)
        {
            foreach (var model in models)
            {
                if (model.Id == null || model.Id.Equals("") ||
                    model.Name == null || model.Name.Equals("") ||
                    model.Code == null || model.Code.Equals(""))
                {
                    return false;
                }
            }

            return true; 
        }




    }
}