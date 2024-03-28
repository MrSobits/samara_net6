namespace Sobits.RosReg.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Entities;

    public class ExtractEgrnInterceptor : EmptyDomainInterceptor<ExtractEgrn>
    {
        
        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<ExtractEgrn> service, ExtractEgrn entity)
        {
            if (entity.RoomId != null)
            {
                entity.IsMerged = Bars.Gkh.Enums.YesNoNotSet.Yes;
            }
            else
            {
                entity.IsMerged = Bars.Gkh.Enums.YesNoNotSet.No;
            }
            return this.Success();
        }

    }
}