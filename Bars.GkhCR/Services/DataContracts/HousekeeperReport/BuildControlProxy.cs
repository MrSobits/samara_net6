namespace Bars.GkhCr.Services
{
    using Bars.GkhCr.Services.DataContracts;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Пользователь стройконтроля, возвращаем на метод аутентификации пользователя
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "CheckBuildControlUserResponce")]
    public class CheckBuildControlUserResponce
    {
        /// <summary>
        /// Юзер стройконтроля 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// Контрагент стройконтроля 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ContragentId")]
        public long ContragentId { get; set; }

        /// <summary>
        /// Результат 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }

    /// <summary>
    /// Объекты для пользователя стройконтроля
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "GetBuildControlObjectsResponce")]
    public class GetBuildControlObjectsResponce
    {
        /// <summary>
        /// Объекты КР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Objects")]
        public BuildControlObjectCR[] Objects { get; set; }

        /// <summary>
        /// Результат 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }

    /// <summary>
    /// Объекты для пользователя стройконтроля
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "GetBuildControlObjectResponce")]
    public class GetBuildControlObjectResponce
    {
        /// <summary>
        /// Объекты КР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "BuildControlObjectCR")]
        public BuildControlObjectCR BuildControlObjectCR { get; set; }

        /// <summary>
        /// Результат 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }

    /// <summary>
    /// Объекты для пользователя стройконтроля
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "GetBuildControlReportListResponce")]
    public class GetBuildControlReportListResponce
    {
        /// <summary>
        /// Объекты КР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = " BuildControlReportList")]
        public BuildControlReportListItemProxy[] BuildControlReportList { get; set; }

        /// <summary>
        /// Результат 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }

    /// <summary>
    /// Информация об объектах КР
    /// </summary>
    public class BuildControlObjectCR
    {
        /// <summary>
        /// Id программы
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ObjectId")]
        public long ObjectId { get; set; }

        /// <summary>
        /// адрес люъекта
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// Наименование программы КПР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ProgramName")]
        public string ProgramName { get; set; }

        /// <summary>
        /// Период проведения ремонта
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Period")]
        public string Period { get; set; }     

        /// <summary>
        /// Работы СМР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Works")]
        public CRObjectWorkSmr[] Works { get; set; }
    }

    [DataContract]
    [XmlRoot(ElementName = "CRObjectWorkSmr")]
    public class CRObjectWorkSmr
    {
        /// <summary>
        /// Ид TypeWorkCr
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "WorkId")]
        public long WorkId { get; set; }

        /// <summary>
        /// Наименование TypeWorkCr
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "WorkName")]
        public string WorkName { get; set; }

        /// <summary>
        /// Ид подрядчика СМР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ContragentId")]
        public long ContragentId { get; set; }

        /// <summary>
        /// Наименование подрядчика СМР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ContragentName")]
        public string ContragentName { get; set; }

        /// <summary>
        /// Номер и дата договора СМР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ContractNumber")]
        public string ContractNumber { get; set; }

        /// <summary>
        /// статус договора СМР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ContractState")]
        public string ContractState { get; set; }

        /// <summary>
        /// Дата начала работ СМР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DateFrom")]
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Дата окончания работ СМР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DateTo")]
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Этапы работ
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "WorkStages")]
        public CRObjectWorkStage[] WorkStages { get; set; }

    }

    /// <summary>
    /// Этап работы СМР TypeWorkCrAddWork
    /// </summary>
    [DataContract]
    [XmlRoot(ElementName = "CRObjectWorkStage")]
    public class CRObjectWorkStage
    {
        /// <summary>
        /// Ид TypeWorkCr
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "WorkId")]
        public long WorkId { get; set; }

        /// <summary>
        /// Ид TypeWorkCrAddWork
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "StageId")]
        public long StageId { get; set; }

        /// <summary>
        /// Наименование этапа AdditWork.Name
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "StageName")]
        public string WorkName { get; set; }

        /// <summary>
        /// Дата начала этапа работ СМР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DateStartWork")]
        public DateTime DateStartWork { get; set; }

        /// <summary>
        /// Дата окончания этапа работ СМР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DateEndWork")]
        public DateTime DateEndWork { get; set; }
    }

    /// <summary>
    /// Информация о работах ДПКР
    /// </summary>
    public class BuildControlReportProxy
    {
        /// <summary>
        /// Объект КР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Объект КР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ObjectCRId")]
        public long ObjectCRId { get; set; }

        /// <summary>
        /// Этап работы
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "StageId")]
        public long StageId { get; set; }

        /// <summary>
        /// Процент выполнения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "PercentOfCompletion")]
        public decimal PercentOfCompletion { get; set; }

        /// <summary>
        /// Широта
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Latitude")]
        public decimal Latitude { get; set; }

        /// <summary>
        /// Широта
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Longitude")]
        public decimal Longitude { get; set; }

        /// <summary>
        /// Работа КР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "WorkId")]
        public long WorkId { get; set; }

        /// <summary>
        ///Исполнитель работ СМР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ContragentSMRId")]
        public long ContragentSMRId { get; set; }

        /// <summary>
        /// Контрагент стройконтроль
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ContragentSKId")]
        public long ContragentSKId { get; set; }

        /// <summary>
        /// Срыв срока
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DeadlineMissed")]
        public bool DeadlineMissed { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Файлы  отчета
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ReportFiles")]
        public ReportFile[] ReportFiles { get; set; }
    }

    /// <summary>
    /// Информация о работах ДПКР
    /// </summary>
    public class BuildControlReportListItemProxy
    {
        /// <summary>
        /// Объект КР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Объект КР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ObjectCR")]
        public string ObjectCR { get; set; }

        /// <summary>
        /// Этап работы
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "StageId")]
        public string StageName { get; set; }


        /// <summary>
        /// Работа КР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "WorkName")]
        public string WorkName { get; set; }

        /// <summary>
        ///Исполнитель работ СМР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ContragentSMR")]
        public string ContragentSMR { get; set; }

        /// <summary>
        /// Контрагент стройконтроль
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ContragentSK")]
        public string ContragentSK { get; set; }

        /// <summary>
        /// Срыв срока
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DeadlineMissed")]
        public bool DeadlineMissed { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }

    }

    /// <summary>
    /// Ответ на добавление отчета
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "InsertBuildControlReportResponce")]
    public class InsertBuildControlReportResponce
    {
        /// <summary>
        /// Результат 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}