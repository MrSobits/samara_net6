namespace Bars.Gkh.RegOperator.Distribution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4.Utils.Annotations;

    using Bars.Gkh.Utils;

    /// <summary>
    /// Утилиты распределения средств
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Распределение денег с раскидыванием копеек
        /// Распределение по формуле должно быть полным, т.е. если из 200 рублей на 3 объекта останется больше чем 3 копейки, то будет брошен эксепшн
        /// пример: 
        /// var proxies =
        ///     Utils.MoneyAndCentDistribution(accounts, //счета оплат жилого дома
        ///         x => x.RealityObject.AreaLiving ?? 0m / sumArea * suspenseAccount.Sum, //формула, по которой вычисляется сумма для одной записи
        ///         suspenseAccount.Sum, // сумма, с которой нужно сравнить округленную сумму
        ///         (acc, sum) => new Proxy(acc, sum), // создается прокси-объект, в котором есть все необходимые поля для отправки на клиент
        ///         (proxy, coin) => { proxy.Sum += coin; }); // добавление копейки
        /// более подробный пример в AbstractRealtyAccountDistribution
        /// </summary>
        /// <typeparam name="TItem">Тип основного объекта</typeparam>
        /// <typeparam name="TProxy">Тип прокси-объекта</typeparam>
        /// <param name="collection">Коллекция объектов</param>
        /// <param name="sumSelector">Селектор суммы для каждой записи, TItem - запись, int - индекс записи, decimal - возвращаемое значение</param>
        /// <param name="sumForEquals">Исходная сумма с которой проводится распределение</param>
        /// <param name="proxyCreator">Создатель прокси-объектов</param>
        /// <param name="tryAddCent">Добавление копейки к прокси-объекту</param>
        /// <param name="distributeCent">Раскидывать ли в конце копейку</param>
        /// <returns>Список прокси-объектов</returns>
        public static List<TProxy> MoneyAndCentDistribution<TItem, TProxy>(
            IEnumerable<TItem> collection,
            Func<TItem, int, decimal> sumSelector,
            decimal sumForEquals,
            Func<TItem, decimal, TProxy> proxyCreator,
            Func<TProxy, decimal, bool> tryAddCent,
            bool distributeCent = true)
            where TProxy : class
        {
            ArgumentChecker.NotNull(collection, nameof(collection));
            ArgumentChecker.NotNull(sumSelector, nameof(sumSelector));
            ArgumentChecker.NotNull(proxyCreator, nameof(proxyCreator));
            ArgumentChecker.NotNull(tryAddCent, nameof(tryAddCent));

            if (!collection.Any())
            {
                return new List<TProxy>();
            }

            // сумма с округлениями
            decimal roundedSum = 0m;

            var result = new List<TProxy>();

            int index = 0;
            foreach (var item in collection)
            {
                var pSum = sumSelector(item, index++).RegopRoundDecimal(2);
                var p = proxyCreator(item, pSum);
                roundedSum += pSum;
                result.Add(p);
            }

            return distributeCent ? Utils.CentDistribution(result, sumForEquals, roundedSum, tryAddCent) : result;
        }

        /// <summary>
        /// Распределение денег с раскидыванием копеек
        /// Распределение по формуле должно быть полным, т.е. если из 200 рублей на 3 объекта останется больше чем 3 копейки, то будет брошен эксепшн
        /// пример: 
        /// var proxies =
        ///     Utils.MoneyAndCentDistribution(accounts, //счета оплат жилого дома
        ///         x => x.RealityObject.AreaLiving ?? 0m / sumArea * suspenseAccount.Sum, //формула, по которой вычисляется сумма для одной записи
        ///         suspenseAccount.Sum, // сумма, с которой нужно сравнить округленную сумму
        ///         (acc, sum) => new Proxy(acc, sum), // создается прокси-объект, в котором есть все необходимые поля для отправки на клиент
        ///         (proxy, coin) => { proxy.Sum += coin; }); // добавление копейки
        /// более подробный пример в AbstractRealtyAccountDistribution
        /// </summary>
        /// <typeparam name="TItem">Тип основного объекта</typeparam>
        /// <typeparam name="TProxy">Тип прокси-объекта</typeparam>
        /// <param name="collection">Коллекция объектов</param>
        /// <param name="sumSelector">Селектор суммы для каждой записи, TItem - запись, int - индекс записи, decimal - возвращаемое значение</param>
        /// <param name="sumForEquals">Исходная сумма с которой проводится распределение</param>
        /// <param name="proxyCreator">Создатель прокси-объектов</param>
        /// <param name="tryAddCent">Добавление копейки к прокси-объекту</param>
        /// <param name="beforeCentModifier">Действие, выполняемое перед раскидыванием копеек</param>
        /// <param name="distributeCent">Раскидывать ли в конце копейку</param>
        /// <returns>Список прокси-объектов</returns>
        /// <returns>Список прокси-объектов</returns>
        public static List<TProxy> MoneyAndCentDistribution<TItem, TProxy>(
            IEnumerable<TItem> collection,
            Func<TItem, decimal> sumSelector,
            decimal sumForEquals,
            Func<TItem, decimal, TProxy> proxyCreator,
            Func<TProxy, decimal, bool> tryAddCent,
            Func<List<TProxy>, decimal, decimal, List<TProxy>> beforeCentModifier = null,
            bool distributeCent = true)
            where TProxy : class
        {
            ArgumentChecker.NotNull(collection, nameof(collection));
            ArgumentChecker.NotNull(sumSelector, nameof(sumSelector));
            ArgumentChecker.NotNull(proxyCreator, nameof(proxyCreator));
            ArgumentChecker.NotNull(tryAddCent, nameof(tryAddCent));

            if (!collection.Any())
            {
                return new List<TProxy>();
            }

            // сумма с округлениями
            decimal roundedSum = 0m;

            var result = new List<TProxy>();

            foreach (var item in collection)
            {
                var pSum = sumSelector(item).RegopRoundDecimal(2);
                var p = proxyCreator(item, pSum);
                roundedSum += pSum;
                result.Add(p);
            }

            if (beforeCentModifier != null)
            {
                result = beforeCentModifier(result, sumForEquals, roundedSum);
            }

            return distributeCent ? Utils.CentDistribution(result, sumForEquals, roundedSum, tryAddCent) : result;
        }

        /// <summary>
        /// Раскидывание копеек
        /// </summary>
        /// <typeparam name="TProxy">Тип прокси-объекта</typeparam>
        /// <param name="result">Список прокси-объектов</param>
        /// <param name="sumForEquals">Сумма для сравнения</param>
        /// <param name="roundedSum">Округленная сумма, полученная в результате распределения</param>
        /// <param name="tryAddCent">Добавление копейки к прокси объекту</param>
        /// <returns></returns>
        private static List<TProxy> CentDistribution<TProxy>(List<TProxy> result, decimal sumForEquals, decimal roundedSum, Func<TProxy, decimal, bool> tryAddCent)
        {
            // если у каждой записи распределяемая сумма = 0, 
            // то раскидывать нечего, просто отдаем результат для ручного редактирования
            if (roundedSum == 0)
            {
                return result;
            }

            // если сумма округлений получилась больше исходной суммы, то отбираем копейки
            // (такая ситуация может возникнуть в случае 200 рублей и 3 записей, после округления получаем 200,01 рублей)
            // иначе добавляем по копейке
            sumForEquals = Math.Round(sumForEquals, 2);
            var cent =
                sumForEquals < roundedSum
                    ? -0.01m
                    : 0.01m;

            int index = 0;

            while (sumForEquals - roundedSum != 0)
            {
                if (index + 1 > result.Count)
                {
                    throw new ArgumentException("Неверная формула вычисления суммы: распределение выполнено не полностью");
                }

                if(tryAddCent(result[index], cent))
                {
                    roundedSum += cent;
                }

                index++;
            }

            return result;
        }
    }
}
