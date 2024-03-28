namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using B4.Modules.FileStorage;
    using B4.Modules.States;

    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.RegOperator.DomainModelServices;

    using Gkh.Entities;
    using Dict;
    using Enums;
    using Gkh.Utils;
    using GkhCr.Entities;
    using GkhRf.Enums;

    /// <summary>
    /// Заявка на перечисление средств подрядчикам
    /// </summary>
    public class TransferCtr : BaseGkhEntity, IStatefulEntity, ITransferParty
    {
        public TransferCtr()
        {
            this.TransferGuid = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Тип программы заявки перечисления рег.фонда
        /// </summary>
        public virtual TypeProgramRequest TypeProgramRequest { get; set; }

        /// <summary>
        /// Программа кап. ремонта
        /// </summary>
        public virtual ProgramCr ProgramCr { get; set; }

        /// <summary>
        /// Объект КР
        /// </summary>
        public virtual ObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Банк контрагента
        /// </summary>
        public virtual ContragentBank ContragentBank { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public virtual string Perfomer { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер зявки
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Дата заявки
        /// </summary>
        public virtual DateTime? DateFrom { get; set; }

        /// <summary>
        /// Номер зявки ПП
        /// </summary>
        public virtual int DocumentNumPp { get; set; }

        /// <summary>
        /// Дата заявки ПП
        /// </summary>
        public virtual DateTime? DateFromPp { get; set; }

        /// <summary>
        /// Не хранимое
        /// </summary>
        [Obsolete]
        public virtual int? TransferFundsCount { get; set; }

        /// <summary>
        /// Не хранимое
        /// </summary>
        [Obsolete]
        public virtual decimal? TransferFundsSum { get; set; }

        /// <summary>
        /// Тип платежа
        /// </summary>
         public virtual TypePaymentRfCtr PaymentType { get; set; }

         /// <summary>
         /// Подрядчик
         /// </summary>
         public virtual Builder Builder { get; set; }

         /// <summary>
         /// Договор подряда
         /// </summary>
         public virtual BuildContract Contract { get; set; }

         /// <summary>
         /// Разрез финансирования
         /// </summary>
         public virtual FinanceSource FinSource { get; set; }

         /// <summary>
         /// Средства собственника
         /// </summary>
        [Obsolete]
         public virtual decimal? OwnerResource { get; set; }

         /// <summary>
         /// Средства фонда
         /// </summary>
        [Obsolete]
         public virtual decimal? FundResource { get; set; }

         /// <summary>
         /// Бюджет МО
         /// </summary>
        [Obsolete] 
         public virtual decimal? BudgetMu { get; set; }

         /// <summary>
         /// Бюджет субъекта
         /// </summary>
        [Obsolete]
         public virtual decimal? BudgetSubject { get; set; }

         /// <summary>
         /// Вид работы
         /// </summary>
         public virtual TypeWorkCr TypeWorkCr { get; set; }

         /// <summary>
         /// Назначение платежа
         /// </summary>
         public virtual string PaymentPurposeDescription { get; set; }

         /// <summary>
         /// Региональный оператор
         /// </summary>
         public virtual RegOperator RegOperator { get; set; }

         /// <summary>
         /// Расчетный счет регионального оператора
         /// </summary>
         public virtual RegopCalcAccount RegopCalcAccount { get; set; }

         /// <summary>
         /// Типы программы КР
         /// </summary>
         public virtual ProgramCrType ProgramCrType { get; set; }

         /// <summary>
         /// Комментарий
         /// </summary>
         public virtual string Comment { get; set; }

         /// <summary>
         ///  Вид платежа
         /// </summary>
         public virtual KindPayment KindPayment { get; set; }

        /// <summary>
        /// Сумма к оплате
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Оплаченная сумма
        /// </summary>
        public virtual decimal PaidSum { get; set; }

        /// <summary>
        /// Дата оплаты
        /// </summary>
        public virtual DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Гуид
        /// </summary>
        public virtual string TransferGuid { get; set; }

        /// <summary>
        /// Сформирован документ
        /// </summary>
        public virtual bool IsExport { get; set; }

        /// <summary>
        /// Очередность платежа
        /// </summary>
        public virtual string PaymentPriority { get; set; }

        /// <summary>
        /// Редактировать назначение платежа
        /// </summary>
        public virtual bool IsEditPurpose { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual FileInfo Document { get; set; }

        /// <summary>
        /// Вид расчета НДС
        /// </summary>
        public virtual TypeCalculationNds TypeCalculationNds { get; set; }

        /// <summary>
        /// Применить оплату
        /// </summary>
        public virtual void ApplyPayment(decimal paidSum, DateTime paymentDate)
        {
            if (this.PaidSum + paidSum != this.Sum)
            {
                throw new InvalidOperationException("Нельзя оплатить сумму, большую чем заявка");
            }

            this.PaidSum += paidSum;
            this.PaymentDate = paymentDate;
        }

        public virtual void UndoPayment(decimal sum)
        {
            if (this.PaidSum < sum)
            {
                throw new InvalidOperationException("Нельзя отменить оплату на сумму большую, чем было оплачено");
            }

            this.PaidSum -= sum;
            this.PaymentDate = null;
        }

        public virtual string GetMyInfo()
        {
            return string.Format("№ {0} от {1}", this.DocumentNum, this.DateFrom.ToDateString());
        }
    }
}