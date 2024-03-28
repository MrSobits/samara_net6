namespace Bars.Gkh.Regions.Chelyabinsk.DomainService
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Bars.Gkh.DomainService.AddressMatching;

    /// <summary>
    /// Сервис сопоставления адресов ЧЭС
    /// </summary>
    public class ChesImportAddressMatchService : BaseImportAddressMatchService
    {
        /// <inheritdoc />
        protected override IDictionary<string, double> MatchGroups { get; } = new Dictionary<string, double>
        {
            { "place_type", .5d },
            { "place_name", 1d },
            { "street_type", .5d },
            { "street_name", 1d },
            { "house_num", 1d },
            { "liter", 1d }
        };

        /// <inheritdoc />
        protected override Regex ImportAddressRegex { get; } =
            new Regex(
                @"^(?<place_type>[\w+/]|\.)(?<place_name>[^,]+),(?<street_type>[^.]+\.)(?<street_name>[^,]+),(?<house_num>\d+)(?<liter>[\w/]{1,3})?,(?<liter>\w+)?$",
                RegexOptions.Compiled | RegexOptions.Singleline);

        /// <inheritdoc />
        protected override Regex LocalAddressRegex { get; } =
            new Regex(
                @"(?<place_type>[\w+/]+\.)\s(?<place_name>[^,]+),\s(?<street_type>[^.]+\.)\s(?<street_name>[^,]+),\sд\.\s(?<house_num>\d+)(/(?<liter>\w))?(?<liter>[\w/]{1,3})?(,\sлит\.\s(?<liter>\w))?(,\sкорп.\s(?<liter>\w))?$",
                RegexOptions.Compiled | RegexOptions.Singleline);
    }
}