namespace Bars.Gkh.RegOperator.Distribution
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    /// <summary>
    /// Базовая реализация распределения
    /// </summary>
    public abstract class BaseDistribution : IDistribution
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Репозиторий периода начислений
        /// </summary>
        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        private string name;

        /// <summary>
        /// Наименование типа распределения
        /// </summary>
        public virtual string Name => this.name ?? (this.name = DistributionNameLocalizer.GetLocalizedName(this, this.DistributionCode.GetDisplayName()));

        /// <summary>
        /// Роут клиентского контроллера
        /// </summary>
        public abstract string Route { get; }

        /// <summary>
        /// Код распределения
        /// </summary>
        public abstract DistributionCode DistributionCode { get; }

        /// <summary>
        /// Маркер распределяемости объекта множественно
        /// </summary>
        public virtual bool DistributableAutomatically => false;

        /// <summary>
        /// Код типа распределения
        /// </summary>
        public virtual string Code => this.DistributionCode.ToString();

        /// <summary>
        /// Идентификатор права доступа
        /// </summary>
        public abstract string PermissionId { get; }

        /// <summary>
        /// Проверка применимости распределения к счету НВС
        /// </summary>
        /// <param name="distributable"></param>
        /// <returns></returns>
        public abstract bool CanApply(IDistributable distributable);

        /// <summary>
        /// Отменить распределение
        /// </summary>
        /// <param name="distributable">Счет НВС</param>
        /// <param name="operation">Отменяемая операция</param>
        public abstract void Undo(IDistributable distributable, MoneyOperation operation);

        /// <summary>
        /// Применить распределение
        /// </summary>
        /// <param name="args"></param>
        public abstract IDataResult Apply(IDistributionArgs args);

        /// <summary>
        /// Вытащить аргументы распределения
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public abstract IDistributionArgs ExtractArgsFrom(BaseParams baseParams);

        /// <summary>
        /// Вытащить аргументы из множества распределений
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public abstract IDistributionArgs ExtractArgsFromMany(BaseParams baseParams, int counter, decimal thisOneDistribSum);

        /// <summary>
        /// Получить объекты распределения
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public abstract IDataResult ListDistributionObjs(BaseParams baseParams);

        /// <inheritdoc />
        public virtual IDataResult GetOriginatorName(BaseParams baseParams)
        {
            return new BaseDataResult();
        }

        /// <summary>
        /// Создание операции
        /// </summary>
        /// <param name="distributable">Распределяемый объект</param>
        /// <returns>Операция движения денег</returns>
        protected MoneyOperation CreateMoneyOperation(IDistributable distributable)
        {
            var dictionary = DynamicDictionary.Create();
            dictionary["distributionCode"] = this.DistributionCode;

            return distributable.CreateOperation(dictionary, this.ChargePeriodRepository.GetCurrentPeriod());
        }
    }
}