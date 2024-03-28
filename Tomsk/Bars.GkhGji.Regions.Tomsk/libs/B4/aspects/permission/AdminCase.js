Ext.define('B4.aspects.permission.AdminCase', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.admincaseperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Delete', applyTo: '#btnDelete', selector: '#adminCaseEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.AdminCase.Field.DocumentNumber_Edit',
            applyTo: '#tfDocumentNumber',
            selector: '#adminCaseEditPanel'
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Field.DocumentDate_Edit',
            applyTo: '[name=DocumentDate]',
            selector: '#adminCaseEditPanel'
        },
        
        {
            name: 'GkhGji.DocumentsGji.AdminCase.Details.TypeAdminCaseBase_View',
            applyTo: '[name=TypeAdminCaseBase]',
            selector: '#adminCaseEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.AdminCase.Details.TypeAdminCaseBase_Edit',
            applyTo: '[name=TypeAdminCaseBase]',
            selector: '#adminCaseEditPanel'
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Details.Inspector_View',
            applyTo: '[name=EntitiedInspector]',
            selector: '#adminCaseEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        
        {
            name: 'GkhGji.DocumentsGji.AdminCase.Details.Inspector_Edit',
            applyTo: '[name=EntitiedInspector]',
            selector: '#adminCaseEditPanel'
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Details.ParentDocument_View',
            applyTo: '[name=ParentDocument]',
            selector: '#adminCaseEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        
        {
            name: 'GkhGji.DocumentsGji.AdminCase.Details.RealityObject_View',
            applyTo: '[name=RealityObject]',
            selector: '#adminCaseEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Details.RealityObject_Edit',
            applyTo: '[name=RealityObject]',
            selector: '#adminCaseEditPanel'
        },
        
        {
            name: 'GkhGji.DocumentsGji.AdminCase.Details.Contragent_View',
            applyTo: '[name=Contragent]',
            selector: '#adminCaseEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Details.Contragent_Edit',
            applyTo: '[name=Contragent]',
            selector: '#adminCaseEditPanel'
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Details.DescriptionQuestion_View',
            applyTo: '[name=DescriptionQuestion]',
            selector: '#adminCaseEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Details.DescriptionQuestion_Edit',
            applyTo: '[name=DescriptionQuestion]',
            selector: '#adminCaseEditPanel'
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Details.DescriptionSet_View',
            applyTo: '[name=DescriptionSet]',
            selector: '#adminCaseEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Details.DescriptionSet_Edit',
            applyTo: '[name=DescriptionSet]',
            selector: '#adminCaseEditPanel'
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Details.DescriptionDefined_View',
            applyTo: '[name=DescriptionDefined]',
            selector: '#adminCaseEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Details.DescriptionDefined_Edit',
            applyTo: '[name=DescriptionDefined]',
            selector: '#adminCaseEditPanel'
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Register.ArticleLaw.Create',
            applyTo: 'b4addbutton',
            selector: 'admincasearticlelawgrid'
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Register.ArticleLaw.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'admincasearticlelawgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Register.Docs.Create',
            applyTo: 'b4addbutton',
            selector: 'admincasedocgrid'
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Register.Docs.Edit',
            applyTo: 'b4savebutton',
            selector: '#admincasedoceditwindow'
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Register.Docs.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'admincasedocgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Register.ProvidedDocs.Create',
            applyTo: 'b4addbutton',
            selector: 'admincaseprovdocgrid'
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Register.ProvidedDocs.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'admincaseprovdocgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Register.Annex.Create',
            applyTo: 'b4addbutton',
            selector: '#adminCaseAnnexGrid'
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Register.Annex.Edit',
            applyTo: 'b4savebutton',
            selector: '#adminCaseAnnexEditWindow'
        },

        {
            name: 'GkhGji.DocumentsGji.AdminCase.Register.Annex.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#adminCaseAnnexGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        }
    ]
});