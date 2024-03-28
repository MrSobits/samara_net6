namespace Bars.Gkh.Interceptors.Licensing
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities.Licensing;

    /// <summary>
    /// Интерцептор для <see cref="FormGovernmentService"/>
    /// </summary>
    public class FormGovernmentServiceInterceptor : EmptyDomainInterceptor<FormGovernmentService>
    {
        /// <summary>
        /// Домен-сервис <see cref="GovernmenServiceDetailGroup"/>
        /// </summary>
        public IDomainService<GovernmenServiceDetailGroup> GovernmenServiceDetailGroupDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="GovernmenServiceDetail"/>
        /// </summary>
        public IDomainService<GovernmenServiceDetail> GovernmenServiceDetailDomain { get; set; }

        /// <inheritdoc />
        public override IDataResult AfterCreateAction(IDomainService<FormGovernmentService> service, FormGovernmentService entity)
        {
            foreach (var source in this.GovernmenServiceDetailGroupDomain.GetAll().OrderBy(x => x.RowNumber))
            {
                this.GovernmenServiceDetailDomain.Save(new GovernmenServiceDetail
                {
                    DetailGroup = source,
                    FormGovernmentService = entity
                });
            }

            return base.AfterCreateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<FormGovernmentService> service, FormGovernmentService entity)
        {
            foreach (var governmenServiceDetail in this.GovernmenServiceDetailDomain.GetAll().Where(x => x.FormGovernmentService == entity).Select(x => x.Id))
            {
                this.GovernmenServiceDetailDomain.Delete(governmenServiceDetail);
            }

            return base.BeforeDeleteAction(service, entity);
        }
    }
}