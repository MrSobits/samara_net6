namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Gkh.Authentification;
    using Entities;

    //Болванка на случай если от этого класса наследвоались
    public class ViewBaseStatementDomainService : ViewBaseStatementDomainService<ViewBaseStatement>
    {
        //Внимание!!! все override писать в Generic 
    }
    
    public class ViewBaseStatementDomainService<T> : BaseDomainService<T>
        where T : ViewBaseStatement
    {
        public override IQueryable<T> GetAll()
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            try
            {
                //var inspectorList = userManager.GetInspectorIds();Временно убрана фильтрация по Инспекторам
                var municipalityList = userManager.GetMunicipalityIds();

                //var serviceInspectionRealityObject = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

                return base.GetAll()
                        .WhereIf(municipalityList.Count > 0, x => x.MunicipalityId.HasValue && municipalityList.Contains(x.MunicipalityId.Value));
            }
            finally 
            {
                Container.Release(userManager);
            }
        }
    }
}