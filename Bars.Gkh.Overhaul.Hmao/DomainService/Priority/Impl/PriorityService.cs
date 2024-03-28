namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.Overhaul;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;
    using Castle.Windsor;

    public partial class PriorityService : IPriorityService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<RealityObjectStructuralElement> RoStructElDomain { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgrammStage2> Stage2Service { get; set; }

        public IDataResult SetPriority(BaseParams baseParams)
        {
            var muId = baseParams.Params.GetAs<long>("muId");
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var startYear = config.ProgrammPeriodStart;
            var method = config.MethodOfCalculation;

            if (muId < 1)
            {
                return new BaseDataResult(false, "Не выбрано муниципальное образование");
            }

            var municipality = Container.Resolve<IDomainService<Municipality>>().Get(muId);

            var recs = baseParams.Params.GetAs<object[]>("records");

            var userCriterias = GetUserCriterias(recs);

            SaveCriterias(userCriterias);

            try
            {
                var listForUpdate = SetPriority(municipality, userCriterias, method, startYear); 

                InTransaction(session =>
                {
                    foreach (var item in listForUpdate)
                    {
                        session.Update(item);
                    }
                });

            }
            catch (ValidationException e)
            {
                return new BaseDataResult(false, e.Message);
            }

            return new BaseDataResult();
        }

        public IDataResult SetPriorityAll(BaseParams baseParams)
        {
            var gkhParams = Container.Resolve<IGkhParams>().GetParams();
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var startYear = config.ProgrammPeriodStart;
            var method = config.MethodOfCalculation;

            var moLevel = gkhParams.ContainsKey("MoLevel")
                ? gkhParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            var municipalities =
                Container.Resolve<IRepository<Municipality>>().GetAll()
                    .AsEnumerable()
                    .Where(x => x.Level.ToMoLevel(Container) == moLevel);

            var recs = baseParams.Params.GetAs<object[]>("priorityParams");

            var userCriterias = GetUserCriterias(recs);

            SaveCriterias(userCriterias);

            var listForUpdate = new List<RealityObjectStructuralElementInProgrammStage3>();

            try
            {
                foreach (var municipality in municipalities)
                {
                    listForUpdate.AddRange(SetPriority(municipality, userCriterias, method, startYear));
                }

                InTransaction(session =>
                {
                    foreach (var item in listForUpdate)
                    {
                        session.Update(item);
                    }
                });

            }
            catch (ValidationException e)
            {
                return new BaseDataResult(false, e.Message);
            }

            return new BaseDataResult();
        }
    }
}