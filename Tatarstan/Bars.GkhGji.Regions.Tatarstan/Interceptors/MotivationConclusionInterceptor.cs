namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Interceptors;

    /// <summary>
    /// Interceptor мотивировочного заключения
    /// </summary>
    public class MotivationConclusionInterceptor : DocumentGjiInterceptor<MotivationConclusion>
    {
        /// <summary>
        /// Действие перед удалением
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<MotivationConclusion> service, MotivationConclusion entity)
        {
            var inspectionGjiDocumentGjiService = this.Container.ResolveDomain<InspectionGjiDocumentGji>();

            using (this.Container.Using(inspectionGjiDocumentGjiService, service))
            {
                var inspections = inspectionGjiDocumentGjiService
                    .GetAll().FirstOrDefault(x => x.Document.Id == entity.Id);

                if (inspections != null)
                {
                    return this.Failure("По данному мотивировочному заключению существует проверка по обращению граждан");
                }

                return base.BeforeDeleteAction(service, entity);
            }
        }
    }
}