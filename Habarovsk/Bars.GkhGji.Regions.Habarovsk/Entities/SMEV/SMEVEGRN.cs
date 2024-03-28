namespace Bars.GkhGji.Regions.Habarovsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using System;
    using Enums;
    using Bars.GkhGji.Regions.Habarovsk.Enums.Egrn;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    public class SMEVEGRN : BaseEntity
    {
        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Дата запроса
        /// </summary>
        public virtual DateTime RequestDate { get; set; }

        /// <summary>
        /// Код региона основной
        /// </summary>
        public virtual RegionCodeMVD RegionCode { get; set; }

        /// <summary>
        /// Тип запроса
        /// </summary>
        public virtual RequestType RequestType { get; set; }

        /// <summary>
        /// Вид запроса данных по ОН
        /// </summary>
        public virtual RequestDataType RequestDataType { get; set; }

        /// <summary>
        /// ID заявителя, представителя заявителя
        /// </summary>
        public virtual Guid DeclarantId { get; set; }

        /// <summary>
        /// Тип заявителя
        /// </summary>
        public virtual EGRNApplicantType EGRNApplicantType { get; set; }

        /// <summary>
        /// Тип объекта
        /// </summary>
        public virtual EGRNObjectType EGRNObjectType { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string MessageId { get; set; }

        /// <summary>
        /// Фамилия Заявителя
        /// </summary>
        public virtual string PersonSurname { get; set; }

        /// <summary>
        /// Отчество Заявителя
        /// </summary>
        public virtual string PersonPatronymic { get; set; }

        /// <summary>
        /// Номер документа физлица
        /// </summary>
        public virtual String DocumentNumber { get; set; }

        /// <summary>
        /// Серия документа физлица
        /// </summary>
        public virtual String DocumentSerial { get; set; }

        /// <summary>
        /// Имя Заявителя
        /// </summary>
        public virtual string PersonName { get; set; }

        /// <summary>
        /// Данные удостоверения личности
        /// </summary>
        public virtual Guid IdDocumentRef { get; set; }

        /// <summary>
        /// Жилой дом, по которому запрашивается выписка
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Помещение, по которому запрашивается выписка
        /// </summary>
        public virtual Room Room { get; set; }

        /// <summary>
        /// Телефон для службы опроса качества
        /// </summary>
        public virtual string QualityPhone { get; set; }

        /// <summary>
        /// Кадастровый номер помещения
        /// </summary>
        public virtual string CadastralNUmber { get; set; }

        /// <summary>
        /// ID заявки по которой создан запрос
        /// </summary>
        public virtual string ProtocolOSPRequestID { get; set; }
    }
}
