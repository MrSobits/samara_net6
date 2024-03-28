Ext.define('B4.aspects.permission.DisposalNullInsp', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.disposalnullinspperm',

    permissions: [
        /*
        * name - имя пермишена в дереве, 
        * applyTo - селектор контрола, к которому применяется пермишен, 
        * selector - селектор формы, на которой находится контрол
        */

        //поля панели редактирования
        { name: 'GkhGji.DocumentsGji.DisposalNullInspection.Field.DocumentNum_Edit', applyTo: '#nfDocumentNum', selector: '#disposalNullInspEditWindow' },
        { name: 'GkhGji.DocumentsGji.DisposalNullInspection.Field.DocumentNum_Edit', applyTo: '#tfDocumentNumber', selector: '#disposalNullInspEditWindow' },
        { name: 'GkhGji.DocumentsGji.DisposalNullInspection.Field.DocumentNum_View', applyTo: '#nfDocumentNum', selector: '#disposalNullInspEditWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        
        { name: 'GkhGji.DocumentsGji.DisposalNullInspection.Field.DocumentYear_Edit', applyTo: '#nfDocumentYear', selector: '#disposalNullInspEditWindow' },
        { name: 'GkhGji.DocumentsGji.DisposalNullInspection.Field.DocumentYear_View', applyTo: '#nfDocumentYear', selector: '#disposalNullInspEditWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //{ name: 'GkhGji.DocumentsGji.DisposalNullInspection.Field.DocumentSubNum_Edit', applyTo: '#nfDocumentSubNum', selector: '#disposalNullInspEditWindow' },
        //{ name: 'GkhGji.DocumentsGji.DisposalNullInspection.Field.DocumentSubNum_View', applyTo: '#nfDocumentSubNum', selector: '#disposalNullInspEditWindow',
        //    applyBy: function (component, allowed) {
        //        if (allowed) component.show();
        //        else component.hide();
        //    }
        //},

        { name: 'GkhGji.DocumentsGji.DisposalNullInspection.Field.Description_Edit', applyTo: '#taDescription', selector: '#disposalNullInspEditWindow' },
        { name: 'GkhGji.DocumentsGji.DisposalNullInspection.Field.ResponsibleExecution_Edit', applyTo: '#sflRespExec', selector: '#disposalNullInspEditWindow' },
        { name: 'GkhGji.DocumentsGji.DisposalNullInspection.Field.IssuedDisposal_Edit', applyTo: '#sflIssued', selector: '#disposalNullInspEditWindow' },

        //DisposalAnnex
        { name: 'GkhGji.DocumentsGji.DisposalNullInspection.Register.Annex.Create', applyTo: 'b4addbutton', selector: '#disposalNullInspAnnexGrid' },
        { name: 'GkhGji.DocumentsGji.DisposalNullInspection.Register.Annex.Edit', applyTo: 'b4savebutton', selector: '#disposalNullInspAnnexEditWindow' },
        { name: 'GkhGji.DocumentsGji.DisposalNullInspection.Register.Annex.Delete', applyTo: 'b4deletecolumn', selector: '#disposalNullInspAnnexGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        }
    ]
});