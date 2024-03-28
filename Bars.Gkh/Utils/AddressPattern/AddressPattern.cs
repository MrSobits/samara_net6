namespace Bars.Gkh.Utils.AddressPattern
{
    using System.Linq;

    using Bars.Gkh.ConfigSections.ClaimWork.Debtor;

    using Castle.Windsor;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.General;
    using Bars.Gkh.ConfigSections.General.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class AddressPattern : IAddressPattern
    {
        private ShortAddressFormat ShortAddressFormat
        {
            get
            {
                var config = Container.Resolve<IGkhConfigProvider>().Get<Bars.Gkh.ConfigSections.General.GeneralConfig>();
                return config.ShortAddressFormat;
            }
        }

        public IWindsorContainer Container { get; set; }
        public IGkhParams GkhParams { get; set; }

        public string FormatShortAddress(Municipality mo, FiasAddress address)
        {
            string result;

            switch (ShortAddressFormat)
            {
                case ShortAddressFormat.StartsFromLowestUrbanArea:
                    var placeIndex = address.AddressName.IndexOf(address.PlaceName);
                    result = address.AddressName.Substring(placeIndex < 0 ? 0 : placeIndex);
                    if (result.Contains("обл."))
                    {
                        var cityPlace = result.IndexOf("г.");
                        result = address.AddressName.Substring(cityPlace);
                    } 
                    break;
                default:
                    result = GetAddressForMunicipality(mo, address);
                    break;
            }

            return result;
        }

        private string GetAddressForMunicipality(Municipality mo, FiasAddress address)
        {
            if (address == null)
            {
                return string.Empty;
            }

            if (mo == null)
            {
                return string.Empty;
            }

            var result = address.AddressName ?? string.Empty;

            if (string.IsNullOrEmpty(result) && string.IsNullOrEmpty(mo.FiasId))
            {
                return string.Empty;
            }

            var repository = Container.Resolve<IFiasRepository>();
            var dinamicAddress = (DinamicAddress)repository.GetDinamicAddress(mo.FiasId);

            if (dinamicAddress == null)
            {
                return string.Empty;
            }

            var addressParts = dinamicAddress.AddressName.Split(",");

            if (result.StartsWith(addressParts.First()))
            {
                if (mo.Level == TypeMunicipality.UrbanArea && address.StreetGuidId == null)
                {
                    result = result.Replace(addressParts[0], string.Empty);
                }
                else
                    if (mo.Level == TypeMunicipality.UrbanArea && ShortAddressFormat == ShortAddressFormat.NoUrbanArea)
                    {
                        result = result.Replace(dinamicAddress.AddressName, string.Empty).Trim();
                    }
                    else 
                    {
                        var urbanAreaName = dinamicAddress.AddressName.Contains("г.")
                            ? dinamicAddress.AddressName.Split("г.")[0]
                            : dinamicAddress.AddressName;

                        result = result.Replace(urbanAreaName, string.Empty).Trim();
                    }              
            }

            if (result.Contains(addressParts[0]))
            {
                result = result.Replace(addressParts[0], "");
            }

            if (result.StartsWith(","))
            {
                result = result.Substring(1).Trim();
            }

            return result;
        }
    }
}
