namespace Bars.GkhGji.Regions.Tula.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Tula.Entities;

	/// <summary>
	/// Сервис для Предмет проверки для приказа 
	/// </summary>
	public class DisposalSurveySubjectService : IDisposalSurveySubjectService
    {
		/// <summary>
		/// Домен сервис для Предмет проверки для приказа 
		/// </summary>
		public IDomainService<DisposalSurveySubject> Service { get; set; }

		/// <summary>
		/// Добавить предмет проверки для приказа 
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult AddDisposalSurveySubject(BaseParams baseParams)
        {
            var dispId = baseParams.Params.GetAs<long>("disposalId");
            var objectIds = baseParams.Params.GetAs<string>("objectIds");

            if (dispId == 0 || string.IsNullOrEmpty(objectIds))
            {
                return new BaseDataResult { Message = "Нет приказа или предметов проверки для сохранения", Success = false };
            }

            var existingIds = this.Service.GetAll().Where(x => x.Disposal.Id == dispId).Select(x => x.Id).ToList();

            foreach (var objectId in objectIds.Split(',').Select(x => x.Trim().ToLong()))
            {
                if (!existingIds.Contains(objectId))
                {
                    this.Service.Save(new DisposalSurveySubject { Disposal = new GkhGji.Entities.Disposal { Id = dispId }, SurveySubject = new SurveySubject { Id = objectId } });
                }
            }

            return new BaseDataResult();
        }
    }
}