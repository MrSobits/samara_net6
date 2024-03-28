namespace Bars.GisIntegration.Base.Entities.External.Housing.PersonalAccount
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;
    using Bars.GisIntegration.Base.Entities.External.Contragent;
    using Bars.GisIntegration.Base.Entities.External.Dict.PersonalAccount;
    using Bars.GisIntegration.Base.Entities.External.SocialSupport.Person;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Лицевой счет
    /// </summary>
    public class ExtPersonalAccount : BaseEntity
    {
        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }

        /// <summary>
        /// Уникальный идентификатор в ГИС ЖКХ
        /// </summary>
        public virtual string GisGuid { get; set; }

        /// <summary>
        /// Номер лицевого счета/Иной идентификатор плательщика
        /// </summary>
        public virtual string AccountNumber { get; set; }

        /// <summary>
        /// Тип лицевого счета 
        /// </summary>
        public virtual LsType LsType { get; set; }

        /// <summary>
        /// Дата начала действия ЛС
        /// </summary>
        public virtual DateTime? OpenedOn { get; set; }

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
        ///  Дата рождения  
        /// </summary>
        public virtual DateTime BornOn { get; set; }

        /// <summary>
        /// Количество проживающих
        /// </summary>
        public virtual int GilCnt { get; set; }

        /// <summary>
        /// Общая площадь для ЛС
        /// </summary>
        public virtual decimal? TotalSquare { get; set; }

        /// <summary>
        /// Жилая площадь для ЛС
        /// </summary>
        public virtual decimal? LiveSquare { get; set; }

        /// <summary>
        /// Отапливаемая площадь для ЛС
        /// </summary>
        public virtual decimal? HeatSquare { get; set; }

        /// <summary>
        /// Лицевой счет закрыт
        /// </summary>
        public virtual bool IsClosed { get; set; }

        /// <summary>
        /// НСИ 22. Причина закрытия счета
        /// </summary>
        public virtual PersonalAccountCloseReason PersonalAccountCloseReason { get; set; }

        /// <summary>
        /// Дата закрытия
        /// </summary>
        public virtual DateTime? ClosedOn { get; set; }

        /// <summary>
        /// Примечание к закрытию
        /// </summary>
        public virtual string CloseComment { get; set; }

        /// <summary>
        /// Плательщик - Физ лицо
        /// </summary>
        public virtual ExtPerson PayPerson { get; set; }

        /// <summary>
        /// Плательщик – Организация
        /// </summary>
        public virtual ExtContragent PayContragent { get; set; }

        /// <summary>
        /// Плательщик является нанимателем
        /// </summary>
        public virtual bool IsRenter { get; set; }

        /// <summary>
        /// Участок (ЖЭУ)
        /// </summary>
        public virtual string Geu { get; set; }

        /// <summary>
        /// Контрагент - Управляющая организация
        /// </summary>
        public virtual ExtContragent UoContragent { get; set; }

        /// <summary>
        /// Тип владения 
        /// </summary>
        public virtual string OwnType { get; set; }

        /// <summary>
        /// Удален
        /// </summary>
        public virtual bool IsDel { get; set; }

        /// <summary>
        /// Пользователь 
        /// </summary>
        public virtual int ChangedBy { get; set; }

        /// <summary>
        /// Дата изменения 
        /// </summary>
        public virtual DateTime ChangedOn { get; set; }

        /// <summary>
        /// Тип абонента
        /// </summary>
        public virtual RisAbonentType AbonentType { get; set; }
    }
}
