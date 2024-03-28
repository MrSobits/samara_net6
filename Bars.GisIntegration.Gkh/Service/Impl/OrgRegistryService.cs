namespace Bars.GisIntegration.Gkh.Service.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Tasks.SendData.OrgRegistryCommon;
    using Bars.GisIntegration.UI.Service;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для получения объектов при выполнении импорта/экспорта данных через сервис OrgRegistry
    /// </summary>
    public class OrgRegistryService: IOrgRegistryService
    {
        /// <summary>
        /// Ioc контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить список контрагентов
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult GetContragentList(BaseParams baseParams)
        {
            var selector = this.Container.Resolve<IDataSelector<ContragentProxy>>("ContragentSelector");

            try
            {
                var loadParams = baseParams.GetLoadParam();
                baseParams.Params.Add("selectedList", "all");

                var contragents = selector.GetExternalEntities(baseParams.Params)
                    .Select(x => new
                    {
                        x.Id,
                        Name = x.Name.ToStr(),
                        Ogrn = x.Ogrn.ToStr(),
                        JuridicalAddress = x.JuridicalAddress.ToStr()
                    })
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                return new ListDataResult(contragents.Order(loadParams).Paging(loadParams).ToList(), contragents.Count());
            }
            finally
            {
                this.Container.Release(selector);
            }
        }
    }
}
