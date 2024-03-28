using System;
using System.Globalization;
using System.Linq;
using Bars.B4;
using Bars.B4.Modules.States;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Castle.Windsor;

namespace Bars.GkhGji.StateChange
{
    public class DocGjiValidationNumberTatRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public string Id { get { return "gji_tatar_document_validation_number_rule"; } }

        public string Name { get { return "Проверка возможности формирования номера документа ГЖИ РТ"; } }

        public string TypeId { get { return "gji_document"; } }

        public string Description { get { return "Данное правило проверяет формирование номера в соответствии с правилами РТ"; } }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var result = new ValidateResult();

            if (statefulEntity is DocumentGji)
            {
                var document = statefulEntity as DocumentGji;

                // Номер можно формировать, только если заполнена дата 
                if (document.DocumentDate == null)
                {
                    result.Message = "Невозможно сформировать номер, поскольку дата документа не указана";
                    result.Success = false;
                    return result;
                }

                /*Если акт проверки предписания регистрируется,
                 *     то необходимо чтоб было зарегистрировано ег ородительское предписание
                 *Если регистрируется акт проверки с типом - Акт проверки документа ГЖИ
                 *     то необходимо чтобы были зарегистрированы и родительские документы и дочерние акты проверки предписаний
                 *Остальные документы регистрируются только если зарегистрированы родительские документы
                 */

                if ((document is ActCheck) && (document as ActCheck).TypeActCheck == TypeActCheckGji.ActCheckDocumentGji)
                {
                    // у акта проверки с типом Акт проверки документа ГЖИ должны быть зарегистрированы дочерние акты проверки
                    // а также все родительские документы
                    var parentDocument = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                        .Where(x => x.Children.Id == document.Id && !x.Parent.State.FinalState)
                        .Select(x => new { x.Parent.TypeDocumentGji, x.Parent.DocumentNumber, x.Parent.DocumentDate })
                        .FirstOrDefault();

                    if (parentDocument != null)
                    {
                        result.Message = string.Format(
                            "Необходимо зарегистрировать родительский документ {0} от {1}",
                            parentDocument.TypeDocumentGji.GetEnumMeta().Display,
                            parentDocument.DocumentDate.ToDateTime().ToShortDateString());
                        result.Success = false;
                        return result;
                    }

                    var childrenDocumentCount = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                        .Count(x => x.Parent.Id == document.Id && !x.Children.State.FinalState && x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval);

                    if (childrenDocumentCount > 0)
                    {
                        result.Message = string.Format("Необходимо зарегистрировать акты проверки предписаний");
                        result.Success = false;
                        return result;
                    }
                }
                else if (document is ActRemoval)
                {
                    // у акта проверки предписаний ActRemovalGJI необходимо чтобы были зарегистрированы все родительские документы кроме акта проверки ActCheckGJI
                    var parentDocument = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                        .Where(x => x.Children.Id == document.Id && !x.Parent.State.FinalState && x.Parent.TypeDocumentGji != TypeDocumentGji.ActCheck)
                        .Select(x => new { x.Parent.TypeDocumentGji, x.Parent.DocumentNumber, x.Parent.DocumentDate })
                        .FirstOrDefault();

                    if (parentDocument != null)
                    {
                        result.Message = string.Format(
                            "Необходимо зарегистрировать родительский документ {0} от {1}",
                            parentDocument.TypeDocumentGji.GetEnumMeta().Display,
                            parentDocument.DocumentDate.ToDateTime().ToShortDateString());
                        result.Success = false;
                        return result;
                    }
                }
                else
                {
                    // проверяем если родительские документы незарегистрированы, то нельзя регистрировать
                    var parentDocument = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                        .Where(x => x.Children.Id == document.Id && !x.Parent.State.FinalState)
                        .Select(x => new { x.Parent.TypeDocumentGji, x.Parent.DocumentNumber, x.Parent.DocumentDate })
                        .FirstOrDefault();

                    if (parentDocument != null)
                    {
                        result.Message = string.Format(
                            "Необходимо зарегистрировать родительский документ {0} от {1}",
                            parentDocument.TypeDocumentGji.GetEnumMeta().Display,
                            parentDocument.DocumentDate.ToDateTime().ToShortDateString());
                        result.Success = false;
                        return result;
                    }
                }

                // Если дошли досуда то производим действие
                Action(document);
            }

            result.Success = true;

            return result;
        }

        private void Action(DocumentGji document)
        {
            // Формируем номер
            // Номер формируется для:
            // распоряжений, распоряжений вне инспекторской деятельности, постановлений прокуратуры
            // путем получения максимального номера за текущий год + 1
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

            // Год берется из даты документа
            document.DocumentYear = document.DocumentDate.HasValue ? document.DocumentDate.Value.Year : (int?)null;

            var documentService = Container.Resolve<IDomainService<DocumentGji>>();

            if (document.TypeDocumentGji == TypeDocumentGji.Disposal || document.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
            {
                document.DocumentDate = DateTime.Now.Date;

                var maxNum = documentService.GetAll()
                    .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal
                                || x.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                    .Where(x => x.DocumentYear == document.DocumentYear && x.Id != document.Id)
                    .Select(x => x.DocumentNum)
                    .Max();

                document.DocumentNum = maxNum.HasValue ? maxNum.Value + 1 : 1;
            }
            else
            // Тут мы не генерируем номера а получаем номера из главных документов Проверки
            // Главные документы проверки это либо Распоряжение либо Постановление прокуратуры
            if (document.TypeDocumentGji != TypeDocumentGji.Disposal && document.TypeDocumentGji != TypeDocumentGji.ResolutionProsecutor)
            {
                var findStage = document.Stage;
                if (document.Stage.Parent != null)
                {
                    findStage = document.Stage.Parent;
                }

                // Теперь получаем документ корневого этапа
                document.DocumentNum = documentService.GetAll()
                    .Where(x => x.Stage.Id == findStage.Id)
                    .Select(x => x.DocumentNum)
                    .FirstOrDefault();

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
                        .Select(x => x.DocumentSubNum)
                        .Max();

                    document.DocumentSubNum = document.DocumentSubNum.HasValue ? document.DocumentSubNum.Value + 1 : 1;
                }
            }

            var muService = Container.Resolve<IDomainService<Municipality>>();

            var muCode = "";

            switch (document.TypeDocumentGji)
            {
                case TypeDocumentGji.Disposal:
                    {
                        var d = Container.Resolve<IDomainService<Disposal>>().Load(document.Id);

                        if (d.TypeDisposal != TypeDisposalGji.NullInspection)
                        {
                            var service = Container.Resolve<IDomainService<ViewDisposal>>();
                            var disp = service.Load(document.Id);

                            if (disp.MunicipalityId.HasValue)
                            {
                                muCode = muService.Load(disp.MunicipalityId).Code;
                            }
                        }

                        if (!string.IsNullOrEmpty(muCode))
                        {
                            document.DocumentNumber =
                                muCode
                                + "-"
                                + document.DocumentNum.ToInt().ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            document.DocumentNumber = document.DocumentNum.ToInt().ToString(CultureInfo.InvariantCulture);
                        }
                    }
                    break;
                case TypeDocumentGji.ResolutionProsecutor:
                    {
                        var service = Container.Resolve<IDomainService<ResolPros>>();
                        var resolpros = service.Get(document.Id);

                        var municipality = resolpros.ReturnSafe(x => x.Contragent.Municipality);

                        if (municipality != null)
                        {
                            muCode = municipality.Code;
                        }
                        else
                        {
                            muCode = Container.Resolve<IDomainService<ResolProsRealityObject>>().GetAll()
                                .Where(x => x.ResolPros.Id == document.Id)
                                .Select(x => x.RealityObject.Municipality.Code)
                                .FirstOrDefault();
                        }

                        if (!string.IsNullOrEmpty(muCode))
                        {
                            document.DocumentNumber =
                                muCode 
                                + "-"
                                + document.DocumentNum.ToInt().ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            document.DocumentNumber = document.DocumentNum.ToLong()
                                                              .ToString(CultureInfo.InvariantCulture);
                        }
                    }
                    break;
                default:
                    {
                        if (document.Inspection.TypeBase != TypeBase.ProsecutorsResolution)
                        {
                            var mainDisposalDocNumber =
                                Container.Resolve<IDomainService<Disposal>>().GetAll()
                                    .Where(x => x.TypeDisposal == TypeDisposalGji.Base)
                                    .Where(x => x.Inspection.Id == document.Inspection.Id)
                                    .Select(x => x.DocumentNumber)
                                    .FirstOrDefault();

                            document.DocumentNumber = mainDisposalDocNumber ?? string.Empty;
                        }
                        else
                        {
                            var resolProsDocNumber =
                                Container.Resolve<IDomainService<ResolPros>>().GetAll()
                                    .Where(x => x.Inspection.Id == document.Inspection.Id)
                                    .Select(x => x.DocumentNumber)
                                    .FirstOrDefault();

                            document.DocumentNumber = resolProsDocNumber ?? string.Empty;
                        }
                    }
                    break;
            }

            if (document.DocumentSubNum.ToLong() > 0)
            {
                document.DocumentNumber += "/" + document.DocumentSubNum.ToLong().ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}