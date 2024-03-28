namespace Bars.Gkh.DomainService.Dict.Impl
{
    using Bars.Gkh.Entities;
    using Bars.B4;
    using System.Linq;

    class MunicipalityDomainService : BaseDomainService<Municipality>
    {

        // временно. чтобы не грузить муниципальные образования нижнего уровня
        // будет убрано после адаптации всех разделов где есть мо
        
        public override IQueryable<Municipality> GetAll()
        {
            return base.GetAll().Where(x => x.ParentMo == null || x.IsOld);
        }

    }
}
