Ext.define('B4.aspects.permission.Prescription', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.prescriptionperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */

        //поля панели редактирования
        { name: 'GkhGji.DocumentsGji.Prescription.Field.DocumentNum_Edit', applyTo: '#nfDocumentNum', selector: '#prescriptionEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Field.DocumentNumber_Edit', applyTo: '#tfDocumentNumber', selector: '#prescriptionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.setReadOnly(false);
                    else component.setReadOnly(true);
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.Prescription.Field.DocumentNum_View', applyTo: '#nfDocumentNum', selector: '#prescriptionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.Prescription.Field.DocumentYear_Edit', applyTo: '#nfDocumentYear', selector: '#prescriptionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Prescription.Field.DocumentYear_View', applyTo: '#nfDocumentYear', selector: '#prescriptionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.Prescription.Field.LiteralNum_Edit', applyTo: '#nfLiteralNum', selector: '#prescriptionEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Field.LiteralNum_View', applyTo: '#nfLiteralNum', selector: '#prescriptionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.Prescription.Field.DocumentSubNum_Edit', applyTo: '#nfDocumentSubNum', selector: '#prescriptionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Prescription.Field.DocumentSubNum_View', applyTo: '#nfDocumentSubNum', selector: '#prescriptionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.Prescription.Field.Inspectors_Edit', applyTo: '#prescriptionInspectorsTrigerField', selector: '#prescriptionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Prescription.Field.Description_Edit', applyTo: '#taDescriptionPrescription', selector: '#prescriptionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Prescription.Field.Executant_Edit', applyTo: '#cbExecutant', selector: '#prescriptionEditPanel' },

        //Violation
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Violation.Edit', applyTo: '#prescriptionViolationSaveButton', selector: '#prescriptionViolationGrid' },

        //ArticleLaw
        { name: 'GkhGji.DocumentsGji.Prescription.Register.ArticleLaw.View', applyTo: '#prescriptionArticleLawTab', selector: '#prescriptionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.tab.show();
                    else component.tab.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.ArticleLaw.Create', applyTo: 'b4addbutton', selector: '#prescriptionArticleLawGrid' },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.ArticleLaw.Edit', applyTo: '#prescriptionSaveButton', selector: '#prescriptionArticleLawGrid' },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.ArticleLaw.Delete', applyTo: 'b4deletecolumn', selector: '#prescriptionArticleLawGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //Cancel
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Cancel.Create', applyTo: 'b4addbutton', selector: '#prescriptionCancelGrid' },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Cancel.Edit', applyTo: 'b4savebutton', selector: '#prescriptionCancelEditWindow' },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Cancel.Delete', applyTo: 'b4deletecolumn', selector: '#prescriptionCancelGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.Prescription.Register.Cancel.DocumentDate', applyTo: '[name=DocumentDate]', selector: '#prescriptionCancelEditWindow' },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Cancel.DocumentNum', applyTo: '[name=DocumentNum]', selector: '#prescriptionCancelEditWindow' },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Cancel.DateCancel', applyTo: '[name=DateCancel]', selector: '#prescriptionCancelEditWindow' },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Cancel.IssuedCancel', applyTo: '[name=IssuedCancel]', selector: '#prescriptionCancelEditWindow' },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Cancel.IsCourt', applyTo: '[name=IsCourt]', selector: '#prescriptionCancelEditWindow' },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Cancel.Reason', applyTo: '[name=Reason]', selector: '#prescriptionCancelEditWindow' },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Register.Cancel.PetitionNum', applyTo: '[name=SmolPetitionNum]', selector: '#prescriptionCancelEditWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Register.Cancel.PetitionDate', applyTo: '[name=SmolPetitionDate]', selector: '#prescriptionCancelEditWindow',
                applyBy: function (component, allowed) {
                    if (component) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
        },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Cancel.DescriptionSet', applyTo: '[name=DescriptionSet]', selector: '#prescriptionCancelEditWindow' },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Cancel.CancelResult', applyTo: '[name=CancelResult]', selector: '#prescriptionCancelEditWindow' },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Cancel.TypeCancel', applyTo: '[name=TypeCancel]', selector: '#prescriptionCancelEditWindow' },


        //Annex
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Annex.Create', applyTo: 'b4addbutton', selector: '#prescriptionAnnexGrid' },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Annex.Edit', applyTo: 'b4savebutton', selector: '#prescriptionAnnexEditWindow' },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Annex.Delete', applyTo: 'b4deletecolumn', selector: '#prescriptionAnnexGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        }
    ]
});