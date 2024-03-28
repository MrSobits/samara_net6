namespace Bars.Gkh.RegOperator.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.RegOperator.DomainService.Import.Ches;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;

    public class ChesMatchAccountOwnerInterceptor<TOwner> : EmptyDomainInterceptor<TOwner> where TOwner : ChesMatchAccountOwner
    {
        public IChesComparingService ComparingService { get; set; }

        /// <inheritdoc />
        public override IDataResult AfterCreateAction(IDomainService<TOwner> service, TOwner entity)
        {
            this.ComparingService.ProcessOwnerMatchAdded(entity);
            return base.AfterUpdateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult AfterDeleteAction(IDomainService<TOwner> service, TOwner entity)
        {
            this.ComparingService.ProcessOwnerMatchRemoved(entity);
            return base.AfterUpdateAction(service, entity);
        }
    }
}