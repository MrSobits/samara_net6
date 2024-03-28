namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Entities;

    public class OrganMvdInterceptor : EmptyDomainInterceptor<OrganMvd>
    {
        /// <summary>
        /// Домен-сервис "Протокол МВД"
        /// </summary>
        public IDomainService<ProtocolMvd> ProtocolMvdService { get; set; }

        /// <summary>
        /// Метод вызывается перед удалением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeDeleteAction(IDomainService<OrganMvd> service, OrganMvd entity)
        {
            if (this.ProtocolMvdService.GetAll().Any(x => x.OrganMvd.Id == entity.Id))
            {
                return this.Failure($"Запись c кодом {entity.Code} не может быть удалена");
            }
            
            return this.Success();
        }
    }
}