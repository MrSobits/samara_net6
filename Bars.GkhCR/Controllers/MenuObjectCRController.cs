namespace Bars.GkhCr.Controllers
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Controllers;
    using Bars.GkhCr.Entities;
    using Gkh.Domain;

    using Microsoft.AspNetCore.Mvc;

    public class MenuObjectCrController : MenuController
    {
        public ActionResult GetObjectCrMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            if (id > 0)
            {
                this.InitActiveOperatorAndRoles();

                var objectCr = this.Container.Resolve<IDomainService<Entities.ObjectCr>>().Get(id);

                this.InitStatePermissions(objectCr.State);

                var menuItems = this.FilterInacessibleStateItems(this.GetMenuItems("ObjectCr"));

                var monitoringSmr = this.Container.Resolve<IDomainService<MonitoringSmr>>().GetAll().FirstOrDefault(x => x.ObjectCr.Id == id);
                if (monitoringSmr != null && monitoringSmr.State != null)
                {
                    this.InitStatePermissions(monitoringSmr.State);

                    var monitoringCmpMenuItems = this.FilterInacessibleStateItems(this.GetMenuItems("MonitoringSmr"));

                    var monitoringSmrMenuItem = monitoringCmpMenuItems.FirstOrDefault();
                    if (monitoringSmrMenuItem != null)
                    {
                        var monSmrMainMenu = menuItems.FirstOrDefault(x => x.Caption == monitoringSmrMenuItem.Caption);
                        if (monSmrMainMenu != null)
                        {
                            monSmrMainMenu.Items = monitoringSmrMenuItem.Items;
                        }
                    }
                }

                return new JsonNetResult(menuItems);
            }

            return new JsonNetResult(null);
        }

        public ActionResult GetBankStatementMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            return id > 0 ? new JsonNetResult(this.GetMenuItems("BankStatement")) : new JsonNetResult(null);
        }
        public ActionResult GetCompetitionMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            return id > 0 ? new JsonNetResult(this.GetMenuItems("Competition")) : new JsonNetResult(null);
        }

        public ActionResult GetSpecialObjectCrMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            if (id > 0)
            {
                this.InitActiveOperatorAndRoles();

                var objectCr = this.Container.Resolve<IDomainService<SpecialObjectCr>>().Get(id);

                this.InitStatePermissions(objectCr.State);

                var menuItems = this.FilterInacessibleStateItems(this.GetMenuItems("SpecialObjectCr"));

                var monitoringSmr = this.Container.Resolve<IDomainService<SpecialMonitoringSmr>>().GetAll().FirstOrDefault(x => x.ObjectCr.Id == id);
                if (monitoringSmr != null && monitoringSmr.State != null)
                {
                    this.InitStatePermissions(monitoringSmr.State);

                    var monitoringCmpMenuItems = this.FilterInacessibleStateItems(this.GetMenuItems("SpecialMonitoringSmr"));

                    var monitoringSmrMenuItem = monitoringCmpMenuItems.FirstOrDefault();
                    if (monitoringSmrMenuItem != null)
                    {
                        var monSmrMainMenu = menuItems.FirstOrDefault(x => x.Caption == monitoringSmrMenuItem.Caption);
                        if (monSmrMainMenu != null)
                        {
                            monSmrMainMenu.Items = monitoringSmrMenuItem.Items;
                        }
                    }
                }

                return new JsonNetResult(menuItems);
            }

            return new JsonNetResult(null);
        }
    }
}