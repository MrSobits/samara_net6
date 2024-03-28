namespace Bars.GkhGji.Regions.Voronezh.Interceptors
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using System.Linq;
    using Bars.GkhGji.Regions.Voronezh.Entities;
    using Bars.GkhGji.Enums;

    class SSTUExportTaskInterceptor : EmptyDomainInterceptor<SSTUExportTask>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<SSTUExportTaskAppeal> SSTUExportTaskAppealDomain { get; set; }

        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<SSTUExportTask> service, SSTUExportTask entity)
        {
            try
            {

                Operator thisOperator = UserManager.GetActiveOperator();
                entity.Operator = thisOperator;
                entity.SSTUExportState = SSTUExportState.WaitForExport;
                entity.ObjectVersion = 1;
                entity.ObjectCreateDate = DateTime.Now;
                entity.ObjectEditDate = DateTime.Now;

                return Success();
            }
            catch (Exception e)
            {
                return Failure("Не удалось создать задачу");
            }

        }

        public override IDataResult BeforeUpdateAction(IDomainService<SSTUExportTask> service, SSTUExportTask entity)
        {
            try
            {

                Operator thisOperator = UserManager.GetActiveOperator();
                entity.Operator = thisOperator;
                entity.ObjectVersion = 1;
                entity.ObjectEditDate = DateTime.Now;

                return Success();
            }
            catch (Exception e)
            {
                return Failure("Не удалось сохранить задачу");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SSTUExportTask> service, SSTUExportTask entity)
        {
            try
            {
                var reportRow = SSTUExportTaskAppealDomain.GetAll()
               .Where(x => x.SSTUExportTask.Id == entity.Id)
               .Select(x => x.Id).ToList();
                foreach (var id in reportRow)
                {
                    SSTUExportTaskAppealDomain.Delete(id);
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure(e.ToString());
            }

        }
    }
}
