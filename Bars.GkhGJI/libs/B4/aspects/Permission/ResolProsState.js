Ext.define('B4.aspects.permission.ResolProsState', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.resolprosstateperm',

    permissions: [
        /*
        * name - имя пермишена в дереве, 
        * applyTo - селектор контрола, к которому применяется пермишен, 
        * selector - селектор формы, на которой находится контрол
        */

        //поля панели редактирования
        { name: 'GkhGji.DocumentsGji.ResolPros.Field.DocumentNum_Edit', applyTo: '#nfDocumentNum', selector: '#resolProsEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.ResolPros.Field.DocumentNumber_Edit', applyTo: '#tfDocumentNumber', selector: '#resolProsEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.setReadOnly(false);
                    else component.setReadOnly(true);
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ResolPros.Field.DocumentNum_View', applyTo: '#nfDocumentNum', selector: '#resolProsEditPanel',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },

        { name: 'GkhGji.DocumentsGji.ResolPros.Field.DocumentYear_Edit', applyTo: '#nfDocumentYear', selector: '#resolProsEditPanel' },
        { name: 'GkhGji.DocumentsGji.ResolPros.Field.DocumentYear_View', applyTo: '#nfDocumentYear', selector: '#resolProsEditPanel',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },
        {
            name: 'GkhGji.DocumentsGji.ResolPros.Documents.Resolution', applyTo: 'gjidocumentcreatebutton', selector: '#resolProsEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.ResolPros.Field.DocumentSubNum_Edit', applyTo: '#nfDocumentSubNum', selector: '#resolProsEditPanel' },
        { name: 'GkhGji.DocumentsGji.ResolPros.Field.DocumentSubNum_View', applyTo: '#nfDocumentSubNum', selector: '#resolProsEditPanel',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },

        { name: 'GkhGji.DocumentsGji.ResolPros.Field.Municipality_Edit', applyTo: '#sfMunicipalityResolPros', selector: '#resolProsEditPanel' },
        { name: 'GkhGji.DocumentsGji.ResolPros.Field.DateSupply_Edit', applyTo: '#dfDateSupplyResolPros', selector: '#resolProsEditPanel' },
        { name: 'GkhGji.DocumentsGji.ResolPros.Field.ActCheck_Edit', applyTo: '#actCheckSelectField', selector: '#resolProsEditPanel' },
        { name: 'GkhGji.DocumentsGji.ResolPros.Field.Executant_Edit', applyTo: '#cbExecutant', selector: '#resolProsEditPanel' },

        //ArticleLaw
        { name: 'GkhGji.DocumentsGji.ResolPros.Register.ArticleLaw.Create', applyTo: 'b4addbutton', selector: '#resolProsArticleLawGrid' },
        { name: 'GkhGji.DocumentsGji.ResolPros.Register.ArticleLaw.Edit', applyTo: '#btnSaveArticles', selector: '#resolProsArticleLawGrid' },
        { name: 'GkhGji.DocumentsGji.ResolPros.Register.ArticleLaw.Delete', applyTo: 'b4deletecolumn', selector: '#resolProsArticleLawGrid',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },

        //RealityObject
        { name: 'GkhGji.DocumentsGji.ResolPros.Register.RealityObject.Create', applyTo: 'b4addbutton', selector: '#resolProsRealityObjectGrid' },
        { name: 'GkhGji.DocumentsGji.ResolPros.Register.RealityObject.Delete', applyTo: 'b4deletecolumn', selector: '#resolProsRealityObjectGrid',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },

        //Annex
        { name: 'GkhGji.DocumentsGji.ResolPros.Register.Annex.Create', applyTo: 'b4addbutton', selector: '#resolProsAnnexGrid' },
        { name: 'GkhGji.DocumentsGji.ResolPros.Register.Annex.Edit', applyTo: 'b4savebutton', selector: '#resolProsAnnexEditWindow' },
        { name: 'GkhGji.DocumentsGji.ResolPros.Register.Annex.Delete', applyTo: 'b4deletecolumn', selector: '#resolProsAnnexGrid',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        }
    ]
});