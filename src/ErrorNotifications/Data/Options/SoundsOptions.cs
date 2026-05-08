namespace ErrorNotifications.Data.Options
{
    public class SoundsOptions
    {
        public const string JSONKey = "Sounds";

        public string[] OnSuccess { get; set; }
        public string[] OnError { get; set; }
    }
}
