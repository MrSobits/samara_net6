namespace Bars.GisIntegration.UI.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Domain;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.Delegacy;

    using Castle.Windsor;

    /// <summary>
    /// Сервис делегирования
    /// </summary>
    public class DelegacyService : IDelegacyService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Добавить поставщиков информации
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult AddInformationProviders(BaseParams baseParams)
        {
            var contragentDomain = this.Container.ResolveDomain<RisContragent>();
            var delegacyDomain = this.Container.ResolveDomain<Delegacy>();

            try
            {
                var operatorIsId = baseParams.Params.GetAsId("operatorIs");
                var informationProviderIds = baseParams.Params.GetAs("informationProviderIds", new long[0]);
                var startDate = baseParams.Params.GetAs<DateTime>("startDate");
                var endDate = baseParams.Params.GetAs<DateTime>("endDate");

                if (operatorIsId == 0)
                {
                    return BaseDataResult.Error("Не указан оператор информационной системы");
                }

                var existInformationProviderIds =
                    delegacyDomain.GetAll()
                        .Where(x => x.OperatorIS.Id == operatorIsId)
                        .Select(x => x.InformationProvider.Id)
                        .ToList();

                var operatorIs = contragentDomain.Get(operatorIsId);

                var listToSave = new List<Delegacy>();

                foreach (var informationProviderId in informationProviderIds)
                {
                    if (!existInformationProviderIds.Contains(informationProviderId))
                    {
                        listToSave.Add(new Delegacy
                        {
                            OperatorIS = operatorIs,
                            InformationProvider = contragentDomain.Get(informationProviderId),
                            StartDate = startDate,
                            EndDate = endDate
                        });
                    }
                }

                TransactionHelper.InsertInManyTransactions(this.Container, listToSave, 10000, true, true);

                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(contragentDomain);
                this.Container.Release(delegacyDomain);
            }
        }
    }
}
