/*
Перекрывается в модуле GkhGji.Regions.Smolensk
*/

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
            name: 'GkhGji.DocumentsGji.Prescription.Field.DocumentNumber_Edit',
            applyTo: '#tfDocumentNumber',
            selector: '#prescriptionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) {
                        component.setReadOnly(false);
                    } else {
                        component.setReadOnly(true);
                    }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Field.DocumentNum_View',
            applyTo: '#nfDocumentNum',
            selector: '#prescriptionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.Prescription.Field.DocumentYear_Edit', applyTo: '#nfDocumentYear', selector: '#prescriptionEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Field.DocumentYear_View',
            applyTo: '#nfDocumentYear',
            selector: '#prescriptionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
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
        {
            name: 'GkhGji.DocumentsGji.Prescription.Field.DocumentSubNum_View',
            applyTo: '#nfDocumentSubNum',
            selector: '#prescriptionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.Prescription.Field.Inspectors_Edit', applyTo: '#prescriptionInspectorsTrigerField', selector: '#prescriptionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Prescription.Field.Description_Edit', applyTo: '#taDescriptionPrescription', selector: '#prescriptionEditPanel' },
        { name: 'GkhGji.DocumentsGji.Prescription.Field.Executant_Edit', applyTo: '#cbExecutant', selector: '#prescriptionEditPanel' },

        {
            name: 'GkhGji.DocumentsGji.Prescription.Register.Requisites.View',  applyTo: '#prescriptionRequisitesTab', selector: '#prescriptionTabPanel',
            applyBy: function (component, allowed) {
                var tabPanel = component.ownerCt;
                if (allowed) {
                    tabPanel.showTab(component);
                } else {
                    
                    tabPanel.hideTab(component);
                }
            }
        },

        //ArticleLaw
        {
            name: 'GkhGji.DocumentsGji.Prescription.Register.ArticleLaw.View',
            applyTo: '#prescriptionArticleLawTab',
            selector: '#prescriptionEditPanel',
            applyBy: function (component, allowed) {
                var tabPanel = component.ownerCt;
                if (allowed) {
                    tabPanel.showTab(component);
                } else {
                    
                    tabPanel.hideTab(component);
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.ArticleLaw.Create', applyTo: 'b4addbutton', selector: '#prescriptionArticleLawGrid' },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.ArticleLaw.Edit', applyTo: '#prescriptionSaveButton', selector: '#prescriptionArticleLawGrid' },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Register.ArticleLaw.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#prescriptionArticleLawGrid',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },

        //Cancel
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Cancel.Create', applyTo: 'b4addbutton', selector: '#prescriptionCancelGrid' },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Cancel.Edit', applyTo: 'b4savebutton', selector: '#prescriptionCancelEditWindow' },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Register.Cancel.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#prescriptionCancelGrid',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Register.Cancel.View',  applyTo: '#prescriptionCancelGrid', selector: '#prescriptionTabPanel',
            applyBy: function (component, allowed) {
                var tabPanel = component.ownerCt;
                if (allowed) {
                    tabPanel.showTab(component);
                } else {
                    
                    tabPanel.hideTab(component);
                }
            }
        },

        //Annex
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Annex.Create', applyTo: 'b4addbutton', selector: '#prescriptionAnnexGrid' },
        { name: 'GkhGji.DocumentsGji.Prescription.Register.Annex.Edit', applyTo: 'b4savebutton', selector: '#prescriptionAnnexEditWindow' },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Register.Annex.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#prescriptionAnnexGrid',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Register.Annex.View',  applyTo: 'prescriptionAnnexGrid', selector: '#prescriptionTabPanel',
            applyBy: function (component, allowed) {
                var tabPanel = component.ownerCt;
                if (allowed) {
                    tabPanel.showTab(component);
                } else {
                    
                    tabPanel.hideTab(component);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Register.Directions.View',  applyTo: 'protocolgjiRequisitePanel', selector: '#protocolTabPanel',
            applyBy: function (component, allowed) {
                var tabPanel = component.ownerCt;
                if (allowed) {
                    tabPanel.showTab(component);
                } else {
                    
                    tabPanel.hideTab(component);
                }
            }
        },
        //Close

        //панель prescrclosepanel не используется в EditPanel для nso
        //хотя в общем модуле она используется,
        //поэтому идет проверка на существование component'а.
        //возможно, что лучше создать в nso свой аспект для пермишенов и убрать лишние,
        //но странно то, что регионы (не нсо) используют проект nso в качестве общего

        {
            name: 'GkhGji.DocumentsGji.Prescription.Register.Close.View',
            applyTo: 'prescrclosepanel',
            selector: '#prescriptionEditPanel',
            applyBy: function (component, allowed) {
                var tabPanel = component.ownerCt;
                if (allowed) {
                    tabPanel.showTab(component);
                } else {
                    
                    tabPanel.hideTab(component);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Register.Close.Docs.Edit',
            applyTo: 'prescrclosepanel b4grid b4editcolumn',
            selector: '#prescriptionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.setVisible(true);
                    } else {
                        component.setVisible(false);
                    }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Register.Close.Docs.Create',
            applyTo: 'prescrclosepanel b4grid b4addbutton',
            selector: '#prescriptionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.setVisible(true);
                    } else {
                        component.setVisible(false);
                    }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Register.Close.Docs.Delete',
            applyTo: 'prescrclosepanel b4grid b4deletecolumn',
            selector: '#prescriptionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.setVisible(true);
                    } else {
                        component.setVisible(false);
                    }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Field.Close.Closed_Edit',
            applyTo: 'prescrclosepanel b4enumcombo[name=Closed]',
            selector: '#prescriptionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.enable();
                    } else {
                        component.disable();
                    }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Field.Close.Reason_Edit',
            applyTo: 'prescrclosepanel b4enumcombo[name=CloseReason]',
            selector: '#prescriptionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.enable();
                    } else {
                        component.disable();
                    }
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Prescription.Field.Close.Note_Edit',
            applyTo: 'prescrclosepanel textarea[name=CloseNote]',
            selector: '#prescriptionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.enable();
                    } else {
                        component.disable();
                    }
                }
            }
        }
    ]
});