Ext.define('B4.aspects.permission.planworkservicerepair.State', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.planworkservicerepairstateperm',

    permissions: [
        // План работ
        { name: 'GkhDi.DisinfoRealObj.PlanWorkServiceRepair.Add', applyTo: 'b4addbutton', selector: '#planWorkServiceRepairGrid' },
        { name: 'GkhDi.DisinfoRealObj.PlanWorkServiceRepair.Delete', applyTo: 'b4deletecolumn', selector: '#planWorkServiceRepairGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        
        // Работы
        { name: 'GkhDi.DisinfoRealObj.PlanWorkServiceRepair.Work.Load', applyTo: '#planWorkServiceRepairWorksReloadButton', selector: '#planWorkServiceRepairWorksGrid' },
        { name: 'GkhDi.DisinfoRealObj.PlanWorkServiceRepair.Work.Save', applyTo: 'b4savebutton', selector: '#planWorkServiceRepairWorksEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.PlanWorkServiceRepair.Work.Delete', applyTo: 'b4deletecolumn', selector: '#planWorkServiceRepairWorksGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});