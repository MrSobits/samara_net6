Ext.define('B4.aspects.permission.inspectionactionisolated.EditPanel', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.inspectionactionisolatededitpanelperm',

    permissions: [
        /*
        * name - имя пермишена в дереве, 
        * applyTo - селектор контрола, к которому применяется пермишен, 
        * selector - селектор формы, на которой находится контрол
        */
        { name: 'GkhGji.Inspection.InspectionActionIsolated.Edit', applyTo: 'b4savebutton', selector: '#inspectionActionIsolatedEditPanel' },
        { name: 'GkhGji.Inspection.InspectionActionIsolated.Delete', applyTo: '#btnDelete', selector: '#inspectionActionIsolatedEditPanel'},

        //TODO Временно ограничил функционал кнопок
        { name: 'GkhGji.Inspection.InspectionActionIsolated.Edit', applyTo: 'gkhbuttonprint', selector: '#inspectionActionIsolatedEditPanel' },
        { name: 'GkhGji.Inspection.InspectionActionIsolated.Edit', applyTo: '#btnState', selector: '#inspectionActionIsolatedEditPanel' },

        {
            name: 'GkhGji.Inspection.InspectionActionIsolated.RealityObject.Create',
            applyTo: 'b4addbutton',
            selector: 'inspectionActionIsolatedRealityObjGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhGji.Inspection.InspectionActionIsolated.RealityObject.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'inspectionActionIsolatedRealityObjGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhGji.Inspection.InspectionActionIsolated.JointInspection.Create',
            applyTo: 'b4addbutton',
            selector: 'inspectionActionIsolatedJointInspectionGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhGji.Inspection.InspectionActionIsolated.JointInspection.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'inspectionActionIsolatedJointInspectionGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});