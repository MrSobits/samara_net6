namespace Bars.Gkh.Overhaul.Hmao.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип основания изменения записи ДПКР
    /// </summary>

    public enum ChangeBasisType
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Нецелесообразность проведения работ
        /// </summary>
        [Display("Нецелесообразность проведения работ")]
        Impractical = 10,

        /// <summary>
        /// Невозможность проведения работ
        /// </summary>
        [Display("Невозможность проведения работ")]
        Impossible = 20,

        /// <summary>
        /// Заключены договоры
        /// </summary>
        [Display("Заключены договоры")]
        ConcludedContracts = 30,

        /// <summary>
        /// Закон ЧО № 430-ЗО
        /// </summary>
        [Display("Закон ЧО № 430-ЗО")]
        Law_430_30 = 40,

        /// <summary>
        /// Лифты
        /// </summary>
        [Display("Лифты")]
        Elevators = 50,

        /// <summary>
        /// Текущие работы
        /// </summary>
        [Display("Текущие работы")]
        OngoingWork = 60
    }
}