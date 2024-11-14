namespace EmailOtpModule.Services;

public class OtpService : IOtpService
{
    #region GenerateOtp
    public string GenerateOtp()
    {
        Random random = new Random();
        int otp = random.Next(100000, 1000000); // Generates a number between 100000 and 999999
        return otp.ToString("D6");
    }
    #endregion

}