namespace Bars.Gkh.DomainService.MetaValueConstructor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService.Config;
    using Bars.Gkh.MetaValueConstructor;
    using Bars.Gkh.MetaValueConstructor.DomainModel;
    using Bars.Gkh.MetaValueConstructor.Enums;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Абстрактный сервис для работы с мета-значениями
    /// </summary>
    public abstract partial class AbstractDataValueService : IDataValueService
    {
        private HashSet<DataMetaInfo> metaInfoData;

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="DataMetaInfo"/>
        /// </summary>
        public IDomainService<DataMetaInfo> DataMetaInfoDomainService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="BaseDataValue"/>
        /// </summary>
        public IDomainService<BaseDataValue> BaseDataValueDomainService { get; set; }

        /// <inheritdoc />
        public abstract DataMetaObjectType ConstructorType { get; }

        /// <inheritdoc />
        public abstract IDataResult GetMetaValues(BaseParams baseParams);

        /// <inheritdoc />
        public abstract IDataResult CalcNow(BaseParams baseParams);

        /// <inheritdoc />
        public abstract IDataResult CalcMass(BaseParams baseParams);

        /// <summary>
        /// Расчёт основного дерева
        /// </summary>
        /// <typeparam name="TElement">Тип элемента</typeparam>
        /// <param name="root">Корень дерева</param>
        protected virtual decimal CalcInternal<TElement>(DataValueTreeNode<TElement> root) where TElement : class, IDataValue, IHasParent<TElement>
        {
            if (root.IsLeaf)
            {
                return this.GetValue(root);
            }

            var paramDict = new Dictionary<string, object>();

            foreach (var dataValueTreeNode in root.Children)
            {
                var metaInfo = dataValueTreeNode.Current.MetaInfo;
                paramDict.Add(metaInfo.Code.Translate(), this.CalcInternal(dataValueTreeNode) * (metaInfo.Weight ?? 1));
            }

            if (!root.IsRoot)
            {
                return this.GetValue(root, paramDict);
            }

            // Если вернулись до корня дерева, там формулы нет (так и мета-информации, значит просто суммируем все элементы верхнего уровня
            return paramDict.Values.Sum(x => x.ToDecimal());
        }

        private decimal GetValue<TElement>(DataValueTreeNode<TElement> element, Dictionary<string, object> paramDict)
            where TElement : class, IDataValue, IHasParent<TElement>
        {
            var currentElem = element.Current;

            var formula = this.GetFormula(currentElem.MetaInfo);
            if (currentElem.MetaInfo.Required && formula.IsEmpty())
            {
                throw new ValidationException(
                    $"Не заполнено обязательное поле конструктора: \"Формула расчета\" элемента: {this.GetPath(currentElem.MetaInfo)}");
            }

            if (!currentElem.MetaInfo.Required)
            {
                return 0;
            }

            formula = formula.Translate();
            var calc = new NCalc.Expression(formula) { Parameters = paramDict };

            try
            {
                currentElem.Value = calc.Evaluate().ToDecimal();
            }
            catch (DivideByZeroException)
            {
                currentElem.Value = 0;
            }

            // округляем, если необходимо
            if (currentElem.MetaInfo.Decimals.HasValue)
            {
                currentElem.Value = ((decimal)currentElem.Value).RoundDecimal(currentElem.MetaInfo.Decimals.Value);
            }

            return element.Value.ToDecimal();
        }

        private decimal GetValue<TElement>(DataValueTreeNode<TElement> element) where TElement : class, IDataValue, IHasParent<TElement>
        {
            var currentElem = element.Current;

            if (currentElem.MetaInfo.Required && currentElem.Value.IsNull())
            {
                throw new ValidationException($"Не заполнено обязательное поле: {this.GetPath(currentElem.MetaInfo)}");
            }

            if (currentElem.MetaInfo.Required)
            {
                return currentElem.Value.ToDecimal();
            }

            return 0;
        }

        /// <summary>
        /// Метод возвращает формулу текущего элемента.
        /// <para>Имеется возможность переопределения, для модификации формулы</para>
        /// </summary>
        /// <param name="metaInfo">Мета-писание</param>
        /// <returns>Формула</returns>
        protected virtual string GetFormula(DataMetaInfo metaInfo)
        {
            return metaInfo.Formula;
        }

        /// <summary>
        /// Метод возвращает полный путь до объекта
        /// </summary>
        /// <param name="metaInfo">Мета-описание</param>
        /// <returns>Путь</returns>
        protected virtual string GetPath(DataMetaInfo metaInfo)
        {
            var parentPath = metaInfo.Parent != null ? this.GetPath(metaInfo.Parent) : string.Empty;
            return parentPath.IsNotEmpty() ? parentPath + " -> " + metaInfo.Name : metaInfo.Name;
        }

        /// <summary>
        /// Вернуть мета-информацию для текущего конструктора
        /// </summary>
        /// <returns>Мета-информация для текущего конструктора</returns>
        protected HashSet<DataMetaInfo> GetMeta(MetaConstructorGroup group)
        {
            if (this.metaInfoData.IsNotEmpty())
            {
                return this.metaInfoData;
            }

            return this.metaInfoData = this.DataMetaInfoDomainService.GetAll()
                .Where(x => x.Group.Id == group.Id && x.Group.ConstructorType == this.ConstructorType)
                .ToHashSet();
        }

        /// <summary>
        /// Метод возвращает информацию о редакторе
        /// </summary>
        /// <param name="metaInfo">Мета-описание</param>
        /// <returns>База для мета-описания типа параметра конфигурации</returns>
        protected ITypeDescription GetEditor(DataMetaInfo metaInfo)
        {
            return ConstructorMetaHelper.DescribeType(metaInfo);
        }
    }
}