namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    
    // TODO: Расскоментировать после перевода  GisIntegration
    //using Bars.GisIntegration.Base.Entities;
    
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Enums;
    using Bars.GkhGji.ViewModel;

    using ControlType = Bars.GkhGji.Entities.Dict.ControlType;

    public class TatarstanDisposalViewModel : TatarstanDisposalViewModel<TatarstanDisposal>
    {
        // Внимание!! Код override писать в Generic калссе
    }

    // Generic-класс для того, чтобы сущность Disposal можно было расширять через subclass в других регионах
    public class TatarstanDisposalViewModel<T> : DisposalViewModel<T> where T : TatarstanDisposal
    {
        public IRepository<T> TatDisposalRepo { get; set; }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            // TODO: Расскоментировать после перевода  GisIntegration
           /* var loadParams = this.GetLoadParam(baseParams);
            var risTaskDomain = this.Container.Resolve<IDomainService<RisTask>>();

            using (this.Container.Using(risTaskDomain))
            {
                var risTaskDict = risTaskDomain.GetAll()
                    .Where(x => x.DocumentGji.TypeDocumentGji == TypeDocumentGji.Disposal)
                    .GroupBy(x => x.DocumentGji.Id)
                    .Select(x => new
                    {
                        DisposalId = x.Key,
                        LastMethodStartTime = x.Max(y => y.StartTime)
                    })
                    .ToDictionary(x => x.DisposalId, x => x.LastMethodStartTime);
                
                return this.TatDisposalRepo.GetAll()
                    .Where(x => x.IsSentToErp == true)
                    .Select(x => new
                    {
                        x.Id,
                        x.DocumentNumber,
                        x.DocumentDate,
                        x.ErpGuid,
                        x.ErpRegistrationDate,
                        InspectionId = x.Inspection.Id,
                        DocumentType = x.TypeDocumentGji,
                        DocumentTypeBase = x.Inspection.TypeBase
                    })
                    .AsEnumerable()
                    .Select( x => new
                    {
                        x.Id,
                        x.DocumentNumber,
                        x.DocumentDate,
                        x.ErpGuid,
                        x.ErpRegistrationDate,
                        x.InspectionId,
                        x.DocumentType,
                        x.DocumentTypeBase,
                        LastMethodStartTime = risTaskDict.Get(x.Id)
                    })
                    .ToListDataResult(loadParams, this.Container);
            }*/
           return null;
        }

        public override IDataResult Get(IDomainService<T> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var disposal = domainService.Get(id);

            return new BaseDataResult(this.GetDtoWithDefaultProperties(disposal));
        }

        protected TatarstanDisposalDto GetDtoWithDefaultProperties(T disposal)
        {
            var serviceDocumentChildren = this.Container.ResolveDomain<DocumentGjiChildren>();

            using (this.Container.Using(serviceDocumentChildren))
            {
                return new TatarstanDisposalDto
                {
                    Id = disposal.Id,
                    IssuedDisposal = disposal.IssuedDisposal,
                    ResponsibleExecution = disposal.ResponsibleExecution,
                    DateStart = disposal.DateStart,
                    DateEnd = disposal.DateEnd,
                    CountHours = disposal.CountHours,
                    CountDays = disposal.CountDays,
                    DocumentNumberWithResultAgreement = disposal.DocumentNumberWithResultAgreement,
                    DocumentDateWithResultAgreement = disposal.DocumentDateWithResultAgreement,
                    TypeDisposal = disposal.TypeDisposal,
                    TypeAgreementProsecutor = disposal.TypeAgreementProsecutor,
                    InformationAboutHarm = disposal.InformationAboutHarm,
                    TypeAgreementResult = disposal.TypeAgreementResult,
                    KindCheck = disposal.KindCheck,
                    TypeDocumentGji = disposal.TypeDocumentGji,
                    Description = disposal.Description,
                    ObjectVisitStart = disposal.ObjectVisitStart,
                    ObjectVisitEnd = disposal.ObjectVisitEnd,
                    OutInspector = disposal.OutInspector,
                    DocumentDate = disposal.DocumentDate,
                    DocumentNum = disposal.DocumentNum,
                    DocumentNumber = disposal.DocumentNumber,
                    LiteralNum = disposal.LiteralNum,
                    DocumentSubNum = disposal.DocumentSubNum,
                    DocumentYear = disposal.DocumentYear,
                    State = disposal.State,
                    NcNum = disposal.NcNum,
                    NcDate = disposal.NcDate,
                    NcNumLatter = disposal.NcNumLatter,
                    NcDateLatter = disposal.NcDateLatter,
                    NcObtained = disposal.NcObtained,
                    NcSent = disposal.NcSent,
                    Prosecutor = disposal.Prosecutor,
                    ReasonErpChecking = disposal.ReasonErpChecking,
                    NotificationType = disposal.NotificationType,
                    InspectionBase = disposal.InspectionBase,
                    ControlType = disposal.ControlType,
                    TorId = disposal.TorId,
                    InteractionPersonHour = disposal.InteractionPersonHour,
                    InteractionPersonMinutes = disposal.InteractionPersonMinutes,
                    SuspensionInspectionBase = disposal.SuspensionInspectionBase,
                    SuspensionDateFrom = disposal.SuspensionDateFrom,
                    SuspensionDateTo = disposal.SuspensionDateTo,
                    SendToErp = string.IsNullOrEmpty(disposal.ErpId) ? YesNo.No : YesNo.Yes,
                    ErpId = disposal.ErpId,
                    ErpRegistrationNumber = disposal.ErpRegistrationNumber,
                    ErpRegistrationDate = disposal.ErpRegistrationDate,
                    TypeBase = (disposal.Inspection as BaseStatement)?.RequestType == BaseStatementRequestType.MotivationConclusion
                        ? TypeBase.MotivationConclusion
                        : disposal.Inspection.TypeBase,
                    InspectionId = disposal.Inspection?.Id ?? 0,
                    HasChildrenActCheck = serviceDocumentChildren.GetAll()
                        .Any(y => y.Parent.Id == disposal.Id && y.Children.TypeDocumentGji == TypeDocumentGji.ActCheck),
                    TimeVisitStart = disposal.TimeVisitStart.HasValue ? disposal.TimeVisitStart.Value.ToString("HH:mm") : string.Empty,
                    TimeVisitEnd = disposal.TimeVisitEnd.HasValue ? disposal.TimeVisitEnd.Value.ToString("HH:mm") : string.Empty,
                    DocumentTime = disposal.DocumentTime.HasValue ? disposal.DocumentTime.Value.ToString("HH:mm") : string.Empty,
                    SuspensionTimeFrom = disposal.SuspensionTimeFrom.HasValue ? disposal.SuspensionTimeFrom.Value.ToString("HH:mm") : string.Empty,
                    SuspensionTimeTo = disposal.SuspensionTimeTo.HasValue ? disposal.SuspensionTimeTo.Value.ToString("HH:mm") : string.Empty
                };
            }
        }

        protected class TatarstanDisposalDto
        {
            /// <summary>
            /// Идентификатор
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Тип распоряжения
            /// </summary>
            public TypeDisposalGji TypeDisposal { get; set; }

            /// <summary>
            /// Дата начала обследования
            /// </summary>
            public DateTime? DateStart { get; set; }

            /// <summary>
            /// Дата окончания обследования
            /// </summary>
            public DateTime? DateEnd { get; set; }

            /// <summary>
            /// Согласование с прокуротурой
            /// </summary>
            public TypeAgreementProsecutor TypeAgreementProsecutor { get; set; }

            /// <summary>
            /// Номер документа с результатом согласования
            /// </summary>
            public string DocumentNumberWithResultAgreement { get; set; }

            /// <summary>
            /// Результат согласования
            /// </summary>
            public TypeAgreementResult TypeAgreementResult { get; set; }

            /// <summary>
            /// Дата документа с результатом согласования
            /// </summary>
            public DateTime? DocumentDateWithResultAgreement { get; set; }

            /// <summary>
            /// Должностное лицо (ДЛ) вынесшее распоряжение
            /// </summary>
            public Inspector IssuedDisposal { get; set; }

            /// <summary>
            /// Ответственный за исполнение
            /// </summary>
            public Inspector ResponsibleExecution { get; set; }

            /// <summary>
            /// Вид проверки
            /// </summary>
            public KindCheckGji KindCheck { get; set; }

            /// <summary>
            /// Описание
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Выезд на объект с
            /// </summary>
            public DateTime? ObjectVisitStart { get; set; }

            /// <summary>
            /// Выезд на объект по
            /// </summary>
            public DateTime? ObjectVisitEnd { get; set; }

            /// <summary>
            /// Выезд инспектора в командировку
            /// </summary>
            public bool OutInspector { get; set; }

            /// <summary>
            /// Номер документа (Уведомление о проверке)
            /// </summary>
            public string NcNum { get; set; }

            /// <summary>
            /// Дата документа (Уведомление о проверке)
            /// </summary>
            public DateTime? NcDate { get; set; }

            /// <summary>
            /// Номер исходящего письма  (Уведомление о проверке)
            /// </summary>
            public string NcNumLatter { get; set; }

            /// <summary>
            /// Дата исходящего пиьма  (Уведомление о проверке)
            /// </summary>
            public DateTime? NcDateLatter { get; set; }

            /// <summary>
            /// Уведомление получено (Уведомление о проверке)
            /// </summary>
            public YesNo NcObtained { get; set; }

            /// <summary>
            /// Уведомление отправлено (Уведомление о проверке)
            /// </summary>
            public YesNo NcSent { get; set; }
            
            /// <summary>
            /// Учетный номер проверки в ЕРП
            /// </summary>
            public string ErpRegistrationNumber { get; set; }

            /// <summary>
            /// Идентификатор ЕРП
            /// </summary>
            public string ErpId { get; set; }

            /// <summary>
            /// Дата присвоения учетного номера / идентификатора ЕРП
            /// </summary>
            public DateTime? ErpRegistrationDate { get; set; }

            /// <summary>
            /// Срок проверки (количество дней)
            /// </summary>
            public int? CountDays { get; set; }

            /// <summary>
            /// Срок проверки (количество часов)
            /// </summary>
            public int? CountHours { get; set; }

            /// <summary>
            /// Наименование прокуратуры
            /// </summary>
            public ProsecutorOfficeDict Prosecutor { get; set; }

            /// <summary>
            /// Основание для включения проверки в ЕРП
            /// </summary>
            public ReasonErpChecking? ReasonErpChecking { get; set; }

            /// <summary>
            /// Основание проверки
            /// </summary>
            public InspectionBaseType InspectionBase { get; set; }

            /// <summary>
            /// Способ уведомления
            /// </summary>
            public NotificationType? NotificationType { get; set; }

            /// <summary>
            /// Вид контроля
            /// </summary>
            public ControlType ControlType { get; set; }

            /// <summary>
            /// Срок взаимодействия с контролируемым лицом не более, часов
            /// </summary>
            public int? InteractionPersonHour { get; set; }

            /// <summary>
            /// Срок взаимодействия с контролируемым лицом не более, минут
            /// </summary>
            public int? InteractionPersonMinutes { get; set; }

            /// <summary>
            /// Основание для приостановления проведения проверки
            /// </summary>
            public string SuspensionInspectionBase { get; set; }

            /// <summary>
            /// Дата начала периода приостановления проведения проверки
            /// </summary>
            public DateTime? SuspensionDateFrom { get; set; }

            /// <summary>
            /// Дата окончания периода приостановления проведения проверки
            /// </summary>
            public DateTime? SuspensionDateTo { get; set; }

            /// <summary>
            /// Тип документа ГЖИ
            /// </summary>
            public TypeDocumentGji TypeDocumentGji { get; set; }

            /// <summary>
            /// Дата документа
            /// </summary>
            public DateTime? DocumentDate { get; set; }

            /// <summary>
            /// Номер документа
            /// </summary>
            public string DocumentNumber { get; set; }

            /// <summary>
            /// Номер документа (Целая часть)
            /// </summary>
            public int? DocumentNum { get; set; }

            /// <summary>
            /// Дополнительный номер документа (порядковый номер если документов одного типа несколько)
            /// </summary>
            public int? DocumentSubNum { get; set; }

            /// <summary>
            /// Буквенный подномер
            /// </summary>
            public string LiteralNum { get; set; }

            /// <summary>
            /// Год документа
            /// </summary>
            public int? DocumentYear { get; set; }

            /// <summary>
            /// Статус
            /// </summary>
            public State State { get; set; }

            /// <summary>
            /// Идентификатор для интеграции
            /// </summary>
            public Guid? TorId { get; set; }
            
            public TypeBase TypeBase { get; set; }
            
            public long InspectionId { get; set; }

            public bool HasChildrenActCheck { get; set; }

            public YesNo SendToErp { get; set; }
            
            /// <summary>
            /// Время начала визита (Время с)
            /// </summary>
            public string TimeVisitStart { get; set; }

            /// <summary>
            /// Время окончания визита (Время по)
            /// </summary>
            public string TimeVisitEnd { get; set; }
            
            /// <summary>
            /// Дата документа
            /// </summary>
            public string DocumentTime { get; set; }
            
            /// <summary>
            /// Время начала периода приостановления проведения проверки
            /// </summary>
            public string SuspensionTimeFrom { get; set; }

            /// <summary>
            /// Время окончания периода приостановления проведения проверки
            /// </summary>
            public string SuspensionTimeTo { get; set; }

            /// <summary>
            /// Сведения о причинении вреда (ущерба) (ст. 66 ФЗ)
            /// </summary>
            public virtual string InformationAboutHarm { get; set; }
        }
    }
}