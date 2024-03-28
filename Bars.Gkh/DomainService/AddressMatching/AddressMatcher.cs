namespace Bars.Gkh.DomainService.AddressMatching
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Сопоставлятель адресов
    /// </summary>
    public class AddressMatcher : IAddressMatcher
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult<IEnumerable<AddressMatch>> MatchAddresses(params AddressMatchDto[] addreses)
        {
            var addressMatchingServices = this.Container.Resolve<IImportAddressMatchService>();

            using (this.Container.Using(addressMatchingServices))
            {
                return addressMatchingServices.MatchAutomatically(addreses);
            }
        }
    }
}