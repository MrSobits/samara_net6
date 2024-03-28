namespace Bars.GkhGji.Regions.Khakasia.DomainService
{
	using System.Collections.Generic;
	using System.Linq;
	using B4.DataAccess;

	using Bars.GkhGji.Entities;

	using Castle.Windsor;

	using Gkh.Domain.CollectionExtensions;

	using ResolProsDefinition = Bars.GkhGji.Regions.Khakasia.Entities.ResolProsDefinition;

	/// <summary>
	/// Сервис для Определение
	/// </summary>
	public class DefinitionService : IDefinitionService
    {
		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Получить максимальный номер определения
		/// </summary>
		/// <param name="year">Год</param>
		/// <returns>Номер определения</returns>
		public int GetMaxDefinitionNum(int year)
        {
            var resolProsDefDomain = this.Container.ResolveDomain<ResolProsDefinition>();
            var actCheckDefDomain = this.Container.ResolveDomain<ActCheckDefinition>();
            var protocolDefDomain = this.Container.ResolveDomain<ProtocolDefinition>();
            var protocolMhcDefDomain = this.Container.ResolveDomain<ProtocolMhcDefinition>();
            var resolutionDefDomain = this.Container.ResolveDomain<ResolutionDefinition>();

            try
            {
                var listNums = new List<int>();

                listNums.Add(resolProsDefDomain.GetAll().Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == year).SafeMax(x => x.DocumentNumber) ?? 0);
                listNums.Add(actCheckDefDomain.GetAll().Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == year).SafeMax(x => x.DocumentNumber) ?? 0);
                listNums.Add(protocolDefDomain.GetAll().Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == year).SafeMax(x => x.DocumentNumber) ?? 0);
                listNums.Add(protocolMhcDefDomain.GetAll().Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == year).SafeMax(x => x.DocumentNumber) ?? 0);
                listNums.Add(resolutionDefDomain.GetAll().Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == year).SafeMax(x => x.DocumentNumber) ?? 0);

                return listNums.SafeMax(x => x);
            }
            finally
            {
                this.Container.Release(resolProsDefDomain);
                this.Container.Release(actCheckDefDomain);
                this.Container.Release(protocolDefDomain);
                this.Container.Release(protocolMhcDefDomain);
                this.Container.Release(resolutionDefDomain);
            }
        }
    }
}