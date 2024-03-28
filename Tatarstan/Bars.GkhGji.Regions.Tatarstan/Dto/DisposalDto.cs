namespace Bars.GkhGji.Regions.Tatarstan.Dto
{
    using System;

    using Bars.B4.Modules.States;
    using Bars.GkhGji.Enums;
    
    /// <summary>
    /// ДТО для отображения реестров Распоряжения, Решения
    /// </summary>
    public class DisposalDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// Статус
        /// </summary>
        public State State { get; set; }
        
        /// <summary>
        /// Дата начала обследования
        /// </summary>
        public DateTime? DateStart { get; set; }
        
        /// <summary>
        /// Дата окончания обследования
        /// </summary>
        public DateTime? DateEnd { get; set; }
        
        /// <summary>
        /// Дата документа
        /// </summary>
        public DateTime? DocumentDate { get; set; }
        
        /// <summary>
        /// Номер документа
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Номер документа (целая часть)
        /// </summary>
        public int? DocumentNum { get; set; }

        /// <summary>
        /// Тип основания проверки
        /// </summary>
        public TypeBase TypeBase { get; set; }
        
        /// <summary>
        /// Наименование вида проверки
        /// </summary>
        public string KindCheck { get; set; }
        
        /// <summary>
        /// Контрагент
        /// </summary>
        public string ContragentName { get; set; }
        
        /// <summary>
        /// Наименование муниципальных образований
        /// </summary>
        public string MunicipalityNames { get; set; }

        /// <summary>
        /// Идентификатор МО
        /// </summary>
        public long? MunicipalityId { get; set; }

        /// <summary>
        /// Адреса жилых домов
        /// </summary>
        public string PersonInspectionAddress { get; set; }

        /// <summary>
        /// Создан акт проверки общий
        /// </summary>
        public bool? IsActCheckExist { get; set; }

        /// <summary>
        /// Количество домов
        /// </summary>
        public int? RealityObjectCount { get; set; }

        /// <summary>
        /// Типы обследования
        /// </summary>
        public string TypeSurveyNames { get; set; }

        /// <summary>
        /// ФИО инспекторов
        /// </summary>
        public string InspectorNames { get; set; }

        /// <summary>
        /// Основание проверки
        /// </summary>
        public long? InspectionId { get; set; }

        /// <summary>
        /// Тип документа ГЖИ
        /// </summary>
        public TypeDocumentGji TypeDocumentGji { get; set; }

        /// <summary>
        /// Тип согласования с прокуратурой
        /// </summary>
        public TypeAgreementProsecutor TypeAgreementProsecutor { get; set; }

        /// <summary>
        /// Вид контроля
        /// </summary>
        public string ControlType { get; set; }

        /// <summary>
        /// Создан акт обследования
        /// </summary>
        public bool HasActSurvey { get; set; }

        /// <summary>
        /// Номер лицензии
        /// </summary>
        public string LicenseNumber { get; set; }

        /// <summary>
        /// Учетный номер проверки в ЕРП
        /// </summary>
        public string ErpRegistrationNumber { get; set; }
        
        /// <summary>
        /// Учетный номер решения в ЕРКНМ
        /// </summary>
        public string ErknmRegistrationNumber { get; set; }
    }
}