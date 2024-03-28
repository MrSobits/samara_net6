namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction
{
    using System;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Base;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Действие акта проверки
    /// </summary>
    public class ActCheckAction : BaseEntity, IEntityUsedInErknm
    {
        /// <summary>
        /// Акт проверки
        /// </summary>
        public virtual ActCheck ActCheck { get; set; }

        /// <summary>
        /// Вид действия
        /// </summary>
        public virtual ActCheckActionType ActionType { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Место составления
        /// </summary>
        public virtual FiasAddress CreationPlace { get; set; }

        #region Реквизиты
        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? StartDate { get; set; }

        /// <summary>
        /// Время начала
        /// </summary>
        public virtual DateTime? StartTime { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? EndDate { get; set; }

        /// <summary>
        /// Время окончания
        /// </summary>
        public virtual DateTime? EndTime { get; set; }

        /// <summary>
        /// Место проведения
        /// </summary>
        public virtual FiasAddress ExecutionPlace { get; set; }
        #endregion

        #region Контролируемое лицо
        /// <summary>
        /// ФИО контролируемого лица
        /// </summary>
        public virtual string ContrPersFio { get; set; }

        /// <summary>
        /// Дата рождения контролируемого лица
        /// </summary>
        public virtual DateTime? ContrPersBirthDate { get; set; }

        /// <summary>
        /// Место рождения контролируемого лица
        /// </summary>
        public virtual string ContrPersBirthPlace { get; set; }

        /// <summary>
        /// Адрес регистрации контролируемого лица
        /// </summary>
        public virtual string ContrPersRegistrationAddress { get; set; }

        /// <summary>
        /// Адрес проживания контролируемого лица совпадает с адресом регистрации?
        /// </summary>
        public virtual bool ContrPersLivingAddressMatched { get; set; }

        /// <summary>
        /// Адрес проживания контролируемого лица
        /// </summary>
        public virtual string ContrPersLivingAddress { get; set; }

        /// <summary>
        /// Контролируемое лицо наниматель?
        /// </summary>
        public virtual bool ContrPersIsHirer { get; set; }

        /// <summary>
        /// Номер телефона контролируемого лица
        /// </summary>
        public virtual string ContrPersPhoneNumber { get; set; }

        /// <summary>
        /// Место работы контролируемого лица
        /// </summary>
        public virtual string ContrPersWorkPlace { get; set; }

        /// <summary>
        /// Должность контролируемого лица
        /// </summary>
        public virtual string ContrPersPost { get; set; }
        #endregion

        #region Документ, удостоверяющий личность
        /// <summary>
        /// Тип документа, удостоверяющего личность
        /// </summary>
        public virtual IdentityDocumentType IdentityDocType { get; set; }

        /// <summary>
        /// Серия документа, удостоверяющего личность
        /// </summary>
        public virtual string IdentityDocSeries { get; set; }

        /// <summary>
        /// Номер документа, удостоверяющего личность
        /// </summary>
        public virtual string IdentityDocNumber { get; set; }

        /// <summary>
        /// Дата выдачи документа, удостоверяющего личность
        /// </summary>
        public virtual DateTime? IdentityDocIssuedOn { get; set; }

        /// <summary>
        /// Кем выдан документ, удостоверяющий личность
        /// </summary>
        public virtual string IdentityDocIssuedBy { get; set; }
        #endregion

        #region Представитель
        /// <summary>
        /// ФИО представителя
        /// </summary>
        public virtual string RepresentFio { get; set; }

        /// <summary>
        /// Место работы представителя
        /// </summary>
        public virtual string RepresentWorkPlace { get; set; }

        /// <summary>
        /// Должность представителя
        /// </summary>
        public virtual string RepresentPost { get; set; }

        /// <summary>
        /// Номер доверенности представителя
        /// </summary>
        public virtual string RepresentProcurationNumber { get; set; }

        /// <summary>
        /// Дата выдачи доверенности представителя
        /// </summary>
        public virtual DateTime? RepresentProcurationIssuedOn { get; set; }

        /// <summary>
        /// Срок действия доверенности представителя
        /// </summary>
        public virtual string RepresentProcurationValidPeriod { get; set; }
        #endregion

        /// <summary>
        /// Не хранимое поле
        /// Id прототипа для создания действия
        /// </summary>
        public virtual long PrototypeId { get; set; }

        /// <summary>
        /// Не хранимое поле
        /// Вид действия прототипа
        /// </summary>
        public virtual ActCheckActionType? PrototypeActionType { get; set; }
        
        /// <inheritdoc />
        public virtual string ErknmGuid { get; set; }

        public ActCheckAction()
        {
        }

        public ActCheckAction(ActCheckAction action)
        {
            var notCopyProperties = new[]
            {
                nameof(action.Number),
                nameof(action.ErknmGuid)
            };

            action.GetType()
                .GetProperties()
                .Where(x => !notCopyProperties.Contains(x.Name))
                .ForEach(property =>
            {
                this.GetType().GetProperty(property.Name)?.SetValue(this, property.GetValue(action));
            });
        }
    }
}