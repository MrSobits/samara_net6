namespace Bars.GkhGji.Regions.Nso.DomainService
{
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Nso.Entities;

	/// <summary>
	/// Данный класс служит заменой домен сервиса IDomainService<ActRemoval> на IDomainService<NsoActRemoval>
	/// </summary>
	public class ReplacementActRemovalDomainService : ReplacementDomainService<ActRemoval, NsoActRemoval>
	{
	}
}
