namespace FestivalHoa.Properties.Helpers;

public static class DefaultCode
{
    public const int SUCCESS = 0; // Thành công 
    
    // TỪ 10 - 19 MÃ CODE THẤT BẠI CỦA THAO TÁC
    public const int CREATE_FAILURE = 10; // THÊM THẤT BẠI
    public const int UPDATE_FAILURE = 11; // CẬP NHẬT THẤP BẠI
    public const int DELETE_FAILURE = 12; // XÓA THẤT BẠI
    public const int USERNAME_NOT_FOUND = 13; // USERNAME KHÔNG TÌM THẤY 
    public const int WRONG_PASSWORD = 14; // PASSWORD SAI 
    public const int DOWLOAD = 15; // PASSWORD SAI 
        
    
    public const int  BEYOND_TIME= 1; // Vượt quá thời gian đăng nhập 
    
    
    
    
    
    public const int  REFRESH_TOKEN_OUT_TIME= 2 ; // Vượt quá thời gian đăng nhập 
    
    
    public const int  TOKEN_NOT_FOUND= 3 ; // Token hoac refresh token khong tim thay  

    public const int  TOKEN_OR_REFRESH_TOKEN_NOT_FOUND= 4 ; // Token hoac refresh token khong tim thay  
    
    
    // 20 - 29 MÃ CODE CỦA VỀ DỮ LIỆU
    public const int EXCEPTION = 20; //  lỗi exception 
    public const int DATA_EXISTED = 21; //   DỮ LIỆU ĐÃ TỒN TẠI TRONG HỆ THỐNG
    public const int DATA_NOT_FOUND = 22; //  DỮ LIỆU KHÔNG TỒN TẠI TRONG HỆ THỐNG
    public const int COMMON_NOT_FOUND = 23; // thông tin không đúng về ràng buộc dữ liệu hoặc danh mục!
    public const int ERROR_STRUCTURE = 24; // Cấu trúc gói tin gửi không đúng quy định
    public const int ACCOUNT_IS_LOCKED = 25; // Cấu trúc gói tin gửi không đúng quy định
    
    public const int ACCOUNT_NOT_AUTHORIZED = 26; // Cấu trúc gói tin gửi không đúng quy định
    
    
    
    public const int DATA_FIELDS_NOT_INCORRECT  = 27; // Một số trường data sai quy định 
    
    
    public const int ID_NOT_CORRECT_FORMAT  = 28; // Một số trường data sai quy định 
    
    
    public const int SORT_BY_NOT_EXIST = 29; // Một số trường data sai quy định 
    
    
 

    
    

}