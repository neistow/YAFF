using System.Linq;
using Microsoft.AspNetCore.SignalR;

namespace YAFF.Api.Hubs.Common
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User!.Claims.Single(c => c.Type == "Id").Value;
        }
    }
}