namespace Bars.Gkh.DomainService.Dict
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities.Dicts;
    using Castle.Windsor;

    /// <summary>
    /// Сервис для <see cref="ResettlementProgram"/>
    /// </summary>
    public class ResettlementProgramService : IResettlementProgramService
	{
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Домен-сервис для <see cref="ResettlementProgram"/>
		/// </summary>
		public IDomainService<ResettlementProgram> ResettlementProgramDomainService { get; set; }

	    /// <summary>
	    /// Получить список программ переселения без пагинации
	    /// </summary>
	    /// <param name="baseParams">Базовые параметры</param>
	    /// <returns>Результат выполнения запроса</returns>
	    public IDataResult ListWithoutPaging(BaseParams baseParams)
	    {
		    var data = this.ResettlementProgramDomainService.GetAll()
			    .Select(
				    x => new
				    {
					    x.Id,
					    x.Name
				    })
			    .OrderBy(x => x.Name);

		    return new ListDataResult(data.ToList(), data.Count());
	    }
	}
}
