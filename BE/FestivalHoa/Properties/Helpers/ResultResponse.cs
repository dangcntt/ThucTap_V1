using FestivalHoa.Properties.Models.Core;

namespace FestivalHoa.Properties.Helpers
{
    public class ResultMessageResponse
    {
        public int Code { get; set; } 
        public string Message { get; set; }
        
        public dynamic Data { get; set; }
        public List<ErrorModel> Detail { get; set; }

        public ResultMessageResponse WithCode(int resultCode)
        {
            if (!string.IsNullOrEmpty(resultCode.ToString()))
            {
                Code = resultCode;
            }

            return this;
        }
        public ResultMessageResponse WithMessage(string resultString)
        {
            if (!string.IsNullOrEmpty(resultString))
            {
                Message = resultString;
            }

            return this;
        }
        
        public ResultMessageResponse WithData(dynamic data)
        {
            if (data != null)
            {
                Data = data;
            }
            return this;
        }
        
        public ResultMessageResponse WithDetail(List<ErrorModel> listError)
        {
            if (listError.Count > 0 )
            {
                Detail = listError;
            }

            return this;
        }
        
    }

    
}