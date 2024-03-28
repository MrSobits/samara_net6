namespace Bars.GkhGji.Regions.Smolensk.StateChange
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActCheckValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id { get { return "gji_actcheck_validation_number"; } }

        public override string Name { get { return "Перенесено из ННовгорода - Проверка возможности формирования номера акта проверки"; } }

        public override string TypeId { get { return "gji_document_actcheck"; } }

        public override string Description { get { return "Перенесено из ННовгорода - Данное правило проверяет формирование номера акта проверки в соответствии с правилами"; } }

        protected override void Action(DocumentGji document)
        {
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

                //строковый номер, целая часть номера и подномер берутся из родительского документа
                if (parentDocument != null && !string.IsNullOrEmpty(parentDocument.DocumentNumber))
                {
                    document.DocumentNum = parentDocument.DocumentNum;

                    if (docGjiChildrenService.GetAll().Any(x => x.Parent == parentDocument
                        && x.Children.TypeDocumentGji == document.TypeDocumentGji
                        && x.Id != document.Id))
                    {
                        var subNumber = docGjiChildrenService.GetAll()
                                                  .Where(x => x.Parent == parentDocument && x.Id != document.Id)
                                                  .Where(x => x.Children.TypeDocumentGji == document.TypeDocumentGji)
                                                  .Select(x => x.Children.DocumentSubNum)
                                                  .Max()
                                                  .ToInt();

                        document.DocumentSubNum = subNumber + 1;

                        if (subNumber == 0)
                        {
                            document.DocumentNumber = parentDocument.DocumentNumber;
                            return;
                        }

                        var parentNumber = parentDocument.DocumentNumber.Split('/').ToList();
                        if (parentNumber.Count > 0)
                        {
                            parentNumber.Insert(parentNumber.Count - 1, subNumber.ToString(CultureInfo.InvariantCulture));

                            document.DocumentNumber = string.Join("/", parentNumber);
                        }
                    }
                    else
                    {
                        // Документов такого типа нет - присваиваем номер родительского
                        document.DocumentNumber = parentDocument.DocumentNumber;
                    }
                }
                //var checkParentDoc =
                //                    docGjiChildrenService.GetAll()
                //                        .FirstOrDefault(x => x.Children.Id == document.Id)
                //                        .With(x => x.Parent);
                //if (checkParentDoc == null)
                //{
                //    throw new Exception("Не удалось получить родительский документ");
                //}

                //document.DocumentNum = checkParentDoc.DocumentNum;

                //var disposalChildDocuments =
                //    docGjiChildrenService.GetAll()
                //        .Where(x => x.Children.TypeDocumentGji == document.TypeDocumentGji
                //                && x.Parent == checkParentDoc)
                //        .Select(x => x.Children.DocumentNumber)
                //        .ToArray();
                //document.DocumentNumber = disposalChildDocuments.Count() == 1
                //    ? checkParentDoc.DocumentNumber
                //    : GetNumber(disposalChildDocuments, checkParentDoc.DocumentNumber);
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