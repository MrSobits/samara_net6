namespace Bars.GkhGji.Regions.Saha.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Saha.Entities;
    using Bars.GkhGji.Regions.Saha.Entities.AppealCits;

    // Пустышка на случай если от этог окласса наследовались
    public class AppealCitsServiceInterceptor : Bars.GkhGji.Interceptors.AppealCitsServiceInterceptor
    {
        public override IDataResult AfterCreateAction(IDomainService<AppealCits> service, AppealCits entity)
        {
            TrackChange(entity);
            return base.AfterCreateAction(service, entity);
        }

        public override IDataResult AfterUpdateAction(IDomainService<AppealCits> service, AppealCits entity)
        {
            TrackChange(entity);
            return base.AfterUpdateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<AppealCits> service, AppealCits entity)
        {
            var servAppCitsExecutant = Container.Resolve<IDomainService<AppealCitsExecutant>>();
            var checkTimeDomain = Container.Resolve<IDomainService<CheckTimeChange>>();

            try
            {

                var executantIds = servAppCitsExecutant.GetAll()
                                        .Where(x => x.AppealCits.Id == entity.Id)
                                        .Select(x => x.Id)
                                        .AsEnumerable();

                foreach (var id in executantIds)
                {
                    servAppCitsExecutant.Delete(id);
                }

                var toDelete = checkTimeDomain.GetAll().Where(x => x.AppealCits.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var changeId in toDelete)
                {
                    checkTimeDomain.Delete(changeId);
                }


                return base.BeforeDeleteAction(service, entity);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                Container.Release(servAppCitsExecutant);
                Container.Release(checkTimeDomain);
            }
        }

        private void TrackChange(AppealCits entity)
        {
            var checkTimeDomain = Container.Resolve<IDomainService<CheckTimeChange>>();
            var userIdent = Container.Resolve<IUserIdentity>();
            var userDomain = Container.Resolve<IDomainService<User>>();

            try
            {
                var prev = checkTimeDomain.GetAll().Where(x => x.AppealCits.Id == entity.Id)
                .OrderByDescending(x => x.NewValue).FirstOrDefault();

                User user = null;
                if (!(userIdent is AnonymousUserIdentity))
                {
                    user = userDomain.FirstOrDefault(x => x.Id == userIdent.UserId);
                }
                
                if (prev == null ||  ( prev.NewValue != entity.CheckTime))
                {
                    var change = new CheckTimeChange()
                    {
                        AppealCits = entity,
                        NewValue = entity.CheckTime,
                        User = user
                    };

                    if (prev != null)
                    {
                        change.OldValue = prev.NewValue;
                    }

                    checkTimeDomain.Save(change);
                }
            }
            finally 
            {
                Container.Release(checkTimeDomain);
                Container.Release(userDomain);
            }
        }
    }
}