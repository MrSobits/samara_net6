using System;
using System.Globalization;
using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Regions.Smolensk.StateChange
{
    public class PrescriptionValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id { get { return "gji_prescription_validation_number"; } }

        public override string Name { get { return "Перенесено из ННовгорода - Проверка возможности формирования номера предписания"; } }

        public override string TypeId { get { return "gji_document_prescr"; } }

        public override string Description { get { return "Перенесено из ННовгорода - Данное правило проверяет формирование номера предписания в соответствии с правилами"; } }

        protected override void Action(DocumentGji document)
        {
            var docGjiService = Container.Resolve<IDomainService<DocumentGji>>();
            var documentGjiChildrenService = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            try
            {
                // Год берется из даты документа
                document.DocumentYear =
                    document.DocumentDate.HasValue
                        ? document.DocumentDate.Value.Year
                        : (int?)null;

                var parentDocument =
                    documentGjiChildrenService.GetAll()
                        .Where(x => x.Children.Id == document.Id)
                        .Select(x => x.Parent)
                        .FirstOrDefault();

                //строковый номер, целая часть номера и подномер берутся из родительского документа
                if (parentDocument != null && !string.IsNullOrEmpty(parentDocument.DocumentNumber))
                {
                    document.DocumentNum = parentDocument.DocumentNum;

                    if (documentGjiChildrenService.GetAll().Any(x => x.Parent == parentDocument
                        && x.Children.TypeDocumentGji == document.TypeDocumentGji
                        && x.Id != document.Id))
                    {
                        var subNumber = documentGjiChildrenService.GetAll()
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