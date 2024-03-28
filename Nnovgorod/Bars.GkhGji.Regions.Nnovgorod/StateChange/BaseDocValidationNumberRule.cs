using System.Collections.Generic;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Regions.Nnovgorod.StateChange
{
    using System;
    using System.Globalization;
    using System.Linq;
    using B4;
    using B4.Modules.States;
    using B4.Utils;

    using Gkh.Entities;
    using Enums;

    using Castle.Windsor;

    public abstract class BaseDocValidationNumberRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public abstract string Id { get; }

        public abstract string Name { get; }

        public abstract string TypeId { get; }

        public abstract string Description { get; }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var result = new ValidateResult();

            if (statefulEntity is DocumentGji)
            {
                var document = statefulEntity as DocumentGji;

                if (!document.DocumentDate.HasValue)
                {
                    result.Message = "Невозможно сформировать номер, поскольку дата документа не указана";
                    result.Success = false;
                    return result;
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

        protected virtual void Action(DocumentGji document)
        {
            
        }

        protected string GetNumber(IEnumerable<string> numbers, string parentDocumentNumber)
        {
            var result = string.Empty;

            // ищем максимальный номер
            var maxNumber = 0;
            foreach (var item in numbers)
            {
                if (item.IsEmpty())
                {
                    continue;
                }

                var elements = item.Split('/');
                if (elements.Length == 2)
                {
                    // Т.е номер вида 515-00-00/2013
                    maxNumber++;
                    result = string.Format("{0}/{1}/{2}", elements[0], maxNumber, elements[1]);
                }
                else if (elements.Length > 2)
                {
                    // Т.е номер вида 515-00-00/2/2013
                    if (maxNumber < elements[1].ToInt())
                    {
                        maxNumber = elements[1].ToInt();
                        result = string.Format("{0}/{1}/{2}", elements[0], maxNumber + 1, elements[2]);
                    }
                }
            }

            if (result.IsEmpty())
            {
                // Что скорее всего означает у переданных номеров неверный формат номера - считаем что данный документ будет первым
                return parentDocumentNumber;
            }

            return result;
        }

        internal class DocumentGjiStateChangeException : Exception
        {
            public DocumentGjiStateChangeException(string message)
                : base(message)
            {

            }
        }
    }
}