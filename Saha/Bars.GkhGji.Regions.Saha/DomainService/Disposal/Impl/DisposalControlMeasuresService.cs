namespace Bars.GkhGji.Regions.Saha.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Saha.Entities;

    public class DisposalControlMeasuresService : IDisposalControlMeasuresService
    {
        public IDomainService<DisposalControlMeasures> DisposalControlMeasuresDomain { get; set; }

        public IDataResult AddDisposalControlMeasures(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;
                var controlMeasuresIds = 
                    baseParams.Params.ContainsKey("controlMeasuresIds") 
                        ? baseParams.Params["controlMeasuresIds"].ToString() 
                        : string.Empty;

                if (!string.IsNullOrEmpty(controlMeasuresIds))
                {
                    //список уже добавленных мероприятий по контролю
                    var listTypes =
                        DisposalControlMeasuresDomain
                            .GetAll()
                            .Where(x => x.Disposal.Id == documentId)
                            .Select(x => x.ControlActivity.Id)
                            .ToList();

                    foreach (var controlMeasureIds in controlMeasuresIds.Split(','))
                    {
                        var newId = controlMeasureIds.ToLong();

                        if (!listTypes.Contains(newId))
                        {
                            var newObj = new DisposalControlMeasures
                            {
                                Disposal = new GkhGji.Entities.Disposal { Id = documentId },
                                ControlActivity = new ControlActivity { Id = newId }
                            };

                            DisposalControlMeasuresDomain.Save(newObj);
                        }
                    }
                }
                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }
    }
}