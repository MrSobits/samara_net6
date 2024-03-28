namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class KnmTypesViewModel : BaseViewModel<KnmTypes>
    {
        public override IDataResult List(IDomainService<KnmTypes> domain, BaseParams baseParams)
        {
            var knmTypeKindCheckDomain = this.Container.ResolveDomain<KnmTypeKindCheck>();

            using (this.Container.Using(knmTypeKindCheckDomain))
            {
                return domain.GetAll()
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        KindCheck = knmTypeKindCheckDomain.GetAll()
                            .Where(y=> y.KnmTypes.Id == x.Id)
                            .Select(y=> y.KindCheckGji)
                            .ToArray(),
                        x.ErvkId
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);   
            }
        }        
    }
}