namespace Domain.Models.GeneralSettings
{
    public class ApiRequestModel
    {
        public required string Url { get; set; }
        public required string TypeRequest { get; set; }
        public string? Body { get; set; }
        public double? TimeOut { get; set; }
        public IDictionary<string, object?>? Headers { get; set; }
        public AuthApiModel? Auth { get; set; }
    }

    public class AuthApiModel
    {
        public required string Type { get; set; }
        public required string Authorization { get; set; }
    }
}
