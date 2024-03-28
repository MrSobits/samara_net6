namespace Bars.GkhCr.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Modules.States;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.IoC;

    using Gkh.Entities;
    using Gkh.Entities.Dicts;
    using Newtonsoft.Json;

    /// <summary>
    /// Вид работы КР
    /// </summary>
    public class TypeWorkCr : BaseGkhEntity, IStatefulEntity
    {
        private IList<PerformedWorkAct> acts;
        private bool actsLoaded;

        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual ObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Разрез финансирования
        /// </summary>
        public virtual FinanceSource FinanceSource { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual Work Work { get; set; }

        /// <summary>
        /// Этап работы
        /// </summary>
        public virtual StageWorkCr StageWorkCr { get; set; }

        /// <summary>
        /// Наличие ПСД
        /// </summary>
        public virtual bool HasPsd { get; set; }

        /// <summary>
        /// Объем (плановый)
        /// </summary>
        public virtual decimal? Volume { get; set; }
        
        /// <summary>
        /// Ремонт по ЧС
        /// </summary>
        public virtual bool IsEmergrncy { get; set; }

        /// <summary>
        /// Потребность материалов
        /// </summary>
        public virtual decimal? SumMaterialsRequirement { get; set; }

        /// <summary>
        /// Сумма (плановая)
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Дата начала работ
        /// </summary>
        public virtual DateTime? DateStartWork { get; set; }

        /// <summary>
        /// Дата окончания работ
        /// </summary>
        public virtual DateTime? DateEndWork { get; set; }

        /// <summary>
        /// Объем выполнения
        /// </summary>
        public virtual decimal? VolumeOfCompletion { get; set; }

        /// <summary>
        /// Производитель
        /// </summary>
        public virtual string ManufacturerName { get; set; }

        /// <summary>
        /// Процент выполнения
        /// </summary>
        public virtual decimal? PercentOfCompletion { get; set; }
        
        /// <summary>
        /// ГИС ЖКХ Transport GUID
        /// </summary>
        public virtual string GisGkhTransportGuid { get; set; }
        
        /// <summary>
        /// ГИС ЖКХ GUID
        /// </summary>
        public virtual string GisGkhGuid { get; set; }

        /// <summary>
        /// Сумма расходов
        /// </summary>
        public virtual decimal? CostSum { get; set; }

        /// <summary>
        /// Чиленность рабочих(дробное так как м.б. смысл поля как пол ставки)
        /// </summary>
        public virtual decimal? CountWorker { get; set; }

        /// <summary>
        /// Доп. срок
        /// </summary>
        public virtual DateTime? AdditionalDate { get; set; }

        /// <summary>
        ///  Год ремонта
        /// </summary>
        public virtual int? YearRepair { get; set; }

        /// <summary>
        /// Признак является ли запись Активной
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Признак создана ли запись из ДПКР
        /// </summary>
        public virtual bool IsDpkrCreated { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        ///  Акты выполненных работ
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<PerformedWorkAct> Acts
        {
            get
            {
                if (!this.actsLoaded)
                {
                    this.LoadActs();
                }

                return this.acts;
            }
        }

        /// <summary>
        /// Наличие актов у типа работы
        /// </summary>
        /// <returns></returns>
        public virtual bool HasActs()
        {
            return this.Acts.Any();
        }

        /// <summary>
        /// Все акты по данной работе оплачены
        /// </summary>
        /// <returns></returns>
        public virtual bool IsTotallyPaid()
        {
            return this.Acts.All(x => x.IsPaid());
        }

        /// <summary>
        /// Сумма по актам
        /// </summary>
        /// <returns></returns>
        public virtual decimal GetSumByActs()
        {
            return this.Acts.Sum(x => x.Sum ?? 0);
        }

        public virtual decimal GetPaidSumByActs()
        {
            return this.Acts
                .Where(x => x.State.FinalState)
                .Sum(x => x.Payments.Sum(y => y.Paid));
        }

        /// <summary>
        /// Покрывается ли сумма работы суммой из актов
        /// </summary>
        /// <returns></returns>
        public virtual bool IsCoveredByActs()
        {
            return this.Sum.GetValueOrDefault(0) == this.Acts.Sum(x => x.Sum);
        }

        public virtual void LoadActs()
        {
            ApplicationContext.Current.Container.UsingForResolved<IDomainService<PerformedWorkAct>>((cnt, service) =>
            {
                this.acts = service.GetAll()
                    .Where(x => x.TypeWorkCr.Id == this.Id)
                    .ToList();

                this.actsLoaded = true;
            });
        }
    }
}