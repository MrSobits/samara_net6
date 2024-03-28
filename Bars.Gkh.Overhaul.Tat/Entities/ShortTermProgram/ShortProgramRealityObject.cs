namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;

    /// <summary>
    /// Запись дома в краткосрочной программе
    /// </summary>
    public class ShortProgramRealityObject : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Дом в краткосрочной программе
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Версия прграммы
        /// </summary>
        public virtual ProgramVersion ProgramVersion { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// тип актуализации с Версией
        /// Когда запись из результатов корректировки создаетсяв краткосрочке то Тип = Actual
        /// Когда пользователь в ручную меняет записи то тип меняется на NoActual
        /// Когда пользователи забили все даныне и поставиливсе суммы онинажимают Актуализирвоать с версией и у всех записей проставляется Тип=Actual
        /// </summary>
        public virtual TypeActuality TypeActuality { get; set; }
    }
}