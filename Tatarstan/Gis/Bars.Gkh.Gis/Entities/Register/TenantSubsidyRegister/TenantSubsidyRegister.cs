namespace Bars.Gkh.Gis.Entities.Register.TenantSubsidyRegister
{
    using System;
    using B4.DataAccess;

    using Bars.Gkh.Entities.Dicts;

    using LoadedFileRegister;

    /// <summary>
    /// Субсидии по жильцам
    /// </summary>
    public class TenantSubsidyRegister : BaseEntity
    {
        /// <summary>
        /// ПСС
        /// </summary>
        public virtual string Pss { get; set; }

        /// <summary>
        /// Расчетный месяц
        /// </summary>
        public virtual DateTime CalculationMonth { get; set; }

        /// <summary>
        /// Лицевой счет управляющей компании
        /// </summary>
        public virtual long ManagementOrganizationAccount { get; set; }

        /// <summary>
        /// Фамилия плучателя
        /// </summary>
        public virtual string Surname { get; set; }

        /// <summary>
        /// Имя получателя
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Отчество получателя
        /// </summary>
        public virtual string Patronymic { get; set; }

        /// <summary>
        /// Дата рождения получателся
        /// </summary>
        public virtual DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Код статьи начисленной суммы
        /// </summary>
        public virtual long ArticleCode { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public virtual ServiceDictionary Service { get; set; }

        /// <summary>
        /// Наименование банка, в который перечислены субсидии
        /// </summary>
        public virtual string BankName { get; set; }

        /// <summary>
        /// Дата начала предоставления субсидий
        /// </summary>
        public virtual DateTime BeginDate { get; set; }

        /// <summary>
        /// Сумма входящего сальдо субсидий - льгот получателя
        /// </summary>
        public virtual double IncomingSaldo { get; set; }

        /// <summary>
        /// Начисленная сумма субсидий - льгот получателя
        /// </summary>
        public virtual double AccruedSum { get; set; }

        /// <summary>
        /// Начисленная перерасчетом сумма субсидий - льгот получателя
        /// </summary>
        public virtual double RecalculatedSum { get; set; }

        /// <summary>
        /// Авансовый платеж субсидий - льгот получателя
        /// </summary>
        public virtual double AdvancedPayment { get; set; }

        /// <summary>
        /// Сумма к выплате субсидий - льгот получателя
        /// </summary>
        public virtual double PaymentSum { get; set; }

        /// <summary>
        /// Сумма субсидий - СМО РФ
        /// </summary>
        public virtual double SmoSum { get; set; }

        /// <summary>
        /// Сумма перерасчетов субсидий - СМО РФ
        /// </summary>
        public virtual double SmoRecalculatedSum { get; set; }

        /// <summary>
        /// Сумма изменений субсидий - льгот получателя
        /// </summary>
        public virtual double ChangesSum { get; set; }

        /// <summary>
        /// Дата окончания предоставления субсидий
        /// </summary>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// Подразделение организации
        /// </summary>
        public virtual string OrganizationUnit { get; set; }

        /// <summary>
        /// Загруженный файл
        /// </summary>
        public virtual LoadedFileRegister LoadedFile { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        public virtual long PersonalAccountId { get; set; }
    }
}
