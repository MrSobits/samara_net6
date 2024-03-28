namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Impl
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.ErknmTypeDocuments;
    using Castle.Windsor;
    using System.Linq;

    public class ErknmTypeDocumentsService : IErknmTypeDocumentsService
    {
        /// <summary>
        /// IoC container
        /// </summary>
        public IWindsorContainer Container { get; set; }

        public JsonNetResult ListWithoutPaging(BaseParams baseParams)
        {
            var typeDocumentDomain = this.Container.Resolve<IDomainService<ErknmTypeDocument>>();

            using (this.Container.Using(typeDocumentDomain))
            {
                var data = typeDocumentDomain.GetAll()
                    .Where(x => x.IsBasisKnm)
                    .OrderBy(x => x.DocumentType)
                    .ToList();

                return new JsonNetResult(new { success = true, data, totalCount = data.Count });
            }
        }
    }
}
