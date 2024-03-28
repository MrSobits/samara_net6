namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using Entities;
    using System;
    using System.Collections.Generic;
    using Enums;
    using Bars.GkhGji.Enums;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using System.Linq;
    using B4.Modules.States;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.B4.DataAccess;

    class AppealOrderInterceptor : EmptyDomainInterceptor<AppealOrder>
    {

        public IDomainService<AppealOrderFile> AppealOrderFileDomain { get; set; }
        public IDomainService<State> StateDomain { get; set; }

        public override IDataResult BeforeUpdateAction(IDomainService<AppealOrder> service, AppealOrder entity)
        {
            try
            {
                if (entity.YesNoNotSet == Gkh.Enums.YesNoNotSet.Yes)
                {
                    var proofs = AppealOrderFileDomain.GetAll().FirstOrDefault(x => x.AppealOrder == entity);
                    if (proofs != null)
                    {

                    }
                    else
                    {
                        return Failure("Не прикреплено ни одного файла");
                    }
                }


                return Success();
            }
            catch (Exception e)
            {
                return Failure("Не удалось сохранить задачу");
            }
        }

        public override IDataResult AfterUpdateAction(IDomainService<AppealOrder> service, AppealOrder entity)
        {
            try
            {
                if (entity.YesNoNotSet == Gkh.Enums.YesNoNotSet.Yes)
                {
                    var appealRepo = this.Container.Resolve<IRepository<Bars.GkhGji.Entities.AppealCits>>();
                    var appeal = appealRepo.Get(entity.AppealCits.Id);
                    if (appeal != null && appeal.State.Code == "СОПР")
                    {
                        var sopr2State = StateDomain.GetAll()
                            .Where(x => x.Code == "СОПР2").FirstOrDefault();
                        if (sopr2State != null)
                        {
                            appeal.State = sopr2State;
                            appealRepo.Update(appeal);
                        }

                    }
                }
            

                return Success();
            }
            catch (Exception e)
            {
                return Failure("Не удалось сохранить задачу");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<AppealOrder> service, AppealOrder entity)
        {
            try
            {
               
                    var proofs = AppealOrderFileDomain.GetAll().FirstOrDefault(x => x.AppealOrder == entity);
                    if (proofs != null)
                    {
                         return Failure("В документе есть вложения, размещенные УК. Удаление запрещено");
                    }
                return Success();
            }
            catch (Exception e)
            {
                return Failure("При удалении произошла ошибка");
            }
        }

    }
}
