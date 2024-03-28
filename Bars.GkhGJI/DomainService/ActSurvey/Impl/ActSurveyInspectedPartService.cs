namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class ActSurveyInspectedPartService : IActSurveyInspectedPartService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddInspectedParts(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;

                var partStrIds = baseParams.Params.ContainsKey("partIds") ? baseParams.Params["partIds"].ToString() : "";

                if (!string.IsNullOrEmpty(partStrIds))
                {
                    var partIds = partStrIds.Split(',');

                    var listIds = new List<long>();

                    var serviceParts = Container.Resolve<IDomainService<ActSurveyInspectedPart>>();

                    listIds.AddRange(serviceParts.GetAll()
                        .Where(x => x.ActSurvey.Id == documentId)
                        .Select(x => x.InspectedPart.Id)
                        .Distinct()
                        .ToList());

                    foreach (var id in partIds)
                    {
                        var newId = id.ToLong();

                        //Если среди существующих частей уже есть такая часть, то пролетаем мимо
                        if (listIds.Contains(newId))
                            continue;

                        //Если такого эксперта еще нет то добалвяем
                        var newObj = new ActSurveyInspectedPart
                        {
                            ActSurvey = new ActSurvey { Id = documentId },
                            InspectedPart = new InspectedPartGji { Id = newId }
                        };

                        serviceParts.Save(newObj);
                    }
                }
                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult {Success = false, Message = e.Message};
            }
        }
    }
}