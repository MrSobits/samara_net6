using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.IoC;
using Bars.B4.Utils;
using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmCharacters;
using System.Linq;

namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.Dict
{
    public class KnmCharacterInterceptor : EmptyDomainInterceptor<KnmCharacter>
    {
        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<KnmCharacter> service, KnmCharacter entity)
        {
            var knmCharacterKindCheckDomain = this.Container.ResolveDomain<KnmCharacterKindCheck>();

            using (this.Container.Using(knmCharacterKindCheckDomain))
            {
                knmCharacterKindCheckDomain.GetAll()
                    .Where(x => x.KnmCharacter.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => knmCharacterKindCheckDomain.Delete(x));
            }

            return base.BeforeDeleteAction(service, entity);
        }
    }
}
