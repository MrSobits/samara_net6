namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.Impl
{
    using System;
    using System.Collections;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.Modules.ClaimWork.Enums;

    using Castle.Windsor;
    using Entity;

    /// <summary>
    /// Сервис  Журнал судебной практики "Реестр неплательщиков"
    /// </summary>
    public class JurJournalDebtorService : IJurJournalDebtorService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Реестр неплательщиков
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="usePaging"></param>
        /// <param name="totalCount"></param>
        /// <returns>data.ToList()</returns>
        public IList GetList(BaseParams baseParams, bool usePaging, out int totalCount)
        {
            totalCount = 0;

            var viewDebtorDomain = this.Container.ResolveDomain<ViewDebtor>();
            var dateStart = baseParams.Params.GetAs<DateTime?>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime?>("dateEnd");
            var address = baseParams.Params.GetAs<string>("address");
            var typeDocument = baseParams.Params.GetAs<ClaimWorkDocumentType>("typeDocument");
            var loadParams = baseParams.GetLoadParam();

            try
            {
                var query = viewDebtorDomain.GetAll()
                    .Where(x => x.DocumentType == typeDocument)
                    .WhereIf(dateStart.HasValue, x => x.LawsuitDocDate.HasValue && x.LawsuitDocDate.Value.Date >= dateStart.Value)
                    .WhereIf(dateEnd.HasValue, x => x.LawsuitDocDate.HasValue && x.LawsuitDocDate.Value.Date <= dateEnd.Value)
                    .WhereIf(!address.IsEmpty(), x => x.Address.ToLower().Contains(address.ToLower()))
                    .Filter(loadParams, this.Container)
                    .Order(loadParams);

                if (usePaging)
                {
                    totalCount = query.Count();
                    query = query.Paging(loadParams);
                }

                return query.ToList();
            }
            finally
            {
                this.Container.Release(viewDebtorDomain);
            }

        }
    }
}