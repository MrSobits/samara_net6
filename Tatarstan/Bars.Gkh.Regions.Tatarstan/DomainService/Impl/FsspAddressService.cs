namespace Bars.Gkh.Regions.Tatarstan.DomainService.Impl
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;

    using Castle.Windsor;

    /// <summary>
    /// Сервис адресов ФССП
    /// </summary>
    public class FsspAddressService : IFsspAddressService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult AddressMatch(long fsspAddressId, long pgmuAddressId)
        {
            var fsspAddressDomain = this.Container.ResolveDomain<FsspAddress>();

            using (this.Container.Using(fsspAddressDomain))
            {
                var fsspAddress = fsspAddressDomain.Get(fsspAddressId);
                var pgmuAddress = new PgmuAddress() { Id = pgmuAddressId };

                fsspAddress.PgmuAddress = pgmuAddress;
                fsspAddressDomain.Update(fsspAddress);
            }
            
            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult AddressMismatch(long fsspAddressId)
        {
            var fsspAddressDomain = this.Container.ResolveDomain<FsspAddress>();

            using (this.Container.Using(fsspAddressDomain))
            {
                var fsspAddress = fsspAddressDomain.Get(fsspAddressId);
                
                fsspAddress.PgmuAddress = null;
                fsspAddressDomain.Update(fsspAddress);
            }

            return new BaseDataResult();
        }
    }
}