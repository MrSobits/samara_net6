namespace Bars.Gkh.Entities
{
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Оператор проекта БарсЖКХ
    /// </summary>
    public class Operator : BaseGkhEntity
    {
        /// <summary>
        /// Контрагент для заполнения
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Контрагент для ГИС ЖКХ
        /// </summary>
        public virtual Contragent GisGkhContragent { get; set; }

        /// <summary>
        /// Тип контрагента (для 1468)
        /// </summary>
        public virtual ContragentType ContragentType { get; set; }

        /// <summary>
        ///   Наименование (не хранимое)
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Логин (не хранимое)
        /// </summary>
        public virtual string Login { get; set; }

        /// <summary>
        /// Пароль (не хранимое)
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// Новый пароль (не хранимое)
        /// </summary>
        public virtual string NewPassword { get; set; }

        /// <summary>
        /// Новый пароль подтверждение (не хранимое)
        /// </summary>
        public virtual string NewPasswordCommit { get; set; }

        /// <summary>
        /// E-mail (не хранимое)
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Роль - используется для получения данных с клиента и во время импорта
        /// </summary>
        public virtual Role Role { get; set; }

        /// <summary>
        /// Тип рабочего места
        /// </summary>
        public virtual TypeWorkplace TypeWorkplace { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// Пользователь активен
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Инспектор
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Токен кросс-авторизации РИС
        /// </summary>
        public virtual string RisToken { get; set; }

        /// <summary>
        /// Формат выгрузки отчетов
        /// </summary>
        public virtual OperatorExportFormat ExportFormat { get; set; }
        
        /// Доступ к мобильному приложению
        /// </summary>
        public virtual bool MobileApplicationAccessEnabled { get; set; }

        /// <summary>
        /// Фото пользователя
        /// </summary>
        public virtual FileInfo UserPhoto { get; set; }
    }
}