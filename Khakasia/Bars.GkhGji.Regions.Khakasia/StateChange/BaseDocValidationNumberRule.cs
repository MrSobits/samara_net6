using System.Collections.Generic;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Regions.Khakasia.StateChange
{
    using System;
    using System.Linq;
    using B4;
    using B4.Modules.States;
    using B4.Utils;
    using Castle.Windsor;

    public abstract class BaseDocValidationNumberRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<DocumentGji> documentService { get; set; }

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
            var num = documentService.GetAll()
                        .Where(x => x.TypeDocumentGji == document.TypeDocumentGji)
                        .Where(x => x.Id != document.Id)
                        .Where(
                            x =>
                                x.DocumentDate.HasValue && x.DocumentDate.Value.Year == document.DocumentDate.Value.Year)
                                .Select(x => x.DocumentNum)
                        .SafeMax(x => x) ?? 0;

            num++;

            document.DocumentNum = num;
            document.DocumentNumber = num.ToString();

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