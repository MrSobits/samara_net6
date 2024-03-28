namespace Bars.GisIntegration.Smev.Tasks.PrepareData
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Bars.GisIntegration.Smev.ConfigSections;
    using Bars.GisIntegration.Smev.SmevExchangeService.Erp;
    using Bars.GisIntegration.Smev.Tasks.PrepareData.Base;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;

    public class ProsecutorOfficePrepareDataTask : ErpPrepareDataTask<LetterToErpType>
    {
        private LetterToErpType RequestObject { get; set; }

        private long ObjectId { get; set; }

        /// <inheritdoc />
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var id = parameters.GetAsId();
            this.ObjectId = id;
            this.RequestObject = this.GetData(parameters);
        }

        /// <inheritdoc />
        protected override List<ValidateObjectResult> ValidateData()
        {
            var errors = string.Empty;

            var request = ((MessageToErpGetType)this.RequestObject.Item)
                .Items.OfType<SpecificDictionaryRequestType>().First();

            if (string.IsNullOrWhiteSpace(request.guid))
            {
                errors = "Не заполнено поле \"Идентификатор справочника прокуратур (формата GUID)\"";
            }

            var validateResult = new ValidateObjectResult
            {
                Message = errors,
                State = string.IsNullOrEmpty(errors) ? ObjectValidateState.Success : ObjectValidateState.Error
            };

            return new List<ValidateObjectResult> { validateResult };
        }

        /// <inheritdoc />
        protected override LetterToErpType GetRequestObject(ref bool isTestMessage, out long objectId)
        {
            objectId = this.ObjectId;
            return this.RequestObject;
        }

        private LetterToErpType GetData(DynamicDictionary parameters) =>
            new LetterToErpType
            {
                Item = new MessageToErpGetType
                {
                    Items = new object[]
                    {
                        new SpecificDictionaryRequestType
                        {
                            guid = this.Container.GetGkhConfig<SmevIntegrationConfig>().GuideProsecutorId.ToUpper()
                        }
                    }
                }
            };
    }
}