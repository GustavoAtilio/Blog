namespace Blog;


public static class Configuration 
{
    // Token - JWT - Json Web Token 
    public static string JwtKey  = "nPr7f3xCl9a06kt8s4CNV3skidTxYH2B";

    public static string ApiKeyName = "Api_key";
    public static string ApiKey = "curso_api_8du83748dsjdhy384y8hue";

    public static SmtpConfiguration Smtp = new();

    public class SmtpConfiguration
    {
        public string Host {get; set;}
        public int Port {get; set;} = 25;
        public string UserName {get; set;}
        public string Password {get; set;}
    };


}