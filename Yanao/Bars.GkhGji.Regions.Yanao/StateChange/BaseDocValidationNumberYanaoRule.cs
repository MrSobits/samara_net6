using System;
using System.Globalization;
using System.Linq;
using Bars.B4;
using Bars.B4.Modules.States;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Castle.Windsor;

namespace Bars.GkhGji.StateChange
{
    public abstract class BaseDocValidationNumberYanaoRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public abstract string Id { get; }

        public abstract string Name { get; }

        public abstract string TypeId { get; }

        public abstract string Description { get; }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State currentState, State newState)
        {
            var documentGjiChildrenDomainService = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            try
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
                        var parentDocument = documentGjiChildrenDomainService.GetAll()
                            .Where(x => x.Children.Id == document.Id && !x.Parent.State.FinalState)
                            .Select(x => new {x.Parent.TypeDocumentGji, x.Parent.DocumentNumber, x.Parent.DocumentDate})
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

                        var childrenDocumentCount = documentGjiChildrenDomainService.GetAll()
                            .Count(
                                x => x.Parent.Id == document.Id && !x.Children.State.FinalState && x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval);

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
                        var parentDocument = documentGjiChildrenDomainService.GetAll()
                            .Where(x => x.Children.Id == document.Id && !x.Parent.State.FinalState && x.Parent.TypeDocumentGji != TypeDocumentGji.ActCheck)
                            .Select(x => new {x.Parent.TypeDocumentGji, x.Parent.DocumentNumber, x.Parent.DocumentDate})
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
                        var parentDocument = documentGjiChildrenDomainService.GetAll()
                            .Where(x => x.Children.Id == document.Id && !x.Parent.State.FinalState)
                            .Select(x => new {x.Parent.TypeDocumentGji, x.Parent.DocumentNumber, x.Parent.DocumentDate})
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
                    this.Action(document);
                }

                result.Success = true;

                return result;
            }
            finally
            {
                this.Container.Release(documentGjiChildrenDomainService);
            }
        }

        private void Action(DocumentGji document)
        {
            // Формируем номер

            // Номер формируется для:
            // распоряжений, распоряжений вне инспекторской деятельности, протоколов, постановлений, представлений, постановлений прокуратуры
            // путем получения максимального номера за текущий год + 1
            // для других документов номер берется относительно Главных документов
            // либо документов Распоряжения, либо документов Постановления прокуратуры
            // Дополнительный номер формируется внутри Этапа, тоесть внтури документов этапа проверки берется
            // максимальный дополнительный номер + 1

            var documentService = this.Container.Resolve<IDomainService<DocumentGji>>();

            try
            {
                // Год берется из даты документа
                document.DocumentYear = document.DocumentDate.HasValue ? document.DocumentDate.Value.Year : (int?) null;
                int? parentYear = null;

                if (document.TypeDocumentGji == TypeDocumentGji.Disposal)
                {
                    document.DocumentYear = document.DocumentDate.HasValue ? document.DocumentDate.Value.Year : (int?) null;

                    var maxNum = documentService.GetAll()
                        .Where(
                            x =>
                                x.DocumentYear == document.DocumentYear
                                    && x.Id != document.Id
                                    && (x.TypeDocumentGji == TypeDocumentGji.Disposal))
                        .Select(x => x.DocumentNum)
                        .Max();

                    document.DocumentNum = maxNum.HasValue ? maxNum.Value + 1 : 1;
                    document.DocumentDate = DateTime.Now.Date;
                }
                else if (document.TypeDocumentGji == TypeDocumentGji.Protocol
                    || document.TypeDocumentGji == TypeDocumentGji.Resolution
                    || document.TypeDocumentGji == TypeDocumentGji.Presentation
                    || document.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                {
                    var maxNum = documentService.GetAll()
                        .Where(
                            x => x.DocumentYear == document.DocumentYear
                                && x.Id != document.Id
                                && x.TypeDocumentGji == document.TypeDocumentGji)
                        .Select(x => x.DocumentNum)
                        .Max();

                    document.DocumentNum = maxNum.HasValue ? maxNum.Value + 1 : 1;
                }

                // Тут мы не генерируем номера а получаем номера из главных документов Проверки
                // Главные документы проверки это либо Распоряжение либо Постановление прокуратуры
                if (document.TypeDocumentGji == TypeDocumentGji.ActCheck
                    || document.TypeDocumentGji == TypeDocumentGji.ActSurvey
                    || document.TypeDocumentGji == TypeDocumentGji.ActRemoval
                    || document.TypeDocumentGji == TypeDocumentGji.Prescription)
                {
                    var findStage = document.Stage;
                    if (document.Stage.Parent != null)
                    {
                        findStage = document.Stage.Parent;
                    }

                    // Теперь получаем документ корневого этапа
                    var parent = documentService
                        .GetAll()
                        .FirstOrDefault(x => x.Stage.Id == findStage.Id);

                    document.DocumentNum = parent?.DocumentNum;
                    parentYear = parent?.DocumentYear;

                    //для акта проверки номер проставляется равный номеру распоряжения, дополнительный номер не нужен
                    if (!(document is ActCheck) || (document as ActCheck).TypeActCheck == TypeActCheckGji.ActCheckIndividual)
                    {
                        // теперь формируем дополнительный номер среди документов Этапа в котором находится данный документ
                        // Это необходимо для того чтобы была нумерация внутри группы документов этапа
                        if (!documentService.GetAll().Any(x => x.Stage.Id == document.Stage.Id && x.Id != document.Id))
                        {
                            // Если в этапе нет ни одного документа, кроме текущего
                            document.DocumentSubNum = 1;
                        }
                        else
                        {
                            document.DocumentSubNum =
                                documentService.GetAll()
                                    .Where(
                                        x => x.Stage.Id == document.Stage.Id
                                            && x.Id != document.Id)
                                    .Select(x => x.DocumentSubNum).Max();

                            document.DocumentSubNum = document.DocumentSubNum.ToInt() + 1;
                        }
                    }
                }

                // Строковый номер итоговый формируется как "Номер/ДополнительныйНомер/пр2017", где 2017 - год родительского документа
                // Если дополнительный номер = 0, то он не входит в стрковый номер и дробь не ставится
                document.DocumentNumber = document.DocumentNum.ToInt().ToString(CultureInfo.InvariantCulture);
                if (document.DocumentSubNum.ToInt() > 0)
                {
                    document.DocumentNumber += $"/{document.DocumentSubNum.ToInt().ToString(CultureInfo.InvariantCulture)}" +
                        $"/{(parentYear != null ? "пр" + parentYear : string.Empty)}";
                }
            }
            finally
            {
                this.Container.Release(documentService);
            }
        }
    }
}