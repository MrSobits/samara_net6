namespace Bars.GkhGji.StateChange
{
    using Bars.B4;
    using System;
    using System.Linq;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.Entities;

    public class ProtocolRSOValidationRule : BaseDocValidationRule
    {
        public override string Id
        {
            get { return "gji_document_protocolrso_validation_rule"; }
        }

        public override string Name
        {
            get { return "Проверка заполненности карточки Протокол РСО"; }
        }

        public override string TypeId
        {
            get { return "gji_document_protocolrso"; }
        }

        public override string Description
        {
            get { return "Данное правило заполняет номер протокола РСО"; }
        }

        public override ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is DocumentGji || statefulEntity is ProtocolRSO)
            {
                var ProtocolRSOService = Container.Resolve<IDomainService<ProtocolRSO>>();
                var ProtocolRSORO = Container.Resolve<IDomainService<ProtocolRSORealityObject>>();
                var documentService = Container.Resolve<IDomainService<DocumentGji>>();

                var document = statefulEntity as DocumentGji;
                var protocolRSO = statefulEntity as ProtocolRSO;

                try
                {
                    if (oldState.StartState)
                    {
                        var municipalityCode = ProtocolRSORO.GetAll()
                                          .Where(x => x.ProtocolRSO.Id == document.Id)
                                          .Where(x => x.RealityObject != null)
                                          .Select(x => x.RealityObject.Municipality.Code).FirstOrDefault();

                        if (municipalityCode == null || municipalityCode == "")
                        {
                            return
                                ValidateResult.No(" Не указаны объекты проверки");
                        }
                        else
                        {
                            //получаем номера
                            var documentNum = documentService.GetAll()
                                .Where(x => x.Id != document.Id)
                                .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal
                                            || x.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor
                                            || x.TypeDocumentGji == TypeDocumentGji.Resolution
                                            || x.TypeDocumentGji == TypeDocumentGji.ProtocolRSO)
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
                            if (nextFreetNum > 0)
                            {
                                string documentNumber = municipalityCode + "-" + nextFreetNum;
                                if (string.IsNullOrEmpty(document.DocumentNumber) && string.IsNullOrEmpty(protocolRSO.DocumentNumber))
                                {
                                    document.DocumentNum = nextFreetNum;
                                    document.DocumentNumber = documentNumber;
                                    protocolRSO.DocumentNum = nextFreetNum;
                                    protocolRSO.DocumentNumber = documentNumber;
                                }
                            }

                        }
                    }

                }
                catch (Exception exc)
                {
                    throw exc;
                }
                finally
                {

                }
            }

            return ValidateResult.Yes();
        }
    }
}