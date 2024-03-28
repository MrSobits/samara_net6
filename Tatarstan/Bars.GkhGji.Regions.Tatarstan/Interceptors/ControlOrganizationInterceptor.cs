namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Orgs;

    /// <summary>
    /// Interceptor Контрольно-надзорного органа (КНО)
    /// </summary>
    public class ControlOrganizationInterceptor : EmptyDomainInterceptor<ControlOrganization>
    {  
        /// <summary>
        /// Действие, выполняемое до удаления сущности 
        /// </summary>
        /// <param name="service">Домен-сервис "Контрольно-надзорного органа (КНО)"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<ControlOrganization> service, ControlOrganization entity)
        {
            try
            {
                var controlOrganizationControlTypeRelationService = this.Container.Resolve<IDomainService<ControlOrganizationControlTypeRelation>>();
                var tatarstanZonalInspectionService = this.Container.Resolve<IDomainService<TatarstanZonalInspection>>();

                using (this.Container.Using(controlOrganizationControlTypeRelationService, tatarstanZonalInspectionService))
                {
                    controlOrganizationControlTypeRelationService.GetAll()
                        .Where(w => w.ControlOrganization.Id == entity.Id)
                        .Select(s => s.Id)
                        .ForEach(f => controlOrganizationControlTypeRelationService.Delete(f));
                                        
                    tatarstanZonalInspectionService.GetAll()
                        .Where(w => w.ControlOrganization.Id == entity.Id)
                        .ForEach(f => {
                            f.ControlOrganization = null;
                            tatarstanZonalInspectionService.Update(f);
                        });
                }

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
        }
    }
}
