Ext.define('B4.aspects.permission.ProtocolRSOState', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.protocolrsostateperm',

    permissions: [
        /*
        * name - имя пермишена в дереве, 
        * applyTo - селектор контрола, к которому применяется пермишен, 
        * selector - селектор формы, на которой находится контрол
        */

        //поля панели редактирования
        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Field.DocumentNum_Edit', applyTo: '#nfDocumentNum', selector: '#protocolRSOEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.ProtocolRSO.Field.DocumentNumber_Edit', applyTo: '#tfDocumentNumber', selector: '#protocolRSOEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.setReadOnly(false);
                    else component.setReadOnly(true);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ProtocolRSO.Field.DocumentNum_View', applyTo: '#nfDocumentNum', selector: '#protocolRSOEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Field.DocumentYear_Edit', applyTo: '#nfDocumentYear', selector: '#protocolRSOEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.ProtocolRSO.Field.DocumentYear_View', applyTo: '#nfDocumentYear', selector: '#protocolRSOEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Field.DocumentSubNum_Edit', applyTo: '#nfDocumentSubNum', selector: '#protocolRSOEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.ProtocolRSO.Field.DocumentSubNum_View', applyTo: '#nfDocumentSubNum', selector: '#protocolRSOEditPanel',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },

        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Field.SupplierGas_Edit', applyTo: '#sfSupplierGas', selector: '#protocolRSOEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Field.DateSupply_Edit', applyTo: '#dfDateSupply', selector: '#protocolRSOEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Field.ActCheck_Edit', applyTo: '#actCheckSelectField', selector: '#protocolRSOEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Field.Executant_Edit', applyTo: '#cbExecutant', selector: '#protocolRSOEditPanel' },

        //ArticleLaw
        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Register.ArticleLaw.Create', applyTo: 'b4addbutton', selector: '#protocolRSOArticleLawGrid' },
        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Register.ArticleLaw.Edit', applyTo: '#btnSaveArticles', selector: '#protocolRSOArticleLawGrid' },
        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Register.ArticleLaw.Delete', applyTo: 'b4deletecolumn', selector: '#protocolRSOArticleLawGrid',
        applyBy: function (component, allowed) {
            if (component) {
                debugger;
                if (allowed) component.show();
                else component.hide();
            }
        }
        },

        //RealityObject
        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Register.RealityObject.Create', applyTo: 'b4addbutton', selector: '#protocolRSORealityObjectGrid' },
        {name: 'GkhGji.DocumentsGji.ProtocolRSO.Register.RealityObject.Delete', applyTo: 'b4deletecolumn', selector: '#protocolRSORealityObjectGrid',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.show();
            }
        }
        },

        //Annex
        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Register.Annex.Create', applyTo: 'b4addbutton', selector: '#protocolRSOAnnexGrid' },
        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Register.Annex.Edit', applyTo: 'b4savebutton', selector: '#protocolRSOAnnexEditWindow' },
        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Register.Annex.Delete', applyTo: 'b4deletecolumn', selector: '#protocolRSOAnnexGrid',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },
        
        //Annex
        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Register.Definition.Create', applyTo: 'b4addbutton', selector: '#protocolRSODefinitionGrid' },
        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Register.Definition.Edit', applyTo: 'b4savebutton', selector: '#protocolRSODefinitionEditWindow' },
        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Register.Definition.Delete', applyTo: 'b4deletecolumn', selector: '#protocolRSODefinitionGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        }
    ]
});