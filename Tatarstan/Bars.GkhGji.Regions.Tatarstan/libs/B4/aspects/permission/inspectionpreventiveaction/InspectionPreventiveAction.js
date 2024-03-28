Ext.define('B4.aspects.permission.inspectionpreventiveaction.InspectionPreventiveAction', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.inspectionpreventiveactionperm',

    permissions: [
        {
            name: 'GkhGji.Inspection.InspectionPreventiveAction.Edit',
            applyTo: 'b4editcolumn',
            selector: 'inspectionpreventiveactionmainpanel inspectionpreventiveactiongrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.Inspection.InspectionPreventiveAction.Create',
            applyTo: 'b4addbutton',
            selector: 'inspectionpreventiveactionmainpanel inspectionpreventiveactiongrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.Inspection.InspectionPreventiveAction.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'inspectionpreventiveactionmainpanel inspectionpreventiveactiongrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.Inspection.InspectionPreventiveAction.ShowClosedInspections',
            applyTo: 'checkbox[name=IsClosed]',
            selector: 'inspectionpreventiveactionmainpanel inspectionpreventiveactiongrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
    ]
});