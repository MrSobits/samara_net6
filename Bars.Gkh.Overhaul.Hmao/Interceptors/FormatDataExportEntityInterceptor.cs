namespace Bars.Gkh.Overhaul.Hmao.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.Enums.Administration.FormatDataExport;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class FormatDataExportEntityInterceptor : EmptyDomainInterceptor<FormatDataExportEntity>
    {
        /// <inheritdoc />
        public override IDataResult AfterCreateAction(IDomainService<FormatDataExportEntity> service, FormatDataExportEntity entity)
        {
            var dpkrDocumentRepository = this.Container.ResolveRepository<DpkrDocument>();
            var stateRepository = this.Container.ResolveRepository<State>();

            try
            {
                if (entity.EntityType == EntityType.СrProgramDoc && entity.ExportEntityState == FormatDataExportEntityState.Success)
                {
                    var draft = stateRepository.GetAll()
                        .FirstOrDefault(w => w.Code == "3" && w.TypeId == "ovrhl_dpkr_documents");

                    if (draft == null)
                        throw new Exception();

                    var entityId = long.Parse(entity.EntityId);
                    var dpkrDocument = dpkrDocumentRepository.GetAll()
                        .FirstOrDefault(w => entityId == w.Id && w.State.FinalState);

                    if (dpkrDocument != null)
                    {
                        dpkrDocument.State = draft;

                        dpkrDocumentRepository.Update(dpkrDocument);
                    }
                }
            }
            catch (Exception e)
            {
                return this.Failure("Не удалось обновить связанные записи");
            }
            finally
            {
                this.Container.Release(dpkrDocumentRepository);
                this.Container.Release(stateRepository);
            }

            return this.Success();
        }
    }
}