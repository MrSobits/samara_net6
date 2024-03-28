namespace Bars.GkhCr.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Enums;
    using Newtonsoft.Json;

    /// <summary>
    /// Акт выполненных работ
    /// </summary>
    public class PerformedWorkAct : BaseGkhEntity, IStatefulEntity
    {
        private readonly List<PerformedWorkActPayment> _payments;

        public PerformedWorkAct()
        {
            _payments = new List<PerformedWorkActPayment>();
        }

        public PerformedWorkAct(ObjectCr objectCr)
            : this()
        {
            ObjectCr = objectCr;
        }

        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual ObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Работа
        /// </summary>
        public virtual TypeWorkCr TypeWorkCr { get; set; }

        /// <summary>
        /// Номер акта
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        public virtual decimal? Volume { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        public virtual decimal? FactVolume { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        /// Дата от
        /// </summary>
        public virtual DateTime? DateFrom { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal? SumTransfer { get; set; }

        /// <summary>
        /// Фактическая сумма больше плановой
        /// </summary>
        public virtual bool OverLimits { get; set; }

        /// <summary>
        /// Дата от
        /// </summary>
        public virtual DateTime? DateFromTransfer { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Справка о стоимости выполненных работ и затрат
        /// </summary>
        public virtual FileInfo CostFile { get; set; }

        /// <summary>
        /// Документ акта
        /// </summary>
        public virtual FileInfo DocumentFile { get; set; }

        /// <summary>
        /// Приложение к акту
        /// </summary>
        public virtual FileInfo AdditionFile { get; set; }

        /// <summary>
        /// Выводить документ на портал
        /// </summary>
        public virtual YesNo UsedInExport { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID
        /// </summary>
        public virtual string GisGkhGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ Transport GUID
        /// </summary>
        public virtual string GisGkhTransportGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID документа договора
        /// </summary>
        public virtual string GisGkhDocumentGuid { get; set; }
        
        /// Акт подписан представителем собственников
        /// </summary>
        public virtual YesNo RepresentativeSigned { get; set; }

        /// <summary>
        /// Фамилия представителя
        /// </summary>
        public virtual string RepresentativeSurname { get; set; }

        /// <summary>
        /// Имя представителя
        /// </summary>
        public virtual string RepresentativeName { get; set; }

        /// <summary>
        /// Отчество представителя
        /// </summary>
        public virtual string RepresentativePatronymic { get; set; }

        /// <summary>
        /// Принята в эксплуатацию
        /// </summary>
        public virtual YesNo ExploitationAccepted { get; set; }

        /// <summary>
        /// Дата начала гарантийного срока
        /// </summary>
        public virtual DateTime? WarrantyStartDate { get; set; }

        /// <summary>
        /// Дата окончания гарантийного срока
        /// </summary>
        public virtual DateTime? WarrantyEndDate { get; set; }

        /// <summary>
        /// Оплаты по данному акту
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<PerformedWorkActPayment> Payments
        {
            get { return _payments; }
            protected set
            {
                _payments.Clear();
                if (value != null)
                {
                    _payments.AddRange(value);
                }
            }
        }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject Realty => this.ObjectCr.RealityObject;

        public virtual PerformedWorkActPayment CreatePayment(ActPaymentType type, decimal sum, DateTime date)
        {
            var payment = new PerformedWorkActPayment(this, type, sum, date);
            _payments.Add(payment);
            return payment;
        }


        /// <summary>
        /// Проверка того, что акт оплачен.
        /// Проверяется, что у акта конечный статус и сумма оплат равна сумме в акте
        /// </summary>
        /// <returns></returns>
        public virtual bool IsPaid()
        {
            return State.FinalState && (Sum.HasValue && Sum.Value == Payments.Sum(x => x.Paid));
        }
    }
}
