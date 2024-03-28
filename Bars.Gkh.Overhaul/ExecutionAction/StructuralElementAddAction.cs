namespace Bars.Gkh.Overhaul.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Entities;

    public class StructuralElementAddAction : BaseExecutionAction
    {
        private List<string> necessaryStructuralElementCodes;

        private List<RoStructuralElementProxy> necessaryStructuralElements;

        private List<RealityObjectStructuralElement> listToSave;

        private Dictionary<long, RoHaveRoofOrFacade> roofAndFacadeSeRoDict;

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<RealityObjectMissingCeo> RealityObjectMissingCeoDomain { get; set; }

        public IDomainService<RealityObjectStructuralElement> RealityObjectStructuralElementDomain { get; set; }

        public IDomainService<StructuralElement> StructuralElementDomain { get; set; }

        public override string Name => "Добавление недостающих КЭ в паспортах МКД (Челябинск)";

        public override string Description => "Только для Челябинска!\r\nДобавление недостающих КЭ в паспортах \r\nПо задаче 29482";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            this.listToSave = new List<RealityObjectStructuralElement>();

            var realtyObjectIds = this.RealityObjectDomain.GetAll()
                .Where(x => x.Municipality.Name == "Челябинский городской округ")
                .Select(x => x.Id).ToList();

            var realityObjectMissingCeo = this.RealityObjectMissingCeoDomain.GetAll()
                .Where(x => x.RealityObject.Municipality.Name == "Челябинский городской округ")
                .Select(x => new {roId = x.RealityObject.Id, ceoId = x.MissingCommonEstateObject.Id})
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.ceoId).ToList());

            var existingRealityObjectStructuralElementDict =
                this.RealityObjectStructuralElementDomain.GetAll()
                    .Where(x => x.RealityObject.Municipality.Name == "Челябинский городской округ")
                    .Select(x => new {RoId = x.RealityObject.Id, SeId = x.StructuralElement.Id})
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.SeId).ToList());

            this.roofAndFacadeSeRoDict = this.RealityObjectStructuralElementDomain.GetAll()
                .Where(x => x.RealityObject.Municipality.Name == "Челябинский городской округ")
                .Where(
                    x => x.StructuralElement.Group.CommonEstateObject.Name == "Фасад"
                        || x.StructuralElement.Group.CommonEstateObject.Name == "Крыша")
                .Select(
                    x => new
                    {
                        x.RealityObject.Id,
                        x.StructuralElement.Group.CommonEstateObject.Name
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x =>
                    {
                        var ooi = x.Select(y => y.Name).ToList();
                        return new RoHaveRoofOrFacade {HaveRoof = ooi.Contains("Крыша"), HaveFacade = ooi.Contains("Фасад")};
                    });

            this.necessaryStructuralElementCodes = new List<string>
            {
                "MKDOG1O06E01",
                "MKDOG1O06E04",
                "MKDOG1O04E03",
                "MKDOG1O04E02",
                "MKDOG1O04E01",
                "MKDOG1O05E03",
                "MKDOG1O05E08",
                "MKDOG1O05E02",
                "MKDOG1O05E01",
                "MKDOG1O02E02",
                "MKDOG1O02E01",
                "MKDOG1O14E08",
                "NMKDG07O34E02",
                "NMKDG07O34E01",
                "NMKDG07O34E03",
                "NMKDG07O34E04",
                "MKDOG2O01E03",
                "56",
                "57",
                "58",
                "NMKDG05O17E07",
                "MKDOG3O01T05"
            };

            this.necessaryStructuralElements =
                this.StructuralElementDomain.GetAll()
                    .Where(x => this.necessaryStructuralElementCodes.Contains(x.Code))
                    .Where(x => x.Group != null && x.Group.CommonEstateObject != null)
                    .Select(
                        x => new RoStructuralElementProxy
                        {
                            Id = x.Id,
                            CeoId = x.Group.CommonEstateObject.Id,
                            Name = x.Name,
                            Code = x.Code,
                            MultipleObject = x.Group.CommonEstateObject.MultipleObject
                        })
                    .ToList();

            foreach (var realtyObjectId in realtyObjectIds)
            {
                var ceoToExclude = realityObjectMissingCeo.ContainsKey(realtyObjectId) ? realityObjectMissingCeo[realtyObjectId] : new List<long>();

                if (existingRealityObjectStructuralElementDict.ContainsKey(realtyObjectId))
                {
                    this.CreateRealityObjectStructuralElements(realtyObjectId, existingRealityObjectStructuralElementDict[realtyObjectId], ceoToExclude);
                    continue;
                }

                this.CreateRealityObjectStructuralElements(realtyObjectId, new List<long>(), ceoToExclude);
            }

            var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();

            using (var tr = session.BeginTransaction())
            {
                try
                {
                    this.listToSave.ForEach(x => session.Insert(x));
                    tr.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        tr.Rollback();

                        return new BaseDataResult
                        {
                            Success = false,
                            Message = exc.Message
                        };
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, exc);
                    }
                }
            }

            return new BaseDataResult {Success = true};
        }

        private void CreateRealityObjectStructuralElements(long roId, List<long> existingStructuralElements, List<long> ceoToExclude)
        {
            var stateProvider = this.Container.Resolve<IStateProvider>();

            foreach (var necessarySe in this.necessaryStructuralElements)
            {
                // Исключаем ООИ из вкладки "Отсутствующие КЭ"
                if (ceoToExclude.Contains(necessarySe.CeoId))
                {
                    continue;
                }

                // если у дома есть какой-либо фасад
                if (necessarySe.Code == "MKDOG3O01T05" && this.roofAndFacadeSeRoDict.ContainsKey(roId) && this.roofAndFacadeSeRoDict[roId].HaveFacade)
                {
                    continue;
                }

                // если у дома есть какая-либо крыша
                if (necessarySe.Code == "NMKDG05O17E07" && this.roofAndFacadeSeRoDict.ContainsKey(roId) && this.roofAndFacadeSeRoDict[roId].HaveRoof)
                {
                    continue;
                }

                if (!existingStructuralElements.Contains(necessarySe.Id))
                {
                    var entity = new RealityObjectStructuralElement
                    {
                        RealityObject = new RealityObject {Id = roId},
                        StructuralElement = new StructuralElement {Id = necessarySe.Id},
                        Volume = 0,
                        Wearout = 0,
                        WearoutActual = 0,
                        LastOverhaulYear = 0,
                        Name = necessarySe.MultipleObject ? necessarySe.Name + " 1" : null,
                        ObjectCreateDate = DateTime.Now,
                        ObjectEditDate = DateTime.Now
                    };

                    stateProvider.SetDefaultState(entity);

                    this.listToSave.Add(entity);
                }
            }
        }
    }

    internal class RoStructuralElementProxy
    {
        public long Id { get; set; }

        public long CeoId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public bool MultipleObject { get; set; }
    }

    internal class RoHaveRoofOrFacade
    {
        public bool HaveRoof { get; set; }

        public bool HaveFacade { get; set; }
    }
}