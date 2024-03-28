namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    public class DisposalControlMeasuresService : IDisposalControlMeasuresService
    {
        public IDomainService<DisposalControlMeasures> DisposalControlMeasuresDomain { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }

        public IDomainService<ControlActivity> ControlActivityDomain { get; set; }

        public IDataResult AddDisposalControlMeasures(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;
                var controlMeasuresIds =
                    baseParams.Params.ContainsKey("controlMeasuresIds")
                        ? baseParams.Params["controlMeasuresIds"].ToString()
                        : string.Empty;

                var disposal = DisposalDomain.Get(documentId);

                if (!string.IsNullOrEmpty(controlMeasuresIds))
                {
                    // список уже добавленных мероприятий по контролю
                    var listTypes =
                        this.DisposalControlMeasuresDomain
                            .GetAll()
                            .Where(x => x.Disposal.Id == documentId)
                            .Select(x => x.ControlActivity.Id)
                            .ToList();

                    foreach (var controlMeasureIds in controlMeasuresIds.Split(','))
                    {
                        var newId = controlMeasureIds.ToLong();

                        if (!listTypes.Contains(newId))
                        {
                            string controlActivityName = ControlActivityDomain.Get(newId).Name;
                            var newObj = new DisposalControlMeasures
                            {
                                Disposal = new GkhGji.Entities.Disposal { Id = documentId },
                                ControlActivity = new ControlActivity { Id = newId },
                                DateStart = disposal.DateStart,
                                DateEnd = disposal.DateEnd,
                                Description = controlActivityName
                            };

                            this.DisposalControlMeasuresDomain.Save(newObj);
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