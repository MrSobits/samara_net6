namespace Bars.Gkh.Entities
{
	using System;

	using Bars.Gkh.Entities.Base;
	using Bars.Gkh.Enums;

    /// <summary>
    /// Муниципальное образование
    /// </summary>
    public class Municipality : BaseGkhEntity, IUsedInTorIntegration
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Guid ФИАС
        /// </summary>
        public virtual string FiasId { get; set; }

        /// <summary>
        /// Группа
        /// </summary>
        public virtual string Group { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// ОКАТО
        /// </summary>
        public virtual string Okato { get; set; }

        /// <summary>
        /// Описание, комментарий
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Федеральный номер
        /// </summary>
        public virtual string FederalNumber { get; set; }

        /// <summary>
        /// Сокращение
        /// </summary>
        public virtual string Cut { get; set; }

        /// <summary>
        /// Наименование региона
        /// </summary>
        public virtual string RegionName { get; set; }

        /// <summary>
        /// Гуид, полученный из карты
        /// </summary>
        public virtual string MapGuid { get; set; }

        /// <summary>
        /// Массив координат полигона
        /// </summary>
        public virtual string PolygonPointsArray { get; set; }

        /// <summary>
        /// Проверять сертификат пользователей, отоносящихся к данному МО, при полдписывании
        /// </summary>
        public virtual bool CheckCertificateValidity { get; set; }

        /// <summary>
        /// ОКТМО
        /// </summary>
        public virtual string Oktmo { get; set; }

        /// <summary>
        /// Муниципальный район
        /// </summary>
        public virtual Municipality ParentMo { get; set; }

        /// <summary>
        /// Уровень
        /// </summary>
        public virtual TypeMunicipality Level { get; set; }

        /// <summary>
        /// Признак МО, было заведено до импорта 2х уровневого справочника
        /// </summary>
        public virtual bool IsOld { get; set; }

	    /// <summary>
	    /// Идентификатор ТОР КНД
	    /// </summary>
		public virtual Guid? TorId { get; set; }

	    /// <summary>
	    /// Код ГЖИ
	    /// </summary>
	    public virtual string CodeGji { get; set; }
    }
}
