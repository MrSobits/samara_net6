

using System;
using Bars.B4.DataAccess;
using Bars.B4.Modules.States;
using Bars.Gkh.Domain;

namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class BaseProsClaimService : IBaseProsClaimService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetInfo(BaseParams baseParams)
        {
            /*
             * метод нужен для получения списка домов управляющей организации для плановой инспекционной проверки
             * inspectionId - id проверки
             * условия выборки домов: жилой дом находится в разделе жилые дома управляющей организации и дата договора пересекается с датой плана проверки юр.лиц
             * inspectorNames - ФИО инспекторов проверки
             * inspectorIds - Id инспекторов проверки
            */

            try
            {
                var inspectorNames = "";
                var inspectorIds = "";

                if (baseParams.Params.ContainsKey("inspectionId"))
                {
                    var inspectionId = baseParams.Params["inspectionId"].ToLong();

                    var dataInspectors = 
                        Container.Resolve<IDomainService<InspectionGjiInspector>>()
                            .GetAll()
                            .Where(x => x.Inspection.Id == inspectionId)
                            .Select(x => new { x.Inspector.Id, x.Inspector.Fio })
                            .ToList();

                    foreach (var item in dataInspectors)
                    {
                        if (!string.IsNullOrEmpty(inspectorNames))
                        {
                            inspectorNames += ", ";
                        }

                        inspectorNames += item.Fio;

                        if (!string.IsNullOrEmpty(inspectorIds))
                        {
                            inspectorIds += ", ";
                        }

                        inspectorIds += item.Id.ToString();
                    }
                }

                return new BaseDataResult(new { inspectorNames, inspectorIds });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult {Success = false, Message = e.Message};
            }
        }

        public IDataResult ChangeInspState(BaseParams baseParams)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            var stateDomain = Container.ResolveDomain<State>();
            var baseProsClaimDomain = Container.ResolveDomain<BaseProsClaim>();

            try
            {
                var inspId = baseParams.Params.GetAsId("inspId");
                var isFinal = baseParams.Params.GetAs<bool>("isFinal");
                var isStart = baseParams.Params.GetAs<bool>("isStart");
                var stateName = baseParams.Params.GetAs<string>("stateName");

                var stateTypeId = stateProvider.GetStatefulEntityInfo(typeof (BaseProsClaim)).TypeId;
                var state = stateDomain.GetAll().Where(x => x.TypeId == stateTypeId)
                    .WhereIf(isFinal, x => x.FinalState)
                    .WhereIf(isStart, x => x.StartState)
                    .WhereIf(stateName.IsNotEmpty(), x => x.Name == stateName)
                    .FirstOrDefault();

                var baseProsClaim = baseProsClaimDomain.Load(inspId);
                baseProsClaim.State = state;
                baseProsClaim.IsResultSent = true;

                baseProsClaimDomain.Update(baseProsClaim);
                return new BaseDataResult(baseProsClaim);
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                Container.Release(stateProvider);
                Container.Release(stateDomain);
                Container.Release(baseProsClaimDomain);
            }

        }
    }
}