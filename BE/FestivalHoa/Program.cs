namespace FestivalHoa
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            //     Tinify.Key = DefaultKey.KEY_TINIFY;
            
            
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}