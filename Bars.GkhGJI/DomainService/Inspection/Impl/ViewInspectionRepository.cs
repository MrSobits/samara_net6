namespace Bars.GkhGji.DomainService.Inspection.Impl
{
    using System;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    public class ViewInspectionRepository : IViewInspectionRepository, IRepository<ViewFormatDataExportInspection>
    {
        public IRepository<ViewFormatDataExportInspection> Repository { get; set; }

        /// <inheritdoc />
        public IQueryable<ViewFormatDataExportInspection> GetAll()
        {
            return this.Repository.GetAll();
        }

        /// <inheritdoc />
        public IQueryable<ViewFormatDataExportInspectionDto> GetAllDto()
        {
            return this.Repository.GetAll()
                .Select(x => new ViewFormatDataExportInspectionDto
                {
                    Id = x.Id,
                    DisposalId = x.Disposal.Id,
                    ActCheckId = x.ActCheck.Id,
                    IsPlanned = x.IsPlanned,
                    TypeBase = x.TypeBase,
                    DocumentDate = x.DocumentDate,
                    DocumentNumber = x.DocumentNumber,
                    CheckDate = x.CheckDate,
                    ContragentName = x.ContragentName,
                    MunicipalityName = x.MunicipalityName
                });
        }

        /// <inheritdoc />
        public void Evict(object entity)
        {
            this.Repository.Evict(entity);
        }

        /// <inheritdoc />
        public void Evict(ViewFormatDataExportInspection entity)
        {
            this.Repository.Evict(entity);
        }

        /// <inheritdoc />
        object IRepository.Get(object id)
        {
            return this.Get(id);
        }

        /// <inheritdoc />
        IQueryable IRepository.GetAll()
        {
            return this.GetAll();
        }

        /// <inheritdoc />
        object IRepository.Load(object id)
        {
            return this.Load(id);
        }

        /// <inheritdoc />
        public ViewFormatDataExportInspection Get(object id)
        {
            throw new NotSupportedException("Use IRepository<InspectionGji>");
        }

        /// <inheritdoc />
        public ViewFormatDataExportInspection Load(object id)
        {
            throw new NotSupportedException("Use IRepository<InspectionGji>");
        }

        /// <inheritdoc />
        public void Save(ViewFormatDataExportInspection value)
        {
            throw new NotSupportedException("Use IRepository<InspectionGji>");
        }

        /// <inheritdoc />
        public void Update(ViewFormatDataExportInspection value)
        {
            throw new NotSupportedException("Use IRepository<InspectionGji>");
        }

        /// <inheritdoc />
        IQueryable<ViewFormatDataExportInspection> IRepository<ViewFormatDataExportInspection>.GetAll()
        {
            throw new NotSupportedException("Use IRepository<InspectionGji>");
        }

        /// <inheritdoc />
        void IRepository<ViewFormatDataExportInspection>.Delete(object id)
        {
            throw new NotSupportedException("Use IRepository<InspectionGji>");
        }

        /// <inheritdoc />
        void IRepository.Delete(object id)
        {
            throw new NotSupportedException("Use IRepository<InspectionGji>");
        }

        /// <inheritdoc />
        public void Save(object value)
        {
            throw new NotSupportedException("Use IRepository<InspectionGji>");
        }

        /// <inheritdoc />
        public void Update(object value)
        {
            throw new NotSupportedException("Use IRepository<InspectionGji>");
        }
    }
}