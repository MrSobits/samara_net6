Ext.define('B4.aspects.permission.ChelyabinskPrescription', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.nsoprescriptionperm',

    permissions: [
        { name: 'GkhGji.DocumentsGji.Prescription.Field.DocumentPlace_Edit', applyTo: '[name=DocumentPlace]', selector: 'prescriptionEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Field.DocumentPlace_View', applyTo: '[name=DocumentPlace]', selector: 'prescriptionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.Prescription.Field.DocumentTime_Edit', applyTo: '[name=DocumentTime]', selector: 'prescriptionEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Field.DocumentTime_View', applyTo: '[name=DocumentTime]', selector: 'prescriptionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.BaseDocument.Create', applyTo: 'b4addbutton', selector: 'prescriptionBaseDocumentGrid' },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.BaseDocument.Edit', applyTo: 'b4savebutton', selector: 'prescriptionBaseDocumentEditWindow' },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Register.BaseDocument.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'prescriptionBaseDocumentGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Violation.Create', applyTo: '#btnAddObjViolation', selector: 'prescriptionRealObjViolGrid' },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Violation.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'prescriptionViolationGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        }
    ]
});