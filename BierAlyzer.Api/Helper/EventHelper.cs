using System.Linq;
using BierAlyzer.EntityModel;

namespace BierAlyzer.Api.Helper
{
    public static class EventHelper
    {
        public static string GenerateCode(BierAlyzerContext context)
        {
            while (true)
            {
                var code = AuthenticationHelper.GenerateSalt().Substring(0, 4).ToLower();
                if (!context.Event.Any(e => e.Code.ToLower() == code))
                    return code;
            }
        }
    }
}
