namespace Bars.Gkh.DomainService.Dict
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;
    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с сущностью <see cref="BuilderDocumentType"/> - тип документа подрядных организаций
    /// </summary>
    public class BuilderDocumentTypeService : IBuilderDocumentTypeService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Вернуть все типы документов без пагинации
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public IDataResult ListWithoutPaging(BaseParams baseParams)
        {
            var documentTypeDomain = this.Container.ResolveDomain<BuilderDocumentType>();
            var loadParams = baseParams.GetLoadParam();

            try
            {
                var data = documentTypeDomain.GetAll()
                    .Select(x => new {x.Id, x.Code, x.Name})
                    .OrderBy(x => x.Name)
                    .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).ToList(), data.Count());
            }
            catch (Exception ex)
            {
                return new BaseDataResult(false, ex.Message);
            }
            finally
            {
                this.Container.Release(documentTypeDomain);
            }
        }
    }
}
