namespace Bars.GisIntegration.CapitalRepair.DataExtractors
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Tasks.PrepareData.CapitalRepair;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Селектор КПКР
    /// </summary>
    public class ProgramCrSelector : IDataSelector<ProgramCrProxy>
    {
        /// <summary>
        /// IoC-контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public List<ProgramCrProxy> GetExternalEntities(DynamicDictionary parameters)
        {
            var objectCrDomain = this.Container.ResolveDomain<ObjectCr>();

            try
            {
                var programs = objectCrDomain.GetAll()
                    .Where(x => x.ProgramCr != null && x.ProgramCr.Period != null)
                    .Where(x => x.RealityObject != null && x.RealityObject.Municipality != null)
                    .Select(
                        x =>
                            new
                            {
                                MoId = x.RealityObject.Municipality.Id,
                                x.ProgramCr.Period.DateStart,
                                x.ProgramCr.Period.DateEnd
                            })
                    .GroupBy(x => new {x.DateStart, x.DateEnd, x.MoId})
                    .Select(x => x.Key)
                    .AsEnumerable()
                    .Select(
                        x =>
                            new
                            {
                                StartMonthYear = x.DateStart.ToString("yyyy-MM"),
                                EndMonthYear = x.DateEnd.GetValueOrDefault().ToString("yyyy-MM"),
                                x.MoId
                            })
                    .GroupBy(x => new { x.StartMonthYear, x.EndMonthYear, x.MoId })
                    .Select(
                        x =>
                            new ProgramCrProxy
                            {
                                StartMonthYear = x.Key.StartMonthYear,
                                EndMonthYear = x.Key.EndMonthYear,
                                MunicipalityId = x.Key.MoId
                            })
                    .ToList();

                return programs;
            }
            finally
            {
                this.Container.Release(objectCrDomain);
            }
        }
    }
}
