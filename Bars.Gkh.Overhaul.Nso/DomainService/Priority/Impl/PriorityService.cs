namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System.Collections.Generic;
    using B4;

    using Bars.Gkh.Overhaul.Nso.ConfigSections;

    using Castle.Windsor;
    using Entities;
    using Gkh.Utils;
    using Overhaul.Entities;

    public partial class PriorityService : IPriorityService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<RealityObjectStructuralElement> RoStructElDomain { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgrammStage2> Stage2Service { get; set; }

        public IDataResult SetPriority(BaseParams baseParams)
        {
            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var startYear = config.ProgrammPeriodStart;
            var method = config.MethodOfCalculation;
            
            var recs = baseParams.Params.GetAs<object[]>("records");

            var userCriterias = GetUserCriterias(recs);

            SaveCriterias(userCriterias);

            try
            {
                var listForUpdate = SetPriority(userCriterias, method, startYear); 

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
            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var method = config.MethodOfCalculation;
            var startYear = config.ProgrammPeriodStart;

            var recs = baseParams.Params.GetAs<object[]>("priorityParams");

            var userCriterias = GetUserCriterias(recs);

            SaveCriterias(userCriterias);

            var listForUpdate = new List<RealityObjectStructuralElementInProgrammStage3>();

            try
            {
                
                listForUpdate.AddRange(SetPriority(userCriterias, method, startYear));

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