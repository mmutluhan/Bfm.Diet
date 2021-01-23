namespace Bfm.Diet.Core.Serilog
{
    public class BfmSerilogOptions
    {
        public AllEnricherPropertyNames EnricherPropertyNames { get; } = new AllEnricherPropertyNames();

        public class AllEnricherPropertyNames
        {
            public string UserId { get; set; } = nameof(UserId);
            public string Adi { get; set; } = nameof(Adi);
            public string EMail { get; set; } = nameof(EMail);
            public string SessionId { get; set; }  = nameof(SessionId);
            public string Token { get; set; } = nameof(Token);
            public string Audience { get; set; } = nameof(Audience);
            public string Issuer { get; set; } = nameof(Issuer);
            public string Expiration { get; set; } = nameof(Expiration);
            public string AccessTokenExpiration { get; set; } = nameof(AccessTokenExpiration);
            
        }
    }
}