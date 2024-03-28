Ext.define('B4.aspects.permission.prescription.PrescriptionViolation', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.prescriptionviolationperm',

    init: function () {
        var me = this;
        me.permissions = [
            { name: 'GkhGji.DocumentsGji.Prescription.Register.Violation.Edit', applyTo: '#prescriptionViolationSaveButton', selector: '#prescriptionViolationGrid' },
            { name: 'GkhGji.DocumentsGji.Prescription.Register.Violation.Create', applyTo: '#btnAddObjViolation', selector: 'prescriptionRealObjListPanel' },
            { name: 'GkhGji.DocumentsGji.Prescription.Register.Violation.Delete', applyTo: 'b4deletecolumn', selector: '#prescriptionViolationGrid' },
            {
                name: 'GkhGji.DocumentsGji.Prescription.Register.Violation.View', applyTo: 'prescriptionRealObjListPanel', selector: '#prescriptionTabPanel',
                applyBy: function (component, allowed) {
                    var tabPanel = component.ownerCt;
                    if (allowed) {
                        tabPanel.showTab(component);
                    } else {

                        tabPanel.hideTab(component);
                    }
                }
            },
            //Violation columns
            {
                name: 'GkhGji.DocumentsGji.Prescription.Register.Violation.Fields.DateFactRemoval_Edit',
                applyTo: '[name=DateFactRemoval]', selector: '#prescriptionViolationGrid',
                applyBy: function (component, allowed) {
                    if (component.editor) {
                        component.editor.readOnly = !allowed;
                    }
                }
            },
        ];
        me.callParent(arguments);
    },

    loadPermissions: function () {
        var me = this;
        return B4.Ajax.request({
            url: B4.Url.action('/Permission/GetPermissions'),
            params: {
                permissions: Ext.encode(me.collectPermissions())
            }
        });
    },

    getGrants: function (grants) {
        return grants;
    },

    preDisable: function () {
        //переопределяем для предотвращения установки disabled readOnly полям
    }
});