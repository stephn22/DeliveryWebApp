using Microsoft.Extensions.Localization;
using System.Reflection;

namespace DeliveryWebApp.Infrastructure.Services
{
    public class IdentityLocalizationService
    {
        private readonly IStringLocalizer _localizer;
        private const string Assembly = "DeliveryWebApp.WebUI";

        public IdentityLocalizationService(IStringLocalizerFactory factory)
        {
            var type = typeof(IdentityResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName ?? Assembly);
            _localizer = factory.Create("IdentityResource", assemblyName.Name ?? Assembly);
        }

        public LocalizedString this[string key] => _localizer[key];

        public LocalizedString GetLocalizedHtmlString(string key)
        {
            return _localizer[key];
        }

        public LocalizedString GetLocalizedHtmlString(string key, string parameter)
        {
            return _localizer[key, parameter];
        }
    }
}
