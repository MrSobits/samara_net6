namespace Bars.GisIntegration.Gkh.DataExtractors.OrgRegistryCommon
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Tasks.SendData.OrgRegistryCommon;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Castle.Windsor;

    using ContragentState = Bars.Gkh.Enums.ContragentState;

    /// <summary>
    /// Экстрактор для контрагентов
    /// </summary>
    public class ContragentSelector : IDataSelector<ContragentProxy>
    {
        /// <summary>
        /// Ioc контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public List<ContragentProxy> GetExternalEntities(DynamicDictionary parameters)
        {
            long[] selectedIds;

            var selectedContracts = parameters.GetAs("selectedList", string.Empty);
            if (selectedContracts.ToUpper() == "ALL")
            {
                selectedIds = null; // выбраны все, фильтрацию не накладываем
            }
            else
            {
                selectedIds = selectedContracts.ToLongArray();
            }

            var contragentRepo = this.Container.ResolveRepository<Contragent>();

            try
            {
                return contragentRepo.GetAll()
                    .Where(x => x.ContragentState == ContragentState.Active)
                    .WhereIf(selectedIds != null, x => selectedIds.Contains(x.Id))
                    .DistinctBy(x => x.Id)
                    .Select(
                        x => new ContragentProxy
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Ogrn = x.Ogrn,
                            ContragentState = (Base.Tasks.SendData.OrgRegistryCommon.ContragentState)x.ContragentState,
                            FactAddress = x.FactAddress,
                            JuridicalAddress = x.JuridicalAddress,
                            OrganizationFormCode = x.OrganizationForm != null ? x.OrganizationForm.Code : string.Empty
                        })
                    .ToList();
            }
            finally
            {
                this.Container.Release(contragentRepo);
            }
        }
    }
}
