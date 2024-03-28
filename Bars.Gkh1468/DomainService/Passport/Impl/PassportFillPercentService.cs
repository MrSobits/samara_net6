namespace Bars.Gkh1468.DomainService.Passport.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Enums;

    /// <summary>
    ///     Сервис подчета процента заполнения паспорта
    /// </summary>
    /// <typeparam name="TPassport"></typeparam>
    /// <typeparam name="TPassportRow"></typeparam>
    public class PassportFillPercentService<TPassport, TPassportRow> :
        IPassportFillPercentService<TPassport, TPassportRow>
        where TPassport : BaseProviderPassport where TPassportRow : BaseProviderPassportRow<TPassport>
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="passportDomain"></param>
        /// <param name="passportRowDomain"></param>
        /// <param name="metaDomain"></param>
        public PassportFillPercentService(
            IDomainService<TPassport> passportDomain,
            IDomainService<TPassportRow> passportRowDomain,
            IDomainService<MetaAttribute> metaDomain)
        {
            this.PassportDomain = passportDomain;
            this.PassportRowDomain = passportRowDomain;
            this.MetaDomain = metaDomain;
        }

        private Dictionary<long, MetaCounterProxy> MetaCounterDict { get; set; }

        private IDomainService<MetaAttribute> MetaDomain { get; set; }

        private IDomainService<TPassport> PassportDomain { get; set; }

        private IDomainService<TPassportRow> PassportRowDomain { get; set; }

        public decimal CountFillPercentage(long providerPassportId)
        {
            return CountFillPercentage(PassportDomain.Get(providerPassportId));
        }

        public decimal CountFillPercentage(TPassport providerPassport)
        {
            ArgumentChecker.NotNull(providerPassport, "providerPassport");

            // шаг 1: строим словарь по структуре по которой создан паспорт
            ProcessMeta(
                MetaDomain.GetAll()
                          .Where(x => x.ParentPart.Struct.Id == providerPassport.PassportStruct.Id)
                          .Select(
                              x =>
                              new MetaCounterProxy
                                  {
                                      MetaId = x.Id,
                                      ParentMetaId = x.Parent != null ? x.Parent.Id : 0,
                                      Type = x.Type,
                                      UseInCalculation = x.UseInPercentCalculation
                                  })
                          .ToArray());

            // шаг 2: накладываем сверху фактически заполненные значения
            ProcessValues(
                PassportRowDomain.GetAll()
                                 .Where(x => x.ProviderPassport.Id == providerPassport.Id)
                                 .Select(x => new ValueProxy { MetaId = x.MetaAttribute.Id, Value = x.Value })
                                 .ToArray());

            // шаг 3: складываем делим
            return Calculate();
        }

        private decimal Calculate()
        {
            // дергаем из словаря все значения, которые учитываются при подсчете процента
            var valuable = MetaCounterDict.Values.Where(x => x.UseInCalculation).ToArray();
            // суммируем счетчик ожиданий
            var expected = valuable.SafeSum(x => x.ExpectedValueCount);
            // и счетчик фактического количества заполненных значений
            var fact = valuable.SafeSum(x => x.FactValueCount);

            // ну и все
            if (expected > 0)
            {
                return 100 * fact / expected;
            }

            return 100;
        }

        /// <summary>
        ///     Строим по структуре паспорта словарь идентификатор -> мета-атрибут
        /// </summary>
        /// <param name="metas"></param>
        private void ProcessMeta(MetaCounterProxy[] metas)
        {
            foreach (var meta in metas)
            {
                // по-умолчанию ожидаем каждое значение один раз
                meta.ExpectedValueCount++;
                meta.Children = metas.Where(x => x.ParentMetaId == meta.MetaId).ToArray();
            }

            MetaCounterDict = metas.ToDictionary(x => x.MetaId, x => x);

            // но для групповых-множественных полей количество ожидаемых
            // дочерних полей зависит от количества созданных родителей
            // поэтому считаем что ни одного родителя может не быть и делаем ожидание = 0
            foreach (var meta in MetaCounterDict.Values)
            {
                if (meta.Type == MetaAttributeType.GroupedComplex)
                {
                    // Р - рекурсия
                    RecursiveApply(meta, m => m.ExpectedValueCount = 0);
                }
            }
        }

        private void ProcessValues(ValueProxy[] values)
        {
            foreach (var value in values)
            {
                var meta = MetaCounterDict[value.MetaId];

                if (meta.Type == MetaAttributeType.GroupedComplex)
                {
                    // встретив группового множественного родителя
                    // увеличиваем ожидание для него и всех его потомков на единицу
                    RecursiveApply(meta, m => m.ExpectedValueCount++);
                    // ну и самому родителю прибавляем счетчик фактических значений
                    // на самом деле смысла в этом нет, но так красивее
                    meta.FactValueCount++;
                }

                if (meta.Type == MetaAttributeType.Simple)
                {
                    // для простых полей проверяем его заполненность
                    // и если все ок, то увеличиваем счетчик фактических значений
                    if (!value.Value.Return(x => x.Trim()).IsEmpty())
                    {
                        meta.FactValueCount++;
                    }
                }
            }
        }

        private void RecursiveApply(MetaCounterProxy meta, Action<MetaCounterProxy> action)
        {
            action(meta);

            if (meta.Children == null || meta.Children.Length == 0)
            {
                return;
            }

            foreach (var child in meta.Children)
            {
                RecursiveApply(child, action);
            }
        }

        private class MetaCounterProxy
        {
            /// <summary>
            /// Дочерние элементы
            /// </summary>
            public MetaCounterProxy[] Children { get; set; }

            /// <summary>
            /// Счетчик ожидаемого количества значений
            /// </summary>
            public int ExpectedValueCount { get; set; }

            /// <summary>
            /// Счетчик фактического количества заполненных значений
            /// </summary>
            public int FactValueCount { get; set; }

            /// <summary>
            /// Идентификатор мета-атрибута
            /// </summary>
            public long MetaId { get; set; }

            /// <summary>
            /// Идентификатор родительского мета-атрибута
            /// </summary>
            public long ParentMetaId { get; set; }

            /// <summary>
            /// Тип мета-атрибута
            /// </summary>
            public MetaAttributeType Type { get; set; }

            /// <summary>
            /// Признак использования в расчете процента заполнения
            /// </summary>
            public bool UseInCalculation { get; set; }

            public override string ToString()
            {
                return
                    string.Format(
                        "MetaId: {0}, Type: {1}, ParentMetaId: {2}, ExpectedValueCount: {3}, FactValueCount: {4}, UseInCalculation: {5}, Children: {6}",
                        MetaId,
                        Type,
                        ParentMetaId,
                        ExpectedValueCount,
                        FactValueCount,
                        UseInCalculation,
                        Children.Length);
            }
        }

        private class ValueProxy
        {
            /// <summary>
            /// Идентификатор мета-атрибута
            /// </summary>
            public long MetaId { get; set; }

            /// <summary>
            /// Значение атрибута в паспорте
            /// </summary>
            public string Value { get; set; }

            public override string ToString()
            {
                return string.Format("MetaId: {0}, Value: {1}", MetaId, Value);
            }
        }
    }
}