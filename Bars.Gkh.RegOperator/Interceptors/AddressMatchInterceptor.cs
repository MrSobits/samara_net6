namespace Bars.Gkh.RegOperator.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainService.Import.Ches;

    public class AddressMatchInterceptor : EmptyDomainInterceptor<AddressMatch>
    {
        public IChesComparingService ComparingService { get; set; }

        /// <inheritdoc />
        public override IDataResult AfterCreateAction(IDomainService<AddressMatch> service, AddressMatch entity)
        {
            this.ComparingService.ProcessAddressMatchAdded(entity);

            return base.AfterCreateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult AfterDeleteAction(IDomainService<AddressMatch> service, AddressMatch entity)
        {
            this.ComparingService.ProcessAddressMatchRemoved(entity);

            return base.AfterDeleteAction(service, entity);
        }
    }
}