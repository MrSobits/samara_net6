namespace Bars.Gkh.RegOperator.Interceptors
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;

    using Entities;
    using Gkh.Domain;
    using GkhCr.Entities;

    public class PerformedWorkActInterceptor : EmptyDomainInterceptor<PerformedWorkAct>
    {
        public IRepository<RealObjSupplierAccOperPerfAct> RealObjSupplierAccOperPerfActDomain { get; set; }
        public IRepository<RealityObjectSupplierAccountOperation> AccountOperationDomain { get; set; }
        public IStateProvider StateProvider { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<PerformedWorkAct> service, PerformedWorkAct entity)
        {
            var notNullCount = RealObjSupplierAccOperPerfActDomain.GetAll().Count(x => x.PerformedWorkAct.Id == entity.Id
                && x.SupplierAccOperation != null && (x.SupplierAccOperation.Debt > 0 || x.SupplierAccOperation.Credit > 0));

            if (notNullCount > 0)
            {
                var emptyPerfomedWorkAct = new PerformedWorkAct();
                StateProvider.SetDefaultState(emptyPerfomedWorkAct);
                var defaultState = emptyPerfomedWorkAct.State;

                if (defaultState != null && entity.State != null && entity.State.Id != defaultState.Id)
                {
                    return Failure(string.Format(
                        "Удаление невозможно! Есть связанные оплаты в счете расчета с поставщиками. Для удаления переведите в статус {0}",
                        defaultState.Name));
                }

                var relations = RealObjSupplierAccOperPerfActDomain.GetAll()
                    .Where(x => x.PerformedWorkAct.Id == entity.Id);

                var operationIds = relations.Select(x => x.SupplierAccOperation.Id).ToArray();
                
                relations.Select(x => x.Id).ForEach(x => RealObjSupplierAccOperPerfActDomain.Delete(x));

                AccountOperationDomain.GetAll()
                    .Where(x => operationIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .ForEach(x => AccountOperationDomain.Delete(x));
            }


            //Так как при копирование ProgramCR не происходило копирования файла, то существуют записи, ссылающиеся на один и тот же файл
            //Для этих файлов делаем ReCreateFile(для CostFile, AdditionalFile, DocumentFile)
            CheckFilesAndRecreate(service, entity);

            return Success();
        }


        private void CheckFilesAndRecreate(IDomainService<PerformedWorkAct> service, PerformedWorkAct entity)
        {
            var fileService = Container.Resolve<IFileService>();
            try
            {
                if (entity.CostFile != null)
                {
                    var actsWithCostFile = service.GetAll().Where(act => act.CostFile.Id == entity.CostFile.Id && act.Id != entity.Id).ToArray();

                    if (actsWithCostFile.Length > 0)
                    {
                        foreach (var performedWorkAct in actsWithCostFile)
                        {
                            performedWorkAct.CostFile = fileService.ReCreateFile(entity.CostFile);
                            service.Update(performedWorkAct);
                        }
                    }
                }

                if (entity.AdditionFile != null)
                {
                    var actsWithAdditionFile = service.GetAll().Where(act => act.AdditionFile.Id == entity.AdditionFile.Id && act.Id != entity.Id).ToArray();

                    if (actsWithAdditionFile.Length > 0)
                    {
                        foreach (var performedWorkAct in actsWithAdditionFile)
                        {
                            performedWorkAct.AdditionFile = fileService.ReCreateFile(entity.AdditionFile);
                            service.Update(performedWorkAct);
                        }
                    }
                }

                if (entity.DocumentFile != null)
                {
                    var actsWithDocumentFile = service.GetAll().Where(act => act.DocumentFile.Id == entity.DocumentFile.Id && act.Id != entity.Id).ToArray();

                    if (actsWithDocumentFile.Length > 0)
                    {
                        foreach (var performedWorkAct in actsWithDocumentFile)
                        {
                            performedWorkAct.DocumentFile = fileService.ReCreateFile(entity.DocumentFile);
                            service.Update(performedWorkAct);
                        }
                    }
                }
            }
            finally
            {
                Container.Release(fileService);
            }
        }
    }
}