namespace Bars.Gkh.RegOperator.Dto
{
    using System;

    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    /// <summary>
    /// Временный объект для хранения использованных параметров.
    /// Создается при каждом вычислении тарифа и т.д.
    /// При закрытии периода используется как источник параметров для фиксирования, затем удаляется
    /// </summary>
    public class PersonalAccountCalcParamDto
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="source"></param>
        public PersonalAccountCalcParamDto(PersonalAccountCalcParam_tmp source)
        {
            this.ObjectCreateDate = source.ObjectCreateDate;
            this.ObjectEditDate = source.ObjectEditDate;
            this.ObjectVersion = source.ObjectVersion;
            this.PersonalAccountId = source.PersonalAccount.Id;
            this.LoggedEntityId = source.LoggedEntity.Id;
        }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime ObjectCreateDate { get; set; }

        /// <summary>
        /// Дата последнего редактирования
        /// </summary>
        public DateTime ObjectEditDate { get; set; }

        /// <summary>
        /// Версия объекта
        /// </summary>
        public int ObjectVersion { get; set; }

        /// <summary>
        /// id ЛС
        /// </summary>
        public long PersonalAccountId { get; set; }

        /// <summary>
        /// id сущности
        /// </summary>
        public long LoggedEntityId { get; set; }
    }
}