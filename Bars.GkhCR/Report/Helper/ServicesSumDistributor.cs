namespace Bars.GkhCr.Report.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Enums;

    public sealed class SumDistributorTypeWorkProxy
    {
        public TypeWork TypeWork;

        public string WorkCode;

        public decimal Sum;
    }

    /// <summary>
    /// Хэлпер раскидывания сумм услуг по работам
    /// </summary>
    public static class ServicesSumDistributor
    {
        /// <summary>
        /// список приоритетов работ при раскидывании копейки
        /// </summary>
        private static List<string> priorityList = new List<string>
            {
                "13", // Ремонт крыши
                "16", // Ремонт фасада
                "17", // Утепление фасада
                "12", // Ремонт подвального помещения
                "2", // Ремонт внутридомовой инж. системы ГВС
                "4", // Ремонт внутридомовой инж. системы водоотведения
                "1", // Ремонт внутридомовой инж. системы теплоснабжения
                "3", // Ремонт внутридомовой инж. системы ХВС
                "6", // Ремонт внутридомовой инж. системы электроснабжения
                "5", // Ремонт внутридомовой инж. системы газоснабжения
                "14", // Ремонт/замена лифтового оборудования
                "15", // Ремонт лифтовой шахты
                "8", // Установка приборов учета: ГВС
                "9", // Установка приборов учета: ХВС
                "7", // Установка приборов учета: тепла
                "10", // Установка приборов учета: электроэнергии
                "11", // Установка приборов учета: газа
                "29", // Установка узлов регулирования
                "30", // Энергообследование
                "18", // Усиление фундаментов
                "21", // Ремонт подъездов 
                "19"  // Устройство систем противопожарной автоматики и дымоудаления
            };

        public static List<SumDistributorTypeWorkProxy> GetDistrubutedList(this List<SumDistributorTypeWorkProxy> typeWorkList, List<string> workCodes)
        {
            var serviceSum = typeWorkList.Where(y => y.TypeWork == TypeWork.Service).SafeSum(y => y.Sum);

            var works = typeWorkList
                .Where(y => y.TypeWork == TypeWork.Work)
                .Where(y => workCodes.Contains(y.WorkCode))
                .ToList();

            var worksSum = works.SafeSum(y => y.Sum);

            var distributedSum = 0m;

            var valuesByWork = works
                .Select(x =>
                    {
                        var proxy = new SumDistributorTypeWorkProxy
                            {
                                Sum = x.Sum,
                                TypeWork = x.TypeWork,
                                WorkCode = x.WorkCode
                            };

                        if (worksSum != 0)
                        {
                            var addToCost = Math.Floor((proxy.Sum * 100 * serviceSum) / worksSum) / 100;
                            distributedSum += addToCost;
                            proxy.Sum += addToCost;
                        }

                        return proxy;
                    })
                .ToList();

            // потерянные копейки при распределении
            var cents = serviceSum - distributedSum;
            if (cents > 0)
            {
                //в соответствии с приоритетами раскидываем копейки по работам
                foreach (var priorityKey in priorityList)
                {
                    var priorWork = valuesByWork.FirstOrDefault(y => y.WorkCode == priorityKey);

                    if (priorWork != null && priorWork.Sum > 0)
                    {
                        priorWork.Sum += cents;
                        break;
                    }
                }
            }

            return valuesByWork;
        }
    }
}