using Bars.B4;
using Bars.Gkh.DomainService;
using Bars.Gkh.Entities;
using Bars.Gkh.Overhaul.Tat.Entities;
using System;
using System.Linq;

namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    /// <inheritdoc />
    public class GkhRealObjOverhaulDataObject : BaseRealObjOverhaulDataObject
    {
        /// <summary>
        /// Документ, на основании которого дом включен в ДПКР
        /// </summary>
        private DpkrDocument _dpkrDocumentIncludedRo;

        /// <summary>
        /// Документ, на основании которого дом исключен из ДПКР
        /// </summary>
        private DpkrDocument _dpkrDocumentExcludedRo;

        /// <summary>
        /// Домен-сервис для сущности Связь дома с документом ДПКР
        /// </summary>
        public IDomainService<DpkrDocumentRealityObject> DpkrDocumentRealityObjectDomain { get; set; }

        public override void Init(RealityObject ro)
        {
            var dpkrDocumentRoList = this.DpkrDocumentRealityObjectDomain.GetAll()
                .Where(x => x.RealityObject == ro)
                .ToList();

            if (dpkrDocumentRoList.Any())
            {
                _dpkrDocumentIncludedRo = dpkrDocumentRoList
                    .FirstOrDefault(x => x.IsIncluded)?
                    .DpkrDocument;

                _dpkrDocumentExcludedRo = dpkrDocumentRoList
                    .FirstOrDefault(x => x.IsExcluded)?
                    .DpkrDocument;
            }

            base.Init(ro);
        }

        /// <inheritdoc />
        public override string DocumentNumber => _dpkrDocumentIncludedRo?.DocumentNumber;

        /// <inheritdoc />
        public override DateTime? DocumentDate => _dpkrDocumentIncludedRo?.DocumentDate;

        /// <inheritdoc />
        public override DateTime? PublishDate => _dpkrDocumentIncludedRo?.PublicationDate;

        /// <inheritdoc />
        public override DateTime? UnpublishDate => _dpkrDocumentExcludedRo?.PublicationDate;

        /// <inheritdoc />
        public override DateTime? ObligationDate
        {
            get
            {
                if (this.realityObject.DateCommissioning == null)
                {
                    return null;
                }

                return this.realityObject.DateCommissioning.Value.Year >= 2014
                    ? _dpkrDocumentIncludedRo?.ObligationAfter2014
                    : _dpkrDocumentIncludedRo?.ObligationBefore2014;
            }
        }
    }
}
