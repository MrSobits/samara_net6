namespace Bars.FIAS.Converter
{
    using System;

    public class FiasHouseRecord
    {
        #region Данные поля незагружаются из ФИАС

        /// <summary>
        /// Идентификатор записи в Б4 (Он нужен для Update)
        /// </summary>
        public int Id;

        /// <summary>
        /// Тип записи. всегда 10 (1-Это добавленная пользователем)
        /// </summary>
        public int TypeRecord = 10;

        #endregion

        /// <summary>
        /// Уникальный идентификатор записи дома
        /// </summary>
        public string HouseId { get; set; }

        /// <summary>
        /// Глобальный уникальный идентификатор дома
        /// </summary>
        public string HouseGuid { get; set; }

        /// <summary>
        /// Guid записи родительского объекта
        /// </summary>
        public string AoGuid { get; set; }

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// ОКАТО
        /// </summary>
        public string Okato { get; set; }

        /// <summary>
        /// ОКТМО
        /// </summary>
        public string Oktmo { get; set; }

        /// <summary>
        /// Номер дома
        /// </summary>
        public string HouseNum { get; set; }

        /// <summary>
        /// Номер корпуса
        /// </summary>
        public string BuildNum { get; set; }

        /// <summary>
        /// Номер строения
        /// </summary>
        public string StrucNum { get; set; }

        /// <summary>
        /// Состояние актуальности, принимает значение: 0 – Не актуальный, 1 – Актуальный
        /// </summary>
        public int ActualStatus { get; set; }

        /// <summary>
        /// Дата внесения записи
        /// </summary>
        public DateTime? UpdateDate;

        /// <summary>
        /// Начало действия записи
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Окончание действия записи
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}