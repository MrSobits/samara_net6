namespace Bars.GkhGji.Regions.Tomsk.StateChange
{
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ProtocolValidationNumberRule : BaseDocValidationNumberRule
    {
        public IDomainService<Protocol> ProtocolDomain { get; set; }

        public IDomainService<Disposal> DisaposalDomain { get; set; }

        public override string Id { get { return "gji_protocol_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера протокола Томска"; } }

        public override string TypeId { get { return "gji_document_prot"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера протокола в соответствии с правилами Томска"; } }

        protected override ValidateResult Action(DocumentGji document)
        {
            var result = new ValidateResult();

            // Если данное правило подставили под другой документ, то ничего не делаем
            if (document is Protocol)
            {
                var protocol = document as Protocol;


                // В случае, если получилось создать протокол, у которого нет родительского этапа - выходим
                if (protocol.Stage == null)
                {
                    return result;
                }

                if (protocol.Stage != null && protocol.Stage.Parent == null)
                {
                    return result;
                }

                if (protocol.DocumentNum == null && string.IsNullOrWhiteSpace(protocol.DocumentNumber))
                {
                    /*
                       Необходимо получить родительский приказ и в рамках этой ветки взять все протоколы
                       нумерация должна быть по протоколам текущей ветки относительно приказа 
                     */

                    var disposal =
                        DisaposalDomain.GetAll()
                            .Where(x => x.Stage.Id == protocol.Stage.Parent.Id)
                            .Select(x => new
                                             {
                                                 x.DocumentNumber, 
                                                 stageId = x.Stage.Id
                                             })
                            .FirstOrDefault();

                    if (disposal == null)
                    {
                        result.Message = "Приказ не найден.";
                        result.Success = false;
                        return result;
                    }

                    if (string.IsNullOrEmpty(disposal.DocumentNumber))
                    {
                        result.Message = "Приказ не имеет номера.";
                        result.Success = false;
                        return result;
                    }

                    var siblings = ProtocolDomain.GetAll()
                        .Where(x => x.Id != protocol.Id)
                        .Where(x => x.Stage.Parent.Id == disposal.stageId)
                        .Select(x => x.DocumentSubNum)
                        .ToList();

                    var documentSubNum = (int?)null;

                    if (siblings.Any())
                    {
                        documentSubNum = (siblings.Max() ?? 0) + 1;
                    }

                    protocol.DocumentSubNum = documentSubNum;

                    protocol.DocumentNumber = disposal.DocumentNumber;
                    if (protocol.DocumentSubNum.ToInt() > 0)
                    {
                        protocol.DocumentNumber += "/" + protocol.DocumentSubNum.ToInt().ToString(CultureInfo.InvariantCulture);
                    }
                    
                   /* по задаче 59475 : протоколы не должны получат номер относительно прямого родителя,
                    * необходимо получать номер относительно приказа и всех протоколов внутри этой ветки
                    */
                }
            }

            result.Success = true;

            return result;
        }
    }
}