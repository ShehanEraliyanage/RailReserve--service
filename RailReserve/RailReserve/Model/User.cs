using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace RailReserve.Model
{
    [CollectionName("users")]
    public class User : MongoIdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
    }
}
