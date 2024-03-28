using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.IoC;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmCharacters;
using System.Linq;

namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Dict
{
    public class KnmCharacterViewModel : BaseViewModel<KnmCharacter>
    {
        public override IDataResult List(IDomainService<KnmCharacter> domainService, BaseParams baseParams)
        {
            var knmCharacterDomain = this.Container.ResolveDomain<KnmCharacterKindCheck>();
            using (this.Container.Using(knmCharacterDomain))
            {
                return domainService.GetAll()
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.ErknmCode,
                        KindCheck = knmCharacterDomain.GetAll()
                            .Where(y => y.KnmCharacter.Id == x.Id)
                            .Select(y => y.KindCheckGji)
                            .ToArray()
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }
    }
}
