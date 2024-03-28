namespace Bars.Gkh.RegOperator.Entities
{
    using System;

    /// <summary>
    /// Класс Dto для <see cref="BasePersonalAccount"/>
    /// </summary>
    public class ViewPersonalAccountDto : PersonalAccountDto
    {
        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime ObjectEditDate { get; set; }
    }
}