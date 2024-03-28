namespace Bars.Gkh.DomainService.MetaValueConstructor
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.MetaValueConstructor.DataFillers;
    using Bars.Gkh.MetaValueConstructor.DomainModel;
    using Bars.Gkh.MetaValueConstructor.Enums;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using StringExtensions = Bars.B4.Utils.StringExtensions;

    /// <summary>
    /// Сервис  по заполнению данными объектов-значений
    /// </summary>
    public class DataFillerService : IDataFillerService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Поставщик работы с источниками данных
        /// </summary>
        public IDataFillerProvider DataFillerProvider { get; set; }

        /// <inheritdoc />
        public void SetValue(BaseParams parameters, BaseDataValue value)
        {
            if (value.MetaInfo.DataValueType != DataValueType.Dictionary || value.MetaInfo.DataFillerName.IsEmpty())
            {
                return;
            }

            this.Container.UsingForResolved<IConstructorDataFiller>(value.MetaInfo.DataFillerName,
                (container, dataFiller) =>
                {
                    dataFiller.PrepareCache(parameters);
                    dataFiller.SetValue(value);
                });
        }

        /// <inheritdoc />
        public void SetValue(BaseParams parameters, IList<BaseDataValue> values)
        {
            // получаем тип конструктора и заодно проверяем, что все элементы относятся к одному типу конструктора
            var constructorType = values.Select(x => x.MetaInfo.Group.ConstructorType).Distinct().Single();

            var dataFillerImplDict = this.Container.ResolveAll<IConstructorDataFiller>().ToHashSet().ToDictionary(x => x.GetType());
            var dataFillerDict = this.DataFillerProvider.GetDataFillerInfo()
                .Where(x => !x.IsNamespace && x.ConstructorType == constructorType)
                .Select(x => new KeyValuePair<string, IConstructorDataFiller>(x.Code, dataFillerImplDict[x.EntityType]))
                .ToDictionary(x => x.Key, x => x.Value);

            using (this.Container.Using(dataFillerImplDict.Values))
            {
                foreach (var valueGroups in values
                    .Where(x => x.MetaInfo.DataValueType == DataValueType.Dictionary)
                    .Where(x => StringExtensions.IsNotEmpty(x.MetaInfo.DataFillerName))
                    .GroupBy(x => x.MetaInfo.DataFillerName))
                {
                    var dataFillerCode = valueGroups.Key;
                    var dataFillerImpl = dataFillerDict[dataFillerCode];

                    dataFillerImpl.PrepareCache(parameters);
                    foreach (var baseDataValue in valueGroups)
                    {
                        dataFillerImpl.SetValue(baseDataValue);
                    }
                }
            }
        }
    }
}