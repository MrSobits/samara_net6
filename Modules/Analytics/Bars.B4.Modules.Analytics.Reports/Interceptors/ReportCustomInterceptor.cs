namespace Bars.B4.Modules.Analytics.Reports.Interceptors
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Entities;

    /// <summary>
    /// Интерцептор сущности "ReportCustom"
    /// </summary>
	public class ReportCustomInterceptor : EmptyDomainInterceptor<ReportCustom>
    {
        /// <summary>
        /// Действия перед созданием сущности
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param>
        /// <returns></returns>
	    public override IDataResult BeforeCreateAction(IDomainService<ReportCustom> service, ReportCustom entity)
        {
            try
            {
                ValidateEntity(service, entity);
            }
            catch (Exception exception)
            {
                return Failure(exception.Message);
            }

            return base.BeforeCreateAction(service, entity);
        }

        /// <summary>
        /// Действия перед обновлением сущности
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param>
        /// <returns></returns>
        public override IDataResult BeforeUpdateAction(IDomainService<ReportCustom> service, ReportCustom entity)
        {
            try
            {
                ValidateEntity(service, entity);
            }
            catch (Exception exception)
            {
                return Failure(exception.Message);
            }

            return base.BeforeUpdateAction(service, entity);
        }

        private void ValidateEntity(IDomainService<ReportCustom> service, ReportCustom entity)
        {
            if (string.IsNullOrEmpty(entity.CodedReportKey))
            {
                throw new Exception("Не выбран базовый отчет");
            }

            if (!CheckUnique(service, entity))
            {
                throw new Exception("Замена шаблона для данного типа отчета уже создана");
            }
        }

        private bool CheckUnique(IDomainService<ReportCustom> service, ReportCustom entity)
        {
            return entity.Id == 0 ?
                !service.GetAll().Any(x => x.CodedReportKey == entity.CodedReportKey) :
                !service.GetAll().Any(x => x.CodedReportKey == entity.CodedReportKey && x.Id != entity.Id);
        }
    }
}
