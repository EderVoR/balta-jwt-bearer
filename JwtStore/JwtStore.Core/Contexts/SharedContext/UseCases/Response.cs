using Flunt.Notifications;
using System.Reflection;

namespace JwtStore.Core.Contexts.SharedContext.UseCases
{
    public abstract class Response
    {
        public string Message { get; set; } = string.Empty;
        public int Status { get; set; } = 400;
        public bool IsSucess => Status >= 200 && Status <= 299;
        public IEnumerable<Notification>? Notifications { get; set; }
    }
}
