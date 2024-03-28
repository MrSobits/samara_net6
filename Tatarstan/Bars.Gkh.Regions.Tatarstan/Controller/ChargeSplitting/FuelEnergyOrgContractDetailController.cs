namespace Bars.Gkh.Regions.Tatarstan.Controller.ChargeSplitting
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tatarstan.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Контроллер <see cref="FuelEnergyOrgContractDetail" />
    /// </summary>
    public class FuelEnergyOrgContractDetailController : B4.Alt.DataController<FuelEnergyOrgContractDetail>
    {
        public IChargeSplittingService ChargeSplittingService { get; set; }

        /// <summary>
        /// Актуализировать сведения по договорам ТЭР
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult ActualizeFuelEnergyValues(BaseParams baseParams)
        {
            return this.ChargeSplittingService.ActualizeFuelEnergyValues(baseParams).ToJsonResult();
        }
    }
}