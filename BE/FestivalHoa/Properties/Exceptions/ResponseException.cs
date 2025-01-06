using FestivalHoa.Properties.Helpers;
using FestivalHoa.Properties.Models.Core;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace FestivalHoa.Properties.Exceptions
{
    public class ResponseMessageException : Exception
    {
        public int ResultCode { get; set; }
        public string ResultString { get; set; }

        public List<ErrorModel> Error { get; set; } = new List<ErrorModel>();

        public ResponseMessageException()
        {
            
        }
        public ResponseMessageException WithException(int resultCode)
        {
            this.ResultCode = resultCode;
            switch (ResultCode)
            {
                case 10 :
                    this.ResultString = DefaultMessage.CREATE_FAILURE;
                    break;
                case 11 :
                    this.ResultString = DefaultMessage.UPDATE_FAILURE;
                    break;
                case 12 :
                    this.ResultString = DefaultMessage.DELETE_FAILURE;
                    break;
                case 13 :
                    this.ResultString = DefaultMessage.USERNAME_NOT_FOUND;
                    break;
                case 14 :
                    this.ResultString = DefaultMessage.WRONG_PASSWORD;
                    break;
                case 20 :
                    this.ResultString = DefaultMessage.EXCEPTION;
                    break;
                case 21 :
                    this.ResultString = DefaultMessage.DATA_EXISTED;
                    break;
                case 22 :
                    this.ResultString = DefaultMessage.DATA_NOT_FOUND;
                    break;
                case 23 :
                    this.ResultString = DefaultMessage.COMMON_NOT_FOUND;
                    break;
                case 24 :
                    this.ResultString = DefaultMessage.ERRER_STRUCTURE;
                    break;
                case 25 :
                    this.ResultString = DefaultMessage.ACCOUNT_IS_LOCKED;
                    break;
                case 26 :
                    this.ResultString = DefaultMessage.ACCOUNT_NOT_AUTHORIZED;
                    break;
                case 27 :
                    this.ResultString = DefaultMessage.DATA_FIELDS_NOT_INCORRECT;
                    break;
                case 28 :
                    this.ResultString = DefaultMessage.ID_NOT_CORRECT_FORMAT;
                    break;
                case 29 :
                    this.ResultString = DefaultMessage.SORT_BY_NOT_EXIST;
                    break;
                case 1 :
                    this.ResultString = DefaultMessage.BEYOND_TIME;
                    break;
                case 2 :
                    this.ResultString = DefaultMessage.REFRESH_TOKEN_OUT_TIME;
                    break;
                case 3 :
                    this.ResultString = DefaultMessage.TOKEN_NOT_FOUND;
                    break;
                case 4 :
                    this.ResultString = DefaultMessage.TOKEN_OR_REFRESH_TOKEN_NOT_FOUND;
                    break;
            }
            return this;
        }

        public ResponseMessageException WithCode(int code)
        {
            if (!string.IsNullOrEmpty(code.ToString()))
            {
                this.ResultCode = code;
            }
            return this;
        }
        
        public ResponseMessageException WithMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                this.ResultString = message;
            }
            return this;
        }
        public ResponseMessageException WithValidationResult(ValidationResult message)
        {
            if (message != null)
            {
                foreach (var item in message.Errors)
                {
                    this.ResultCode = DefaultCode.DATA_FIELDS_NOT_INCORRECT;
                    this.ResultString  = DefaultMessage.DATA_FIELDS_NOT_INCORRECT;
                    this.Error.Add(new ErrorModel(item.PropertyName , item.ErrorMessage));
                }
            }
            return this;
        }
        
        public ResponseMessageException WithDetail(List<ErrorModel> error)
        {
            if (error != null && error.Count > 0 )
            {
                this.Error = error;
            }
            return this;
        }
        
        
    }
   
}