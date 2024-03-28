namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Резолюция АС ДОУ
    /// </summary>
    public class AppealCitsResolutionExecutor : BaseGkhEntity
    {
        /// <summary>
        /// Резолюция
        /// </summary>
        public virtual AppealCitsResolution AppealCitsResolution { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string Surname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string Patronymic { get; set; }

        /// <summary>
        /// Персональный срок
        /// </summary>
        public virtual DateTime PersonalTerm { get; set; }

        /// <summary>
        /// Комментарий исполнителю
        /// </summary>
        public virtual string Comment { get; set; }

        /// <summary>
        /// Ответственный
        /// </summary>
        public virtual YesNo IsResponsible { get; set; }
    }
}