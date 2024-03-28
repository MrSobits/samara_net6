namespace Bars.Gkh.Regions.Tyumen.DomainService
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Bars.Gkh.DomainService.AddressMatching;

    /// <summary>
    /// Сервис сопоставления адресов для импорта ручками из ТРИЦ
    /// </summary>
    public class TymenImportAddressMatchService : BaseImportAddressMatchService
    {
        /// <inheritdoc />
        protected override IDictionary<string, double> MatchGroups { get; } = new Dictionary<string, double>
        {
            { "place_type", .5d },
            { "place_name", 1d },
            { "street_type", .5d },
            { "street_name", 1d },
            { "house_type", .5d },
            { "house_num", 1d }
        };

        /// <inheritdoc />
        protected override Regex ImportAddressRegex { get; } =
            new Regex(
                @"^(?<place_type>\w+?\.?)\s+?(?<place_name>.+?),\s+?(?<street_type>.+?\.?)\s+?(?<street_name>.+?),\s+?(?<house_type>\w+?\.?)\s+?(?<house_num>\d+.?\d*?)$",
                RegexOptions.Compiled | RegexOptions.Singleline);

        /// <inheritdoc />
        protected override Regex LocalAddressRegex { get; } =
            new Regex(
                @"(?<place_type>[\w+/]+\.)\s(?<place_name>[^,]+),\s(?<street_type>[^.]+\.)\s(?<street_name>[^,]+),\s(?<house_type>\w+?\.?)\s(?<house_num>\d+.?\d*?)$",
                RegexOptions.Compiled | RegexOptions.Singleline);
    }
}