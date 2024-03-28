namespace Bars.Gkh.Overhaul.Reports
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;

    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;

    public class LackOfRequiredStructEls : BasePrintForm
    {
        public LackOfRequiredStructEls()
            : base(new ReportTemplateBinary(Properties.Resources.LackOfRequiredStructEls))
        {
        }

        #region параметры отчета
        private long[] municipalityIds;

        private bool typeManyApartments;
        private bool typeSocialBehavior;
        private bool typeIndividual;
        private bool typeBlockedBuilding;

        #endregion

        public IWindsorContainer Container { get; set; }
        
        public override string Name
        {
            get { return "Дома с отсутствующими обязательными КЭ"; }
        }

        public override string Desciption
        {
            get { return "Дома с отсутствующими обязательными КЭ"; }
        }

        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.LackOfRequiredStructEls";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.LackOfRequiredStructEls";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            typeManyApartments = baseParams.Params.GetAs("typeManyApartments", false);
            typeSocialBehavior = baseParams.Params.GetAs("typeSocialBehavior", false);
            typeBlockedBuilding = baseParams.Params.GetAs("typeBlockedBuilding", false);
            typeIndividual = baseParams.Params.GetAs("typeIndividual", false);
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
           

            var missingCeoDomain = Container.Resolve<IDomainService<RealityObjectMissingCeo>>();

            var realObjQuery = Container.Resolve<IRepository<RealityObject>>()
                                .GetAll()
                                .Where(x=> x.IsInvolvedCrTo2.HasValue && x.IsInvolvedCrTo2.Value)
                                .WhereIf(!typeManyApartments, x => x.TypeHouse != TypeHouse.ManyApartments)
                                .WhereIf(!typeSocialBehavior, x => x.TypeHouse != TypeHouse.SocialBehavior)
                                .WhereIf(!typeIndividual, x => x.TypeHouse != TypeHouse.Individual)
                                .WhereIf(!typeBlockedBuilding, x => x.TypeHouse != TypeHouse.BlockedBuilding)                               
                                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id)
                                            || municipalityIds.Contains(x.MoSettlement.Id))
                                .Where(x => x.ConditionHouse != ConditionHouse.Emergency && x.ConditionHouse != ConditionHouse.Razed);

            var neededStructElGroups =
                Container.Resolve<IDomainService<StructuralElementGroup>>()
                         .GetAll()
                         .Where(x => x.UseInCalc && x.Required && x.CommonEstateObject.IncludedInSubjectProgramm)
                         .Select(x => new
                                          {
                                              x.Id, 
                                              CeoId = x.CommonEstateObject.Id,
                                              GroupName = x.Name, 
                                              CeoName = x.CommonEstateObject.Name
                                          })
                          .OrderBy(x => x.CeoName)
                          .ThenBy(x => x.GroupName)
                          .ToArray();

            var existRoStructEls = Container.Resolve<IDomainService<RealityObjectStructuralElement>>()
                                .GetAll()
                                .Where(x => realObjQuery.Any(y => y.Id == x.RealityObject.Id))
                                .Where(x => x.StructuralElement.Group.UseInCalc && x.StructuralElement.Group.Required
                                    && x.StructuralElement.Group.CommonEstateObject.IncludedInSubjectProgramm && x.State.StartState)
                                .Select(x => new
                                                 {
                                                     x.RealityObject.Id,
                                                     SeGroupId = x.StructuralElement.Group.Id
                                                 })
                                .AsEnumerable()
                                .GroupBy(x => x.Id)
                                .ToDictionary(x => x.Key, y => y.Select(x => x.SeGroupId).Distinct().ToArray());

            var missingCeoByRo = missingCeoDomain.GetAll()
                                      .Where(x => realObjQuery.Any(y => y.Id == x.RealityObject.Id))
                                      .Select(x => new
                                      {
                                          x.RealityObject.Id,
                                          missngCeoId = x.MissingCommonEstateObject.Id
                                      })
                                     .AsEnumerable()
                                     .GroupBy(x => x.Id)
                                     .ToDictionary(x => x.Key, y => y.Select(x => x.missngCeoId).Distinct().ToArray());


            var realObjs = realObjQuery
                          .Select(x => new
                                           {
                                               x.Id, 
                                               MuName = x.Municipality.Name, 
                                               x.Address
                                           })
                          .OrderBy(x => x.MuName)
                          .ThenBy(x => x.Address)
                          .ToArray();

            var sectionGroup = reportParams.ComplexReportParams.ДобавитьСекцию("sectionGroup");

            var number = 1;
            foreach (var realObj in realObjs) 
            {
                var existgroups = existRoStructEls.ContainsKey(realObj.Id) ? existRoStructEls[realObj.Id] : new long [0];
                var missingCeos = missingCeoByRo.ContainsKey(realObj.Id) ? missingCeoByRo[realObj.Id] : new long[0];

                foreach (var group in neededStructElGroups.Where(x => !existgroups.Contains(x.Id) && !missingCeos.Contains(x.CeoId)))
                {
                    sectionGroup.ДобавитьСтроку();
                    sectionGroup["Number"] = number++;
                    sectionGroup["Municipality"] = realObj.MuName;
                    sectionGroup["Address"] = realObj.Address;
                    sectionGroup["Ceo"] = group.CeoName;
                    sectionGroup["Group"] = group.GroupName;
                }
            }
        }
    }
}

