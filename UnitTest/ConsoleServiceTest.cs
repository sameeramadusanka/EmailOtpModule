
using EmailOtpModule.Constants;
using EmailOtpModule.Services;
using FakeItEasy;
using Xunit;

public class ConsoleServiceTest
{
    private IOtpService _otpService;
    
    public ConsoleServiceTest()
    {
        _otpService = A.Fake<IOtpService>();
    }

    private ConsoleService CreateService(){
        return new ConsoleService(_otpService);
    }

    [Fact]
    public void Test_ConsoleService_Start_ValidateUserEmail_Negative_Invalid_Domain()
    {
        //Arrange
        string userEmail = "sameera@test.com";
        
        var input = new StringReader(userEmail);
        Console.SetIn(input);
        
        var output = new StringWriter();
        Console.SetOut(output);
        
        var service =  CreateService();

        //Act
        service.start();


        //Asssert
        Assert.Contains(EmailConstants.STATUS_EMAIL_INVALID, output.ToString());

    }

    [Fact]
    public void Test_ConsoleService_Start_ValidateUserEmail_Negative_Empty_Email()
    {
        //Arrange
        string userEmail = "";
        
        var input = new StringReader(userEmail);
        Console.SetIn(input);
        
        var output = new StringWriter();
        Console.SetOut(output);
        
        var service =  CreateService();

        //Act
        service.start();


        //Asssert
        Assert.Contains(EmailConstants.STATUS_EMAIL_INVALID, output.ToString());
    }

    [Fact]
    public void Test_ConsoleService_Start_ValidateUserEmail_Positive_Valid_Domain()
    {
        //Arrange
        string userEmail = "sameera@dso.org.sg";
        
        var input = new StringReader(userEmail);
        Console.SetIn(input);
        
        var output = new StringWriter();
        Console.SetOut(output);
        
        var service =  CreateService();

        //Act
        service.start();


        //Asssert
        Assert.Contains(EmailConstants.YOUR_OTP, output.ToString());
    }
    
    [Fact]
    public void Test_ConsoleService_Start_GenerateOtp_Positive()
    {
        //Arrange
        string userEmail = "sameera@dso.org.sg";
        
        var input = new StringReader(userEmail);
        Console.SetIn(input);
        
        var output = new StringWriter();
        Console.SetOut(output);
        
        Random random = new Random();
        int otp = random.Next(100000, 1000000); // Generates a number between 100000 and 999999
        string _generatedOtp = otp.ToString("D6");
        
        A.CallTo(() => _otpService.GenerateOtp()).Returns(_generatedOtp);
        var service =  CreateService();

        //Act
        service.start();

        //Asssert
        Assert.Contains(_generatedOtp, output.ToString());
    }

    [Fact]
    public void Test_ConsoleService_Start_SendEmail_Positive()
    {
        //Arrange
        string userEmail = "sameera@dso.org.sg";
        
        var input = new StringReader(userEmail);
        Console.SetIn(input);
        
        var output = new StringWriter();
        Console.SetOut(output);
        
        Random random = new Random();
        int otp = random.Next(100000, 1000000); // Generates a number between 100000 and 999999
        string _generatedOtp = otp.ToString("D6");
        
        A.CallTo(() => _otpService.GenerateOtp()).Returns(_generatedOtp);
        var service =  CreateService();

        //Act
        service.start();

        //Asssert
        Assert.Contains(EmailConstants.STATUS_EMAIL_OK, output.ToString());
    }
    
    [Fact]
    public void Test_ConsoleService_Start_CheckUserOtp_Negative_Retries()
    {
        //Arrange
        Random random = new Random();
        int otp = random.Next(100000, 1000000); // Generates a number between 100000 and 999999
        string _generatedOtp = otp.ToString("D6");

        string userEmail = "sameera@dso.org.sg\n000001\n000002\n000003\n000004\n000005\n000006\n000007\n000008\n000009\n000010";
        
        var emailInput = new StringReader(userEmail);
        Console.SetIn(emailInput);
        
        var output = new StringWriter();
        Console.SetOut(output);
        
        
        
        A.CallTo(() => _otpService.GenerateOtp()).Returns(_generatedOtp);

        var service =  CreateService();

        //Act
        service.start();

        //Asssert
        Assert.Contains(EmailConstants.STATUS_OTP_FAIL, output.ToString());
    }

    [Fact]
    public void Test_ConsoleService_Start_CheckUserOtp_Positive()
    {
        //Arrange
        Random random = new Random();
        int otp = random.Next(100000, 1000000); // Generates a number between 100000 and 999999
        string _generatedOtp = otp.ToString("D6");
        string userEmail = $"sameera@dso.org.sg\n{_generatedOtp}";
        
        var emailInput = new StringReader(userEmail);
        Console.SetIn(emailInput);
        
        var output = new StringWriter();
        Console.SetOut(output);
        
        
        A.CallTo(() => _otpService.GenerateOtp()).Returns(_generatedOtp);

        var service =  CreateService();

        //Act
        service.start();

        //Asssert
        Assert.Contains(EmailConstants.STATUS_OTP_OK, output.ToString());
    }
}