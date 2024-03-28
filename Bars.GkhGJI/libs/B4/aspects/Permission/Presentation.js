Ext.define('B4.aspects.permission.Presentation', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.presentationperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */

        //поля панели редактирования
        {
            name: 'GkhGji.DocumentsGji.Presentation.Field.DocumentNumber_Edit', applyTo: '#tfDocumentNumber', selector: '#presentationEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.setReadOnly(false);
                    else component.setReadOnly(true);
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.Presentation.Field.DocumentNum_Edit', applyTo: '#nfDocumentNum', selector: '#presentationEditPanel' },
        { name: 'GkhGji.DocumentsGji.Presentation.Field.DocumentNum_View', applyTo: '#nfDocumentNum', selector: '#presentationEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.Presentation.Field.DocumentYear_Edit', applyTo: '#nfDocumentYear', selector: '#presentationEditPanel' },
        { name: 'GkhGji.DocumentsGji.Presentation.Field.DocumentYear_View', applyTo: '#nfDocumentYear', selector: '#presentationEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.Presentation.Field.LiteralNum_Edit', applyTo: '#nfLiteralNum', selector: '#presentationEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Presentation.Field.LiteralNum_View', applyTo: '#nfLiteralNum', selector: '#presentationEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.Presentation.Field.DocumentSubNum_Edit', applyTo: '#nfDocumentSubNum', selector: '#presentationEditPanel' },
        { name: 'GkhGji.DocumentsGji.Presentation.Field.DocumentSubNum_View', applyTo: '#nfDocumentSubNum', selector: '#presentationEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.Presentation.Field.TypeInitiativeOrg_Edit', applyTo: '#cbTypeInitiativeOrgpresentation', selector: '#presentationEditPanel' },
        { name: 'GkhGji.DocumentsGji.Presentation.Field.Official_Edit', applyTo: '#sfPresentationOfficial', selector: '#presentationEditPanel' },
        { name: 'GkhGji.DocumentsGji.Presentation.Field.Executant_Edit', applyTo: '#cbExecutant', selector: '#presentationEditPanel' },
        { name: 'GkhGji.DocumentsGji.Presentation.Field.Contragent_Edit', applyTo: '#sfContragent', selector: '#presentationEditPanel' },
        { name: 'GkhGji.DocumentsGji.Presentation.Field.PhysicalPerson_Edit', applyTo: '#tfPhysPerson', selector: '#presentationEditPanel' },
        { name: 'GkhGji.DocumentsGji.Presentation.Field.PhysicalPersonInfo_Edit', applyTo: '#taPhysPersonInfo', selector: '#presentationEditPanel' },

        //Annex
        { name: 'GkhGji.DocumentsGji.Presentation.Register.Annex.Create', applyTo: 'b4addbutton', selector: '#presentationAnnexGrid' },
        { name: 'GkhGji.DocumentsGji.Presentation.Register.Annex.Edit', applyTo: 'b4savebutton', selector: '#presentationAnnexEditWindow' },
        { name: 'GkhGji.DocumentsGji.Presentation.Register.Annex.Delete', applyTo: 'b4deletecolumn', selector: '#presentationAnnexGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        }
    ]
});