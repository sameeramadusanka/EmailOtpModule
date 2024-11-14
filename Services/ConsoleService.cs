using EmailOtpModule.Constants;
using System.Text.RegularExpressions;

namespace EmailOtpModule.Services;

public class ConsoleService : IConsoleService
{

    private readonly IOtpService _otpService;

    private string? _currentOtp;
    private DateTime _otpExpiryTime;
    private const int _maxRetries = 10;
    private const int _timeoutDurationSeconds = 60;

    public ConsoleService(IOtpService otpService)
    {
        _otpService = otpService;
    }

    #region start of console application
    public void start()
    {

        Console.WriteLine("###############################################################");

        //ask user to enter email
        Console.WriteLine("Please enter your email");
        string? userEmail = Console.ReadLine();

        //validate email
        bool isValidEmail = ValidateUserEmail(userEmail);
        if (!isValidEmail)
        {
            Console.WriteLine(EmailConstants.STATUS_EMAIL_INVALID);
            return;
        }

        //generate otp and email body
        _currentOtp = _otpService.GenerateOtp();
        _otpExpiryTime = DateTime.Now.AddSeconds(_timeoutDurationSeconds);

        string emailBody = $"{EmailConstants.YOUR_OTP}{_currentOtp}{EmailConstants.CODE_IS_VALID}";

        //send email
        bool isEmailSuccess = SendEmail(userEmail, emailBody);

        if (!isEmailSuccess)
        {
            Console.WriteLine(EmailConstants.STATUS_EMAIL_FAIL);
            return;
        }
        Console.WriteLine(EmailConstants.STATUS_EMAIL_OK);

        Console.WriteLine("###############################################################");

        //ask user to enter the otp and validate
        OtpStatus otpStatus = CheckUserOtp();

        if (otpStatus == OtpStatus.STATUS_OTP_OK)
        {
            Console.WriteLine(EmailConstants.STATUS_OTP_OK);
        }
        else if (otpStatus == OtpStatus.STATUS_OTP_TIMEOUT)
        {
            Console.WriteLine(EmailConstants.STATUS_OTP_TIMEOUT);
        }
        else
        {
            Console.WriteLine(EmailConstants.STATUS_OTP_FAIL);
        }
        Console.WriteLine("###############################################################");
    }
    #endregion

    #region CheckUserOtp
    private OtpStatus CheckUserOtp()
    {

        if (_currentOtp == null || DateTime.Now > _otpExpiryTime)
        {
            return OtpStatus.STATUS_OTP_TIMEOUT;
        }

        int attempts = 0;
        while (attempts < _maxRetries)
        {
            try
            {
                // Wait for OTP input with timeout
                Console.WriteLine("Please enter OTP");
                string userOtp = ReadUserInputWithTimeout();

                if (userOtp == _currentOtp)
                {
                    return OtpStatus.STATUS_OTP_OK;
                }
                else
                {
                    attempts++;
                }
            }
            catch (TimeoutException)
            {
                return OtpStatus.STATUS_OTP_TIMEOUT;
            }
        }
        return OtpStatus.STATUS_OTP_FAIL;
    }

    private string ReadUserInputWithTimeout()
    {
        string? userOtp = null;

        // Create a task to read input
        Task inputTask = Task.Run(() =>
        {
            userOtp = Console.ReadLine(); // Blocking call
        });

        // Wait for input or timeout
        if (Task.WaitAny(inputTask, Task.Delay(_timeoutDurationSeconds * 1000)) == 0)
        {
            // Input was provided before timeout
            return userOtp ?? string.Empty;
        }
        else
        {
            // Timeout occurred
            throw new TimeoutException();
        }
    }
    #endregion

    #region SendEmail
    private bool SendEmail(string email, string emailBody)
    {
        try
        {
            //Assume email is sent out
            Console.WriteLine(emailBody);
            return true;
        }
        catch (Exception e)
        {
            //Current implementstion there will be no exceptions
            return false;
        }

    }
    #endregion

    #region ValidateUserEmail
    private bool ValidateUserEmail(string? userEmail)
    {

        if (string.IsNullOrEmpty(userEmail))
        {
            return false;
        }
        userEmail = userEmail.Trim();
        
        //validate with regex
        string emailPattern = @"^[a-zA-Z0-9.]+@dso\.org\.sg$";

        if (!Regex.IsMatch(userEmail, emailPattern))
        {
            return false;
        }
        return true;
    }
    #endregion



}