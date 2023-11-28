using MongoDB.Bson;

namespace ASP.NetCore_WebAPI_MongoDB_EntityFrameworkCore.Models
{
    public class Country
    {
        public ObjectId CountryId { get; set; }
        public required string CountryName { get; set; } = string.Empty;
        public required string CountryCode { get; set; } = string.Empty;
    }
}
