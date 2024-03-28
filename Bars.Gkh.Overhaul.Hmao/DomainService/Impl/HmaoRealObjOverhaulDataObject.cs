namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using System;
    using Bars.B4;
    using System.Linq;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <inheritdoc />
    public class HmaoRealObjOverhaulDataObject : BaseRealObjOverhaulDataObject
    {
        private DateTime? publishDate;
        
        private DateTime? unpublishDate;

        public IDomainService<DpkrDocumentRealityObject> DpkrDocumentRealityObjectDomain { get; set; }
        
        /// <inheritdoc />
        public override DateTime? PublishDate => this.publishDate;

        /// <inheritdoc />
        public override DateTime? UnpublishDate => this.unpublishDate;
        
        /// <inheritdoc />
        public override void Init(RealityObject ro)
        {
            var dpkrDocumentRoList = this.DpkrDocumentRealityObjectDomain.GetAll()
                .Where(x => x.RealityObject.Id == ro.Id)
                .Select(x => new
                {
                    x.IsExcluded,
                    x.DpkrDocument.DocumentDate
                })
                .ToList();

            if (dpkrDocumentRoList.Any())
            {
                this.publishDate = dpkrDocumentRoList.FirstOrDefault(x => !x.IsExcluded)?.DocumentDate;
                this.unpublishDate = dpkrDocumentRoList.FirstOrDefault(x => x.IsExcluded)?.DocumentDate;
            }
            
            base.Init(ro);
        }
    }
}