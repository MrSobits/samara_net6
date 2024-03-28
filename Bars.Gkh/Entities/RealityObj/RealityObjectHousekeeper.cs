namespace Bars.Gkh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Старший по дому
    /// </summary>
    public class RealityObjectHousekeeper : BaseImportableEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Действует
        /// </summary>
        public virtual YesNoNotSet IsActive { get; set; }
        
        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string FIO { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public virtual string Login { get; set; }

        /// <summary>
        /// новый Логин
        /// </summary>
        public virtual string NewPassword { get; set; }

        /// <summary>
        /// подтверждение Логин
        /// </summary>
        public virtual string NewConfirmPassword { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public virtual string PhoneNumber { get; set; }
    }
}