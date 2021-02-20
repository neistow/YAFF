namespace YAFF.Core.Settings
{
    public class EmailSettings
    {
        public string Host { get; set; }
        public bool EnableSsl { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}