namespace Bars.GkhOverhaulTp.Regions.Chelyabinsk.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Entities;

    using NHibernate;

    public class RobjectPropertiesUpdateAction : BaseExecutionAction
    {
        public IDomainService<RealityObject> RealityObjectService { get; set; }

        public IDomainService<RealityObjectStructuralElement> RealityObjectStructuralElementService { get; set; }

        public override string Name => "Действие к Задаче 18690";

        public override string Description
            =>
                @"1) В общих сведениях, в том случае если в поле 'Общая площадь жилых и нежилых помещений' стоит 0 то переносим в него данные из поля 'в т. ч жилых, всего'
2) Поля 'Год постройки' и 'Дата сдачи в эксплуатацию'. Если 'Год постройки' пусто, то переносим в него год из 'Дата сдачи в эксплуатацию' и наоборот, подставляя к году 01.01 (т.е. в формате дд.мм.гггг).
3) По лифтам. Если кол-во этажей 9 и более и указано кол-во подъездов, создадим в конструктивах по дому КЭ 'Лифтовая кабина' в кол-ве равному кол-ву подъездов."
            ;

        public override Func<IDataResult> Action => this.Execute;

        protected ISession OpenSession()
        {
            return this.Container.Resolve<ISessionProvider>().GetCurrentSession();
        }

        private BaseDataResult Execute()
        {
            var structuralElementId = this.Container.Resolve<IDomainService<StructuralElement>>()
                .GetAll()
                .Where(x => x.Code == "MKDOG1O08E01")
                .Select(x => (long?) x.Id)
                .FirstOrDefault();

            if (structuralElementId == null)
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = "Не найден КЭ 'Пассажирская лифтовая кабина' с кодом MKDOG1O08E01"
                };
            }

            var query = this.OpenSession().CreateSQLQuery(@"UPDATE gkh_reality_object 
SET area_liv_not_liv_mkd = area_living
WHERE ( (area_liv_not_liv_mkd is null) or (area_liv_not_liv_mkd = 0))
and area_living is not null
and area_living != 0");
            query.ExecuteUpdate();

            query = this.OpenSession().CreateSQLQuery(@"UPDATE gkh_reality_object 
SET build_year = extract( year from date_commissioning)
WHERE build_year is null
and date_commissioning is not null");
            query.ExecuteUpdate();

            query = this.OpenSession().CreateSQLQuery(@"UPDATE gkh_reality_object 
SET date_commissioning  = cast( build_year || '-01-01' as date)
WHERE date_commissioning  is null
and build_year is not null");
            query.ExecuteUpdate();

            var roLiftsDict = this.RealityObjectStructuralElementService
                .GetAll()
                .Where(x => x.RealityObject != null)
                .Where(x => x.StructuralElement.Id == structuralElementId)
                .GroupBy(x => x.RealityObject.Id)
                .Select(x => new {x.Key, count = x.Count()})
                .ToDictionary(x => x.Key, x => x.count);

            var realtyObjectToUpdateDict = this.RealityObjectService.GetAll()
                .Where(x => x.Municipality.Name == "Челябинский городской округ")
                .Select(
                    x => new
                    {
                        x.Id,
                        x.NumberEntrances,
                        x.MaximumFloors,
                        x.DateCommissioning
                    })
                .AsEnumerable()
                .Select(
                    x => new
                    {
                        x.Id,
                        createLifts = x.NumberEntrances.HasValue && x.NumberEntrances.Value > 0 && (x.MaximumFloors >= 9)
                            && !(roLiftsDict.ContainsKey(x.Id) && roLiftsDict[x.Id] >= x.NumberEntrances),
                        x.NumberEntrances,
                        x.DateCommissioning
                    })
                .Where(x => x.createLifts)
                .ToDictionary(x => x.Id);

            this.InTransaction(
                () =>
                {
                    foreach (var realtyObjectKvapair in realtyObjectToUpdateDict)
                    {
                        var realtyObjectData = realtyObjectKvapair.Value;

                        if (realtyObjectData.createLifts)
                        {
                            var firstIndex = roLiftsDict.ContainsKey(realtyObjectKvapair.Key) ? roLiftsDict[realtyObjectKvapair.Key] : 0;
                            var count = realtyObjectKvapair.Value.NumberEntrances.Value - firstIndex;

                            Enumerable.Range(firstIndex, count).ForEach(
                                index =>
                                {
                                    var realityObjectConstructElement = new RealityObjectStructuralElement
                                    {
                                        RealityObject = new RealityObject {Id = realtyObjectKvapair.Key},
                                        StructuralElement = new StructuralElement {Id = structuralElementId.Value},
                                        Name = string.Format("Пассажирская лифтовая кабина {0}", index + 1),
                                        Volume = 1
                                    };

                                    if (realtyObjectData.DateCommissioning.HasValue)
                                    {
                                        realityObjectConstructElement.LastOverhaulYear = realtyObjectData.DateCommissioning.Value.Year;
                                    }

                                    this.RealityObjectStructuralElementService.Save(realityObjectConstructElement);
                                });
                        }
                    }
                });

            return new BaseDataResult {Success = true};
        }

        private BaseDataResult InTransaction(Action action)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    action();

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();

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
    }
}