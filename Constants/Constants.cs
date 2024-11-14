namespace EmailOtpModule.Constants;

public static class EmailConstants{
    public static readonly string STATUS_EMAIL_OK = "email containing OTP has been sent successfully.";
    public static readonly string STATUS_EMAIL_FAIL = "email address does not exist or sending to the email has failed.";
    public static readonly string STATUS_EMAIL_INVALID = "email address is invalid.";
    public static readonly string STATUS_OTP_OK = "OTP is valid and checked";
    public static readonly string STATUS_OTP_FAIL = "OTP is wrong after 10 tries";
    public static readonly string STATUS_OTP_TIMEOUT = "timeout after 1 min";
    public static readonly string YOUR_OTP = "Your OTP Code is ";
    public static readonly string CODE_IS_VALID = ". The code is valid for 1 minute";
}