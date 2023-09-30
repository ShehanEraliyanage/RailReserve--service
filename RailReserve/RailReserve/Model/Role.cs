using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace RailReserve.Model
{
    [CollectionName("roles")]
    public class Role : MongoIdentityRole<Guid>
    {

    }
}
