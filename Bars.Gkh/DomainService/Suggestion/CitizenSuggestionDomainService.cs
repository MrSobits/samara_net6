namespace Bars.Gkh.DomainService
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.Entities.Suggestion;

    public class CitizenSuggestionDomainService : BaseDomainService<CitizenSuggestion>
    {
        public override IDataResult Save(BaseParams baseParams)
        {
            var service = Container.Resolve<ICitizenSuggestionService>();
            var values = new List<CitizenSuggestion>();
            InTransaction(() =>
            {
                var saveParam = GetSaveParam(baseParams);
                foreach (var record in saveParam.Records)
                {
                    var value = record.AsObject();

                    value.HasAnswer = !string.IsNullOrWhiteSpace(value.AnswerText);

                    SaveInternal(value);
                    values.Add(value);
                    service.SaveAnswerFile(baseParams, value);
                }
            });

            return new SaveDataResult(values); 
        }

        public override IDataResult Update(BaseParams baseParams)
        {
            var service = Container.Resolve<ICitizenSuggestionService>();
            var values = new List<CitizenSuggestion>();
            InTransaction(() =>
            {
                var saveParam = GetSaveParam(baseParams);
                foreach (var record in saveParam.Records)
                {
                    var value = record.AsObject();

                    value.HasAnswer = !string.IsNullOrWhiteSpace(value.AnswerText);
                    
                    UpdateInternal(value);
                    values.Add(value);
                    service.SaveAnswerFile(baseParams, value);
                }
            });

            return new BaseDataResult(values);
        }
    }
}