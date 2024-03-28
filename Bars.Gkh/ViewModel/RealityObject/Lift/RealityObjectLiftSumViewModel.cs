namespace Bars.Gkh.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Вью-модель для панели лифта
    /// </summary>
    public class RealityObjectLiftSumViewModel : BaseViewModel<RealityObjectLiftSum>
    {
        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domain">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public override IDataResult Get(IDomainService<RealityObjectLiftSum> domain, BaseParams baseParams)
        {
            var liftDomain = this.Container.Resolve<IDomainService<RealityObjectLift>>();

            try
            {
                var roId = baseParams.Params.GetAs("id", 0L);
                Dictionary<long, string> liftDict = new Dictionary<long, string>();

                var liftData = liftDomain.GetAll().Where(x => x.RealityObject.Id == roId)
                    .Select(
                        x => new RealityObjectLift
                        {
                            Id = x.Id,
                            Info = x.TypeLift != null ? x.TypeLift.Name : string.Empty
                        }).Distinct().ToList();
                foreach (RealityObjectLift lift in liftData)
                {
                    if (!liftDict.ContainsKey(lift.Id))
                    {
                        liftDict.Add(lift.Id, lift.Info);
                    }
                }

                var summaryData = domain.GetAll().FirstOrDefault(x => x.RealityObject.Id == roId);

                if (summaryData == null)
                {
                    summaryData = new RealityObjectLiftSum();
                }

                return new BaseDataResult(
                    new
                    {
                        MainCount = liftDict.Count(),
                        MainPassenger = liftDict.Count(x => x.Value.ToLower() == "пассажирский"),
                        MainCargo = liftDict.Count(x => x.Value.ToLower() == "грузовой"),
                        MainMixed = liftDict.Count(x => x.Value.ToLower() == "грузопассажирский"),
                        summaryData.Hinged,
                        summaryData.Lowerings,
                        summaryData.MjiCount,
                        summaryData.MjiPassenger,
                        summaryData.MjiCargo,
                        summaryData.MjiMixed,
                        summaryData.Risers,
                        summaryData.ShaftCount,
                        summaryData.BtiCount,
                        summaryData.BtiPassenger,
                        summaryData.BtiCargo,
                        summaryData.BtiMixed
                    });
            }
            finally
            {
                this.Container.Release(liftDomain);
            }
        }
    }
}