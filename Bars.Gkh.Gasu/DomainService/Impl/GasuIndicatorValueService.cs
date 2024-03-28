using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Gasu.Entities;
using Bars.Gkh.Gasu.Enums;
using Castle.Core.Internal;
using Castle.Windsor;

namespace Bars.Gkh.Gasu.DomainService
{
    using Bars.B4.DataAccess;

    public class GasuIndicatorValueService : IGasuIndicatorValueService
    {

        public IWindsorContainer Container { get; set; }

        private Dictionary<int, int> quarterDict = new Dictionary<int, int>()
        {
            {1, 1}, {2,1}, {3,1}, 
            {4,4}, {5,4}, {6,4},
            {7,7}, {8,7}, {9,7},
            {10,10}, {11,10}, {12,10}
        };

        public IDataResult GetListYears(BaseParams baseParams)
        {
            // получаем все года уже созданных записей + проверяем чтобы даже есл ничег онесоздано чтобы можно было выбрать втечении 5 лет относителньо текущего года

            var valuesDomain = Container.Resolve<IDomainService<GasuIndicatorValue>>();

            try
            {
                var result = new List<int>();

                var startYear = DateTime.Today.Year;
                var endYear = startYear + 5;

                while (startYear <= endYear)
                {
                    result.Add(startYear);
                    startYear++;
                }

                // Для того чтобы неупустить те года которые уже были созданы ранее к существующей пятилетки 
                // добавляем те года которые сохранены в системе 
                var curentYears = valuesDomain.GetAll()
                    .Select(x => x.Year)
                    .Distinct()
                    .AsEnumerable();
                
                foreach (var currYear in curentYears)
                {
                    if (!result.Contains(currYear))
                        {
                            result.Add(currYear);
                        }
                }

                return new BaseDataResult(result);
            }
            finally 
            {
                Container.Release(valuesDomain);
            }
        }

        // Метод создания записей  
        public IDataResult CreateRecords(BaseParams baseParams)
        {

            var year = baseParams.Params.GetAs("year", 0);

            var month = baseParams.Params.GetAs("month", 0);

            if (month <= 0 || month > 12)
            {
                return new BaseDataResult(false, "Необходимо указать месяц");
            }

            if (year <= 0)
            {
                return new BaseDataResult(false, "Необходимо указать год");
            }

            var indicatorDomain = Container.Resolve<IDomainService<GasuIndicator>>();
            var muDomain = Container.Resolve<IDomainService<Municipality>>();
            var indicatorValuesDomain = Container.Resolve<IDomainService<GasuIndicatorValue>>();
            var unProxy = Container.Resolve<IUnProxy>();
            var prov = Container.Resolve<ISessionProvider>();

            try
            {
                var ValuesToSave = new List<GasuIndicatorValue>();
                var ValuesToDelete = new List<GasuIndicatorValue>();

                var currValues = indicatorValuesDomain.GetAll()
                    .Where(x => x.Year == year && x.Month == month)
                    .Select(x => new
                    {
                        muId = x.Municipality.Id,
                        indId = x.GasuIndicator.Id,
                        rec = x
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.muId + "_" + x.indId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.rec).First());

                var indicatorList = indicatorDomain.GetAll()
                                    .WhereIf(month != 1 , x => x.Periodicity != Periodicity.Annually) // Если месяц не начало года, то исключаем показатели с признаком Периодичности = Годовой
                                    .WhereIf(quarterDict[month] != month, x => x.Periodicity != Periodicity.Quarterly) // Если месяц не начало квартала, то исключаем показатели с признаком Периодичности = Квартальный
                                    .ToList();

                var muList = muDomain.GetAll()
                    .Select(x => x.Id)
                    .ToList();
                
                foreach (var ind in indicatorList)
                {
                    foreach (var muId in muList)
                    {
                        var key = muId+"_"+ ind.Id;

                        GasuIndicatorValue rec = null;

                        if (currValues.ContainsKey(key))
                        {
                            rec = currValues[key];
                            currValues.Remove(key); // удаляем чтобы потом неудалить
                        }
                        else
                        {
                            rec = new GasuIndicatorValue();
                            rec.GasuIndicator = ind;
                            rec.Municipality = new Municipality {Id = muId};
                            rec.Year = year;
                            rec.Month = month;
                            rec.Value = 0;
                        }

                        var startPeriod = GetStartPeriod(year, month, ind.Periodicity);

                        if (rec.Id > 0)
                        {
                            // нужно проверить неизменилась ли дата начала 
                            if (startPeriod != rec.PeriodStart)
                            {
                                rec.PeriodStart = startPeriod;
                                ValuesToSave.Add(rec);
                            }
                        }
                        else
                        {
                            rec.PeriodStart = startPeriod;
                            ValuesToSave.Add(rec);
                        }
                    }
                }

                foreach (var kvp in currValues)
                {
                    ValuesToDelete.Add(kvp.Value);
                }

                if (ValuesToSave.Any())
                {
                    using (var stateLess = prov.OpenStatelessSession())
                    {

                        using (var tr = stateLess.BeginTransaction(IsolationLevel.Serializable))
                        {
                            try
                            {
                                ValuesToSave.ForEach(x =>
                                {
                                    
                                    if (x.Id == 0)
                                    {
                                        x.ObjectCreateDate = DateTime.Now;
                                        x.ObjectEditDate = DateTime.Now;
                                        stateLess.Insert(x);
                                    }
                                    else
                                    {
                                        x.ObjectEditDate = DateTime.Now;
                                        x.ObjectVersion += 1;

                                        stateLess.Update(unProxy.GetUnProxyObject(x));
                                    }
                                });

                                //удаляем те которые были но уже ненужны (Возможн отакое будет)
                                ValuesToDelete.ForEach(x => stateLess.Delete(unProxy.GetUnProxyObject(x)));

                                ValuesToSave.Clear();
                                ValuesToDelete.Clear();

                                tr.Commit();
                            }
                            catch(Exception exc) 
                            {
                                tr.Rollback();
                                return new BaseDataResult(false, exc.Message);
                                throw;
                            }
                        }

                    }     
                }
            }
            finally 
            {
                Container.Release(indicatorDomain);
                Container.Release(unProxy);
                Container.Release(indicatorValuesDomain);
                Container.Release(muDomain);
            }

            return new BaseDataResult();
        }

        private DateTime GetStartPeriod(int year, int month, Periodicity type)
        {
            var result = DateTime.Now;

            switch (type)
            {
                case Periodicity.Annually: result =  new DateTime(year - 1, month, 1); break; // Получаем начало предыдущего года

                case Periodicity.Monthly:
                {
                    var lastMonth = month - 1;
                    if (lastMonth <= 0)
                    {
                        month = 12;
                        year = year - 1;
                    }
                    else
                    {
                        month = lastMonth;
                    }

                    result = new DateTime(year, month, 1);
                }

                    break; // Получаем начало предыдущего месяца


                case Periodicity.Quarterly: // Поулчаем начало предыдущего квартала
                {
                    var current = quarterDict[month];

                    switch (current)
                    {
                        case 1: result = new DateTime(year -1, 10, 1); break; // Получаем октябрьский квартал
                        case 4: result = new DateTime(year, 1, 1); break; // Поулчаем январский квартал
                        case 7: result =  new DateTime(year, 4, 1); break; // Получаем апрельский квартал
                        case 10: result = new DateTime(year, 7, 1); break; // Получаем июльский квартал
                    }
                    
                }
                break;
            }

            return result;
        }
    }
}
