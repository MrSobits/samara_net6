namespace Bars.GkhGji.Regions.Samara.StateChange
{
    using System;
    using System.Globalization;
    using System.Linq;

    using B4;
    using B4.Modules.States;
    using B4.Utils;
    using Gkh.Entities;
    using Entities;
    using Enums;
    using Castle.Windsor;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Формирование номера в соответствии с правилами Самара
    /// </summary>
    public abstract class BaseDocValidationNumberSamaraRule : IRuleChangeStatus
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
                    var parDocs =
                        Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
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
                    var mainDoc = Container.Resolve<IDomainService<DocumentGji>>().GetAll()
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
                    var childrenDocumentCount = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
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
                else if (document is ActRemoval)
                {

                }

                //если номер уже присвоен, то ничего не делаем, чтобы не проставлялся другой номер
                if (string.IsNullOrEmpty(document.DocumentNumber))
                {
                    // Если дошли досюда то производим действие
                    Action(document);
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
            // путем получения первого свободного номера если не нашли то максимальный номер + 1
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

            var documentService = Container.Resolve<IDomainService<DocumentGji>>();
            var documentGjiChildrenService = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            // Год берется из даты документа
            document.DocumentYear = document.DocumentDate.HasValue ? document.DocumentDate.Value.Year : (int?)null;

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

                //получаем номера
                var documentNum = documentService.GetAll()
                    .Where(x => x.Id != document.Id)
                    .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal
                                || x.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor
                                || (x.TypeDocumentGji == TypeDocumentGji.Resolution && resolutionsByProtocolMvdQuery.Contains(x.Id)))
                     .Where(x => x.DocumentNum > 0)
                     .Select(x => x.DocumentNum)
                     .OrderBy(x => x)
                     .Distinct()
                     .ToArray();


                int? nextFreetNum = 0;
                // получаем минимальный свободный номер если не нашли то берем максимальный + 1
                for (int i = 0; i < documentNum.Length; i++)
                {

                    if (i != documentNum.Length - 1)
                    {
                        int? value1 = documentNum[i] + 1;
                        int? value2 = documentNum[i + 1];
                        if (value1 != value2)
                        {
                            nextFreetNum = documentNum[i] + 1;
                            break;
                        }
                    }
                    else
                    {
                        nextFreetNum = documentNum[i] + 1;
                    }
                }

                document.DocumentNum = nextFreetNum.HasValue ? nextFreetNum.Value : 1;
            }
            else
            {
                if (document.Inspection.TypeBase != TypeBase.ProsecutorsResolution)
                {
                    //получаем родительский стейдж
                    if (document.Stage.Parent != null)
                    {
                        var disp = documentService.GetAll().FirstOrDefault(x => x.Stage.Id == document.Stage.Parent.Id);
                        document.DocumentNum = disp != null ? disp.DocumentNum : null;
                    }
                }
                else
                {
                    var resolPros = Utils.Utils.GetParentDocumentByType(documentGjiChildrenService, document, TypeDocumentGji.ResolutionProsecutor);
                    document.DocumentNum = resolPros != null ? resolPros.DocumentNum : null;
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

                    document.DocumentSubNum = document.DocumentSubNum.HasValue ? document.DocumentSubNum.Value + 1 : 0;
                }
            }

            var muService = Container.Resolve<IDomainService<Municipality>>();

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
                    var mainDispId =
                        Container.Resolve<IDomainService<Disposal>>().GetAll()
                            .Where(x => x.Inspection.Id == document.Inspection.Id
                                        && x.TypeDisposal == TypeDisposalGji.Base)
                            .Select(x => x.Id)
                            .FirstOrDefault();

                    if (mainDispId > 0)
                    {
                        var mainDisp = Container.Resolve<IDomainService<ViewDisposal>>().Load(mainDispId);

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

                if (document.DocumentSubNum.ToLong() > 0)
                {
                    document.DocumentNumber += "/" + document.DocumentSubNum.ToLong().ToString(CultureInfo.InvariantCulture);
                }
            }
        }
    }

}
