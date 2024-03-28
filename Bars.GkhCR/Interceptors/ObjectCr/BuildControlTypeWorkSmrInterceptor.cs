namespace Bars.GkhCr.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhCr.Entities;

    public class BuildControlTypeWorkSmrInterceptor : EmptyDomainInterceptor<BuildControlTypeWorkSmr>
    {
        public IDomainService<TypeWorkCrAddWork> TypeWorkCrAddWorkDomain { get; set; }
        public IDomainService<AdditWork> AdditWorkDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<BuildControlTypeWorkSmr> service, BuildControlTypeWorkSmr entity)
        {
            var additWork = AdditWorkDomain.Get(entity.TypeWorkCrAddWork.AdditWork.Id);
            //проверка на очередность
            var queue = entity.TypeWorkCrAddWork.Queue;
          
            //Отключаем проверку на незавершенные этапы с меньшей очередностью
            //if (queue > 0)
            //{
            //    // ищем работы отслеживаемые стройконтролем с меньшей очередностью
            //    var typewotk = TypeWorkCrAddWorkDomain.GetAll()
            //        .Where(x => x.TypeWorkCr.Id == entity.TypeWorkCr.Id)
            //        .Where(x => x.Queue > 0 && x.Queue < queue)
            //        .Where(x => x.Required).ToList();
            //    if (typewotk != null)
            //    {
            //        foreach (var rec in typewotk)
            //        {
            //            var completed = service.GetAll()
            //                .Where(x => x.TypeWorkCrAddWork == rec)
            //                .Where(x => Convert.ToInt32(x.PercentOfCompletion) == 100).FirstOrDefault();
            //            if (completed == null)
            //            {
            //                return Failure($"Обнаружен не завершенный этап {rec.AdditWork.Name}, который должен быть выполнен до {additWork.Name}");
            //            }
            //        }
            //    }
            //}

            if (entity.DeadlineMissed)
            {
                var crRepo = this.Container.Resolve<IRepository<ObjectCr>>();
                var crObj = crRepo.Get(entity.TypeWorkCr.ObjectCr.Id);
                // TODO: Найти поле
                //crObj.DeadlineMissed = entity.DeadlineMissed;
                crRepo.Update(crObj);
            }

            // Перед сохранением проставляем начальный статус
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<BuildControlTypeWorkSmr> service, BuildControlTypeWorkSmr entity)
        {
            if (entity.PercentOfCompletion > 0)
            {
                var workRepo =  this.Container.Resolve<IRepository<TypeWorkCr>>();
                try
                {
                    var worklist = service.GetAll().Where(x => x.TypeWorkCr == entity.TypeWorkCr)
                       .Select(x => new
                       {
                           x.PercentOfCompletion,
                           x.TypeWorkCrAddWork.AdditWork.Percentage
                       }).ToList();
                    decimal perc = 0;
                    foreach (var v in worklist)
                    {
                        if (v.Percentage.HasValue)
                        {
                            perc += v.PercentOfCompletion * v.Percentage.Value;
                            perc = Decimal.Round(perc, 2);

                        }
                    }
                    var work = workRepo.Get(entity.TypeWorkCr.Id);
                    work.PercentOfCompletion = perc;
                    workRepo.Update(work);
                }
                catch
                {
                    
                }
            }
            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<BuildControlTypeWorkSmr> service, BuildControlTypeWorkSmr entity)
        {            
            try
            {

                if (entity.DeadlineMissed)
                {
                    var crRepo = this.Container.Resolve<IRepository<ObjectCr>>();
                    var crObj = crRepo.Get(entity.TypeWorkCr.ObjectCr.Id);
                    // TODO: Найти поле
                    //crObj.DeadlineMissed = entity.DeadlineMissed;
                    crRepo.Update(crObj);
                }
                if (entity.PercentOfCompletion > 0)
                {
                    var workRepo = this.Container.Resolve<IRepository<TypeWorkCr>>();
                    try
                    {
                        var worklist = service.GetAll().Where(x => x.TypeWorkCr == entity.TypeWorkCr)
                           .Select(x => new
                           {
                               x.PercentOfCompletion,
                               x.TypeWorkCrAddWork.AdditWork.Percentage
                           }).ToList();
                        decimal perc = 0;
                        foreach (var v in worklist)
                        {
                            if (v.Percentage.HasValue)
                            {
                                perc += v.PercentOfCompletion * v.Percentage.Value/100;
                                perc = Decimal.Round(perc, 2);

                            }
                        }
                        var work = workRepo.Get(entity.TypeWorkCr.Id);
                        work.PercentOfCompletion = perc;
                        workRepo.Update(work);
                    }
                    catch
                    {

                    }
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure("Ошибка " + e.Message);
            }
           
        }

        public override IDataResult BeforeDeleteAction(IDomainService<BuildControlTypeWorkSmr> service, BuildControlTypeWorkSmr entity)
        {
            var BuildControlTypeWorkSmrFileService = Container.Resolve<IDomainService<BuildControlTypeWorkSmrFile>>();
            if (entity.ObjectCreateDate.AddDays(1) < DateTime.Now)
            {
                var userManager = Container.Resolve<IGkhUserManager>();
                Operator operatorChanger = userManager.GetActiveOperator();
                User userChanger = operatorChanger.User;
                var changerRoles = userChanger.Roles;
                if (changerRoles != null && changerRoles.Count == 1)
                {
                    Role changerRole = changerRoles.FirstOrDefault().Role;
                    if (changerRole != null)
                    {
                        if (!changerRole.Name.ToLower().Contains("админ"))
                        {
                            return Failure("Удаление отчета запрещено");
                        }

                    }


                }
               
            }
            try
            {

                var buildControlTypeWorkSmrFileList = BuildControlTypeWorkSmrFileService.GetAll().Where(x => x.BuildControlTypeWorkSmr.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in buildControlTypeWorkSmrFileList)
                {
                    BuildControlTypeWorkSmrFileService.Delete(value);
                }


                return Success();
            }
            catch (Exception e)
            {
                return Failure("Ошибка удаления работ " + e.Message);
            }
            finally
            {
                Container.Release(BuildControlTypeWorkSmrFileService);
            }
        }
    }
}