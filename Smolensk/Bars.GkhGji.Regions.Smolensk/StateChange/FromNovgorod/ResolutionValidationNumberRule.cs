using System;
using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Regions.Smolensk.StateChange
{
    public class ResolutionValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id { get { return "gji_resolution_validation_number"; } }

        public override string Name { get { return "Перенесено из ННовгорода - Проверка возможности формирования номера постановления"; } }

        public override string TypeId { get { return "gji_document_resol"; } }

        public override string Description { get { return "Перенесено из ННовгорода - Данное правило проверяет формирование номера постановления в соответствии с правилами"; } }

        protected override void Action(DocumentGji document)
        {
            var docGjiService = Container.Resolve<IDomainService<DocumentGji>>();
            var docGjiChildrenService = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            try
            {
                // Год берется из даты документа
                document.DocumentYear =
                    document.DocumentDate.HasValue
                        ? document.DocumentDate.Value.Year
                        : (int?)null;

                var parentDocument =
                    docGjiChildrenService.GetAll()
                        .Where(x => x.Children.Id == document.Id)
                        .Select(x => x.Parent)
                        .FirstOrDefault();

                if (parentDocument != null && !string.IsNullOrEmpty(parentDocument.DocumentNumber))
                {
                    document.DocumentNum = parentDocument.DocumentNum;

                    if (docGjiService.GetAll()
                        .Any(x => x.Stage.Id == document.Stage.Id && x.Id != document.Id))
                    {
                        document.DocumentSubNum =
                            docGjiService.GetAll()
                                .Where(x => x.Stage.Id == document.Stage.Id && x.Id != document.Id)
                                .Select(x => x.DocumentSubNum)
                                .Max()
                                .ToInt() + 1;
                    }
                }

                var parentDoc =
                    docGjiChildrenService.GetAll()
                                         .FirstOrDefault(x => x.Children.Id == document.Id)
                                         .With(x => x.Parent);
                if (parentDoc.TypeDocumentGji == TypeDocumentGji.Protocol)
                {
                    document.DocumentNumber = parentDoc.DocumentNumber;
                }
                else
                {
                    var resolChildDocuments =
                        docGjiChildrenService.GetAll()
                                             .Where(x => x.Parent == parentDoc)
                                             .Select(x => x.Children.DocumentNumber)
                                             .ToArray();
                    document.DocumentNumber = resolChildDocuments.Count() == 1
                                                  ? parentDoc.DocumentNumber
                                                  : this.GetNumber(resolChildDocuments, parentDoc.DocumentNumber);
                }
            }
            catch (Exception e)
            {
                if (e is DocumentGjiStateChangeException)
                {
                    throw;
                }

                throw new Exception("Не удалось установить номер документа");
            }
        }
    }
}