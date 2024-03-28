Ext.define('B4.aspects.permission.ProtocolMhcState', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.protocolmhcstateperm',

    permissions: [
        /*
        * name - имя пермишена в дереве, 
        * applyTo - селектор контрола, к которому применяется пермишен, 
        * selector - селектор формы, на которой находится контрол
        */

        //поля панели редактирования
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Field.DocumentNum_Edit', applyTo: '#nfDocumentNum', selector: '#protocolMhcEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMhc.Field.DocumentNumber_Edit', applyTo: '#tfDocumentNumber', selector: '#protocolMhcEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.setReadOnly(false);
                    else component.setReadOnly(true);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMhc.Field.DocumentNum_View', applyTo: '#nfDocumentNum', selector: '#protocolMhcEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Field.DocumentYear_Edit', applyTo: '#nfDocumentYear', selector: '#protocolMhcEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.ProtocolMhc.Field.DocumentYear_View', applyTo: '#nfDocumentYear', selector: '#protocolMhcEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Field.DocumentSubNum_Edit', applyTo: '#nfDocumentSubNum', selector: '#protocolMhcEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Field.DocumentSubNum_View', applyTo: '#nfDocumentSubNum', selector: '#protocolMhcEditPanel',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },

        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Field.Municipality_Edit', applyTo: '#sfMunicipality', selector: '#protocolMhcEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Field.DateSupply_Edit', applyTo: '#dfDateSupply', selector: '#protocolMhcEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Field.ActCheck_Edit', applyTo: '#actCheckSelectField', selector: '#protocolMhcEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Field.Executant_Edit', applyTo: '#cbExecutant', selector: '#protocolMhcEditPanel' },

        //ArticleLaw
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Register.ArticleLaw.Create', applyTo: 'b4addbutton', selector: '#protocolMhcArticleLawGrid' },
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Register.ArticleLaw.Edit', applyTo: '#btnSaveArticles', selector: '#protocolMhcArticleLawGrid' },
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Register.ArticleLaw.Delete', applyTo: 'b4deletecolumn', selector: '#protocolMhcArticleLawGrid',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },

        //RealityObject
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Register.RealityObject.Create', applyTo: 'b4addbutton', selector: '#protocolMhcRealityObjectGrid' },
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Register.RealityObject.Delete', applyTo: 'b4deletecolumn', selector: '#protocolMhcRealityObjectGrid',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },

        //Annex
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Register.Annex.Create', applyTo: 'b4addbutton', selector: '#protocolMhcAnnexGrid' },
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Register.Annex.Edit', applyTo: 'b4savebutton', selector: '#protocolMhcAnnexEditWindow' },
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Register.Annex.Delete', applyTo: 'b4deletecolumn', selector: '#protocolMhcAnnexGrid',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },
        
        //Annex
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Register.Definition.Create', applyTo: 'b4addbutton', selector: '#protocolMhcDefinitionGrid' },
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Register.Definition.Edit', applyTo: 'b4savebutton', selector: '#protocolMhcDefinitionEditWindow' },
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Register.Definition.Delete', applyTo: 'b4deletecolumn', selector: '#protocolMhcDefinitionGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        }
    ]
});