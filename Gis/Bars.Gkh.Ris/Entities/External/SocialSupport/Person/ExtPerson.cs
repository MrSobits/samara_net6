namespace Bars.Gkh.Ris.Entities.External.SocialSupport.Person
{
    using System;

    using B4.DataAccess;
    using Administration.System;
    using Dict.SocialSupport;

    /// <summary>
    /// Физ лицо
    /// </summary>
    public class ExtPerson : BaseEntity
    {
        /// <summary>
        ///  Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }
        /// <summary>
        ///  Фамилия
        /// </summary>
        public virtual string Fam { get; set; }
        /// <summary>
        ///  Имя 
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        ///  Отчество
        /// </summary>
        public virtual string FName { get; set; }
        /// <summary>
        ///  Пол 
        /// </summary>
        public virtual string Sex { get; set; }
        /// <summary>
        ///  Дата рождения  
        /// </summary>
        public virtual DateTime BornOn { get; set; }
        /// <summary>
        ///  НСИ 95 Документ, удостоверяющий личность 
        /// </summary>
        public virtual Passport Passport { get; set; }
        /// <summary>
        ///  Серия 
        /// </summary>
        public virtual string PassSeries { get; set; }
        /// <summary>
        ///  Номер 
        /// </summary>
        public virtual string PassNumber { get; set; }
        /// <summary>
        ///  Дата выдачи 
        /// </summary>
        public virtual DateTime? IssuedOn { get; set; }
        /// <summary>
        ///  СНИЛС 
        /// </summary>
        public virtual string Snils { get; set; }
        /// <summary>
        ///  Место рождения  
        /// </summary>
        public virtual string BirthPlace { get; set; }
        ///// <summary>
        /////  Зарегистрирован
        ///// </summary>
        //public virtual bool IsRegistered { get; set; }
        ///// <summary>
        /////  Постоянный житель 
        ///// </summary>
        //public virtual bool IsResident { get; set; }
        /// <summary>
        /// Пользователь 
        /// </summary>
        public virtual int ChangedBy { get; set; }
        /// <summary>
        /// Дата изменения 
        /// </summary>
        public virtual DateTime ChangedOn { get; set; }
    }
}
