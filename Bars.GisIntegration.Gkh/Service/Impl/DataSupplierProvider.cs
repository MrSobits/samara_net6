namespace Bars.GisIntegration.Gkh.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Service;
    using Bars.GisIntegration.Base.Service.Impl;
    using Bars.Gkh.Authentification;

    using Castle.Windsor;

    public class DataSupplierProvider : IDataSupplierProvider
    {
        public IWindsorContainer Container { get; set; }

        public RisContragent GetCurrentDataSupplier()
        {
            if (DataSupplierContext.Current != null)
            {
                return DataSupplierContext.Current.DataSupplier;
            }

            var userManager = this.Container.Resolve<IGkhUserManager>();
            var contragentDomain = this.Container.ResolveDomain<RisContragent>();

            try
            {
                var currentContragentIds = userManager.GetContragentIds();

                if (currentContragentIds == null || currentContragentIds.Count == 0)
                {
                    throw new Exception("К учетной записи не привязан контрагент");
                }
                else if (currentContragentIds.Count > 1)
                {
                    throw new Exception("К учетной записи привязано более 1 контрагента.");
                }

                var risContragent = contragentDomain.GetAll().FirstOrDefault(x => x.GkhId == currentContragentIds[0]);

                if (risContragent == null)
                {
                    throw new Exception("Не получены данные о зарегистрированной организации в ГИС.");
                }

                if (string.IsNullOrEmpty(risContragent.OrgPpaGuid))
                {
                    throw new Exception("Не получен идентификатор зарегистрированной организации в ГИС.");
                }

                return risContragent;
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(contragentDomain);
            }
        }

        public bool IsDelegacy(RisContragent contragent)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

            try
            {
                return userManager.GetContragentIds().Contains(contragent.GkhId);
            }
            finally
            {
                this.Container.Release(userManager);
            }
        }

        public List<long> GetContragentIds()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var contragentDomain = this.Container.ResolveDomain<RisContragent>();

            try
            {
                var currentContragentIds = userManager.GetContragentIds();

                if (currentContragentIds == null || currentContragentIds.Count == 0)
                {
                    return new List<long>();
                }

                return contragentDomain.GetAll().WhereContains(x => x.GkhId, currentContragentIds).Select(x => x.Id).ToList();
            }
            finally
            {
                this.Container.Release(contragentDomain);
                this.Container.Release(userManager);
            }
        }
    }
}