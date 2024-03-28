Ext.define('B4.aspects.permission.inspectionactionisolated.InspectionActionIsolated', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.inspectionactionisolatedperm',

    permissions: [
        /*
        * name - имя пермишена в дереве, 
        * applyTo - селектор контрола, к которому применяется пермишен, 
        * selector - селектор формы, на которой находится контрол
        */
        {
            name: 'GkhGji.Inspection.InspectionActionIsolated.Edit',
            applyTo: 'b4editcolumn',
            selector: 'inspectionactionisolatedmainpanel inspectionactionisolatedgrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.Inspection.InspectionActionIsolated.Create',
            applyTo: 'b4addbutton',
            selector: 'inspectionactionisolatedmainpanel inspectionactionisolatedgrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.Inspection.InspectionActionIsolated.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'inspectionactionisolatedmainpanel inspectionactionisolatedgrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.Inspection.InspectionActionIsolated.ShowClosedInspections',
            applyTo: 'checkbox[name=IsClosed]',
            selector: 'inspectionactionisolatedmainpanel inspectionactionisolatedgrid',
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