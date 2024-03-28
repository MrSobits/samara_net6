namespace Bars.GkhGji.StateChange
{
    using System;
    using System.Globalization;
    using System.Linq;
    using B4;
    using B4.Modules.States;
    using B4.Utils;

    using Bars.GkhGji.Contracts.Reminder;

    using Castle.Core.Internal;

    using Gkh.Entities;
    using Entities;
    using Enums;

    using Castle.Windsor;

    /// <summary>
    /// Формирование номера в соответствии с правилами Перми
    /// </summary>
    public abstract class BaseDocValidationNumberPermRule : IRuleChangeStatus
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public abstract string Id { get; }

        /// <summary>
        /// Название
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Тип док-та
        /// </summary>
        public abstract string TypeId { get; }

        /// <summary>
        /// Описание
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Валидация
        /// </summary>
        /// <param name="statefulEntity">Сущность со статусами</param>
        /// <param name="oldState">Предыдущий статус</param>
        /// <param name="newState">Новый статус</param>
        /// <returns>Результат валидации</returns>
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var result = new ValidateResult();

            if (statefulEntity is DocumentGji)
            {
                var document = statefulEntity as DocumentGji;

                if (document.DocumentDate == null)
                {
                    result.Message = "Невозможно сформировать номер, поскольку дата документа не указана";
                    result.Success = false;
                    return result;
                }

#warning перенести проверку в правило для каждого документа отдельно
                if (document.TypeDocumentGji != TypeDocumentGji.ActRemoval)
                {
                    /* проставление номера возможно только если у всех родительских документов есть строковый номер */
                    var parDocs = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                            .Where(x => x.Children.Id == document.Id)
                            .Select(x => new { x.Parent.TypeDocumentGji, x.Parent.DocumentDate, x.Parent.DocumentNumber })
                            .ToList();

                    foreach (var doc in parDocs)
                    {
                        if (string.IsNullOrEmpty(doc.DocumentNumber))
                        {
                            result.Message = string.Format(
                                "Номер не может быть присвоен, потому что у предыдущего документа {0} от {1} нет номера",
                                doc.TypeDocumentGji.GetEnumMeta().Display,
                                doc.DocumentDate.ToDateTime().ToShortDateString());
                            result.Success = false;
                            return result;
                        }
                    }
                }

                /*проверка что у родительского документа по Stage был строковый номер*/
                if (document.Stage.Parent != null)
                {
                    var mainDoc = this.Container.Resolve<IDomainService<DocumentGji>>().GetAll()
                        .Where(x => x.Stage.Id == document.Stage.Parent.Id)
                        .Select(x => new { x.TypeDocumentGji, x.DocumentDate, x.DocumentNumber })
                        .ToList();

                    foreach (var doc in mainDoc)
                    {
                        if (string.IsNullOrEmpty(doc.DocumentNumber))
                        {
                            result.Message = string.Format(
                                "Номер не может быть присвоен, потому что у предыдущего документа {0} от {1} нет номера",
                                doc.TypeDocumentGji.GetEnumMeta().Display,
                                doc.DocumentDate.ToDateTime().ToShortDateString());
                            result.Success = false;
                            return result;
                        }
                    }
                }

                /* Если регистрируется акт проверки с типом - Акт проверки документа ГЖИ
                 * то дополнительно необходимо чтобы были зарегистрированы дочерние акты проверки предписаний 
                 */
                if ((document is ActCheck) && (document as ActCheck).TypeActCheck == TypeActCheckGji.ActCheckDocumentGji)
                {
                    var childrenDocumentCount = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                        .Where(x => x.Parent.Id == document.Id && x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                        .Select(x => new { x.Children.DocumentNumber })
                        .ToList();

                    if (childrenDocumentCount.Any(doc => string.IsNullOrEmpty(doc.DocumentNumber)))
                    {
                        result.Message = string.Format("Необходимо присвоить номера актам проверки предписаний");
                        result.Success = false;
                        return result;
                    }
                }
                else if(document is ActRemoval)
                {

                }

                //если номер уже присвоен, то ничего не делаем, чтобы не проставлялся другой номер
                if (string.IsNullOrEmpty(document.DocumentNumber))
                {
                    // Если дошли досюда то производим действие
                    this.Action(document);
                }

            }

            result.Success = true;

            return result;
        }

        /// <summary>
        /// Формируем номер
        /// </summary>
        /// <param name="document">Базовый документ ГЖИ</param>
        protected virtual void Action(DocumentGji document)
        {
            // Формируем номер
            // Номер формируется для:
            // распоряжений, распоряжений вне инспекторской деятельности, постановлений прокуратуры
            // путем получения максимального номера за текущий год + 1
            // для распоряжений в "Проверки соискателей лицензии" в зависимости от значения из поля "Тип проверки"(GKH-14900)
            // для других документов номер берется относительно Главных документов
            // либо документов Распоряжения, либо документов Постановления прокуратуры
            // строковый номер формируется следующим образом:
            // если распоряжение вне инспекторской деятельности то просто Номер
            // для распоряжений и постановлений прокуратуры ЛитераРайона-Номер,
            // для остальных документов ЛитераРайона-Номер/ДополнительныйНомер
            // Дополнительный номер формируется внутри Этапа, то есть внутри документов этапа проверки берется
            // максимальный дополнительный номер + 1
            // Если дополнительный номер = 0, то он не входит в строковый номер и дробь не ставится
            // для остальных документов копируем из главного распоряжения проверки или постановления прокуратуры
            // литера района берется из поля код в справочнике муниципальных образований

            var documentService = this.Container.Resolve<IDomainService<DocumentGji>>();
            var documentGjiChildrenService = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var baseLicenseApplicantsService = this.Container.Resolve<IDomainService<BaseLicenseApplicants>>();
            var muService = this.Container.Resolve<IDomainService<Municipality>>();
            var disposalService = this.Container.Resolve<IDomainService<Disposal>>();
            var viewDisposal = this.Container.Resolve<IDomainService<ViewDisposal>>();

            try
            {
                // Год берется из даты документа
                document.DocumentYear = document.DocumentDate?.Year;

                if (document.TypeDocumentGji == TypeDocumentGji.Disposal
                    || document.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                {
                    if (document.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                    {
                        document.DocumentDate = DateTime.Now.Date;
                    }

                    // т.к. для постановлений сформированных из протокола МВД должна быть сквозная нумерация через все распоряжения и постановления прокуратуры с начала года
                    var resolutionsByProtocolMvdQuery = documentGjiChildrenService.GetAll()
                        .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                        .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ProtocolMvd)
                        .Select(x => x.Children.Id);

                    var maxNum = documentService.GetAll()
                        .Where(x => x.DocumentYear == document.DocumentYear)
                        .Where(x => x.Id != document.Id)
                        .Where(
                            x => x.TypeDocumentGji == TypeDocumentGji.Disposal
                                || x.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor
                                || (x.TypeDocumentGji == TypeDocumentGji.Resolution && resolutionsByProtocolMvdQuery.Contains(x.Id)))
                        .Select(x => x.DocumentNum)
                        .Max();

                    document.DocumentNum = maxNum + 1 ?? 1;
                }
                else
                {
                    if (document.Inspection.TypeBase != TypeBase.ProsecutorsResolution)
                    {
                        //получаем родительский стейдж
                        if (document.Stage.Parent != null)
                        {
                            var disp = documentService.GetAll().FirstOrDefault(x => x.Stage.Id == document.Stage.Parent.Id);
                            document.DocumentNum = disp?.DocumentNum;
                            document.LiteralNum = disp?.LiteralNum;
                        }
                    }
                    else
                    {
                        var resolPros = Utils.Utils.GetParentDocumentByType(documentGjiChildrenService, document, TypeDocumentGji.ResolutionProsecutor);
                        document.DocumentNum = resolPros?.DocumentNum;
                        document.LiteralNum = resolPros?.LiteralNum;
                    }

                    // теперь формируем дополнительный номер среди документов Этапа в котором находится данный документ
                    // Это необходимо для того чтобы была нумерация внутри группы документов этапа
                    if (!documentService.GetAll().Any(x => x.Stage.Id == document.Stage.Id && x.Id != document.Id))
                    {
                        // Если в этапе нет ни одного документа кроме текущего
                        document.DocumentSubNum = 0;
                    }
                    else
                    {
                        document.DocumentSubNum = documentService.GetAll()
                            .Where(x => x.Stage.Id == document.Stage.Id && x.Id != document.Id)
                            .Select(x => x.DocumentSubNum).Max();

                        document.DocumentSubNum = document.DocumentSubNum + 1 ?? 0;
                    }
                }

                var muCode = "";

                if (document.Inspection != null)
                {
                    if (document.Inspection.TypeBase == TypeBase.ProsecutorsResolution)
                    {
                        var resolPros = document as ResolPros;
                        if (resolPros != null)
                        {
                            muCode = resolPros.ReturnSafe(x => x.Municipality.Code);
                        }
                    }
                    else
                    {
                        var mainDispId = disposalService.GetAll()
                                .Where(
                                    x => x.Inspection.Id == document.Inspection.Id
                                        && x.TypeDisposal == TypeDisposalGji.Base)
                                .Select(x => x.Id)
                                .FirstOrDefault();

                        if (mainDispId > 0)
                        {
                            var mainDisp = viewDisposal.Load(mainDispId);

                            if (mainDisp != null)
                            {
                                var mu = muService.Load(mainDisp.MunicipalityId);

                                if (mu != null)
                                {
                                    muCode = mu.Code;
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(muCode))
                    {
                        document.DocumentNumber = muCode + "-" + document.DocumentNum.ToLong().ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        document.DocumentNumber = document.DocumentNum.ToLong().ToString(CultureInfo.InvariantCulture);
                    }

                    if (!document.LiteralNum.IsNullOrEmpty())
                    {
                        document.DocumentNumber += document.LiteralNum.Trim();
                    }

                    if (document.DocumentSubNum.ToLong() > 0)
                    {
                        document.DocumentNumber += "/" + document.DocumentSubNum.ToLong().ToString(CultureInfo.InvariantCulture);
                    }
                }

                // Для распоряжений в "Проверки соискателей лицензии" в зависимости от значения из поля "Тип проверки"(GKH-14900)
                if (document.TypeDocumentGji == TypeDocumentGji.Disposal
                    || document.TypeDocumentGji == TypeDocumentGji.ActCheck)
                {
                    var inspectionType = baseLicenseApplicantsService.GetAll()
                        .Where(x => x.Id == document.Inspection.Id)
                        .Select(x => x.InspectionType)
                        .FirstOrDefault();
                    if (inspectionType == InspectionGjiType.MatchInformation)
                    {
                        document.DocumentNumber += "д";
                    }
                }
            }
            finally
            {
                this.Container.Release(documentService);
                this.Container.Release(documentGjiChildrenService);
                this.Container.Release(baseLicenseApplicantsService);
                this.Container.Release(muService);
                this.Container.Release(disposalService);
                this.Container.Release(viewDisposal);
            }
        }
    }
}