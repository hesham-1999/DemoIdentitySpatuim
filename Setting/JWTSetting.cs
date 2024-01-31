namespace DemoIdentity.Setting
{
    public class JWTSetting
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ValidTo { get; set; }
    }
}
