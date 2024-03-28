namespace Bars.GkhGji.Regions.Khakasia.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Khakasia.Entities;

	/// <summary>
	/// Сервис для Мероприятия по контролю распоряжения ГЖИ
	/// </summary>
	public class DisposalControlMeasuresService : IDisposalControlMeasuresService
    {
		/// <summary>
		/// Домен сервис для Мероприятия по контролю распоряжения ГЖИ
		/// </summary>
		public IDomainService<DisposalControlMeasures> DisposalControlMeasuresDomain { get; set; }

		/// <summary>
		/// Добавить Мероприятия по контролю распоряжения ГЖИ
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
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
                            var newObj = new DisposalControlMeasures
                            {
                                Disposal = new GkhGji.Entities.Disposal { Id = documentId },
                                ControlActivity = new ControlActivity { Id = newId }
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