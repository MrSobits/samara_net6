Ext.define('B4.aspects.permission.ActCheck', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.actcheckperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */

    //поля панели редактирования ActCheck
        {
            name: 'GkhGji.DocumentsGji.ActCheck.Field.DocumentNumber_Edit', applyTo: '#tfDocumentNumber', selector: '#actCheckEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.setReadOnly(false);
                    else component.setReadOnly(true);
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.DocumentNum_Edit', applyTo: '#nfDocumentNum', selector: '#actCheckEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.ActCheck.Field.DocumentNum_View', applyTo: '#nfDocumentNum', selector: '#actCheckEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.ActCheck.Field.DocumentPlace_Edit', applyTo: '#tfDocumentPlace', selector: '#actCheckEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.ActCheck.Field.DocumentPlace_View', applyTo: '#tfDocumentPlace', selector: '#actCheckEditPanel',
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
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.DocumentTime_Edit', applyTo: '#tfDocumentTime', selector: '#actCheckEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.ActCheck.Field.DocumentTime_View', applyTo: '#tfDocumentTime', selector: '#actCheckEditPanel',
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

        { name: 'GkhGji.DocumentsGji.ActCheck.Field.DocumentYear_Edit', applyTo: '#nfDocumentYear', selector: '#actCheckEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.DocumentYear_View', applyTo: '#nfDocumentYear', selector: '#actCheckEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.ActCheck.Field.LiteralNum_Edit', applyTo: '#nfLiteralNum', selector: '#actCheckEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.ActCheck.Field.LiteralNum_View', applyTo: '#nfLiteralNum', selector: '#actCheckEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.DocumentSubNum_Edit', applyTo: '#nfDocumentSubNum', selector: '#actCheckEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.DocumentSubNum_View', applyTo: '#nfDocumentSubNum', selector: '#actCheckEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.DocumentDate_Edit', applyTo: '#dfDocumentDate', selector: '#actCheckEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.Inspectors_Edit', applyTo: '#trigfInspectors', selector: '#actCheckEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.Area_Edit', applyTo: '#nfArea', selector: '#actCheckEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.ActCheck.Field.Area_View', applyTo: '#nfArea', selector: '#actCheckEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.Flat_Edit', applyTo: '#tfFlat', selector: '#actCheckEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.Flat_View', applyTo: '#tfFlat', selector: '#actCheckEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.ToProsecutor_Edit', applyTo: '#cbToPros', selector: '#actCheckEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.DateToProsecutor_Edit', applyTo: '#dfToPros', selector: '#actCheckEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.ResolutionProsecutor_Edit', applyTo: '#sfResolPros', selector: '#actCheckEditPanel' },

        //ActCheckAnnex
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Annex.Create', applyTo: 'b4addbutton', selector: '#actCheckAnnexGrid' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Annex.Edit', applyTo: 'b4savebutton', selector: '#actCheckAnnexEditWindow' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Annex.Delete', applyTo: 'b4deletecolumn', selector: '#actCheckAnnexGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ActCheck.Register.Annex.View', applyTo: '#actCheckAnnexGrid', selector: '#actCheckTabPanel',
            applyBy: function (component, allowed) {
                var tabPanel = component.ownerCt;
                if (allowed) {
                    tabPanel.showTab(component);
                } else {
                    
                    tabPanel.hideTab(component);
                }
            }
        },

        //ActCheckAnnex
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.ProvidedDoc.Create', applyTo: 'b4addbutton', selector: 'actcheckprovideddocgrid' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.ProvidedDoc.Edit', applyTo: 'b4savebutton', selector: 'actcheckprovideddocgrid' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.ProvidedDoc.Delete', applyTo: 'b4deletecolumn', selector: 'actcheckprovideddocgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.ProvidedDoc.View', applyTo: 'actcheckprovideddocgrid', selector: '#actCheckTabPanel',
            applyBy: function (component, allowed) {
                var tabPanel = component.ownerCt;
                if (allowed) {
                    tabPanel.showTab(component);
                } else {
                    
                    tabPanel.hideTab(component);
                }
            }
        },

        //ActCheckInspectedPart
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.InspectedPart.Create', applyTo: 'b4addbutton', selector: '#actCheckInspectedPartGrid' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.InspectedPart.Edit', applyTo: 'b4savebutton', selector: '#actCheckInspectedPartEditWindow' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.InspectedPart.Delete', applyTo: 'b4deletecolumn', selector: '#actCheckInspectedPartGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.InspectedPart.View', applyTo: 'actCheckInspectedPartGrid', selector: '#actCheckTabPanel',
            applyBy: function (component, allowed) {
                var tabPanel = component.ownerCt;
                if (allowed) {
                    tabPanel.showTab(component);
                } else {
                    
                    tabPanel.hideTab(component);
                }
            }
        },

        //ActCheckDefinition
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Definition.Create', applyTo: 'b4addbutton', selector: 'actCheckDefinitionGrid' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Definition.Edit', applyTo: 'b4savebutton', selector: '#actCheckDefinitionEditWindow' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Definition.Delete', applyTo: 'b4deletecolumn', selector: 'actCheckDefinitionGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Definition.View', applyTo: 'actCheckDefinitionGrid', selector: '#actCheckTabPanel',
            applyBy: function (component, allowed) {
                var tabPanel = component.ownerCt;
                if (allowed) {
                    tabPanel.showTab(component);
                } else {
                    
                    tabPanel.hideTab(component);
                }
            }
        },

        //ActCheckPeriod
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Period.Create', applyTo: 'b4addbutton', selector: '#actCheckPeriodGrid' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Period.Edit', applyTo: 'b4savebutton', selector: '#actCheckPeriodEditWindow' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Period.Delete', applyTo: 'b4deletecolumn', selector: '#actCheckPeriodGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Period.View', applyTo: '#actCheckPeriodGrid', selector: '#actCheckTabPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //ActCheckViolation
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Violation.Create', applyTo: 'b4addbutton', selector: '#actCheckViolationGrid' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Violation.Delete', applyTo: 'b4deletecolumn', selector: '#actCheckViolationGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Violation.View', applyTo: '#actCheckViolationTab', selector: '#actCheckTabPanel',
            applyBy: function (component, allowed) {
                var tabPanel = component.ownerCt;
                if (allowed) {
                    tabPanel.showTab(component);
                } else {
                    
                    tabPanel.hideTab(component);
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Requisites.View', applyTo: '#actCheckWitnessTab', selector: '#actCheckTabPanel',
            applyBy: function (component, allowed) {
                var tabPanel = component.ownerCt;
                if (allowed) {
                    tabPanel.showTab(component);
                } else {
                    
                    tabPanel.hideTab(component);
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.Inspectors_Edit', applyTo: '#trigfInspectors', selector: '#actCheckWitnessTab' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.Inspectors_View', applyTo: '#trigfInspectors', selector: '#actCheckWitnessTab',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.AcquaintedWithDisposalCopy_Edit', applyTo: '#nfAcquaintedWithDisposalCopy', selector: '#actCheckWitnessTab' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.AcquaintedWithDisposalCopy_View', applyTo: '#nfAcquaintedWithDisposalCopy', selector: '#actCheckWitnessTab',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.DocumentPlace_Edit', applyTo: '#tfDocumentPlace', selector: '#actCheckWitnessTab' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.DocumentPlace_View', applyTo: '#tfDocumentPlace', selector: '#actCheckWitnessTab',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.DocumentTime_Edit', applyTo: '#tfDocumentTime', selector: '#actCheckWitnessTab' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.DocumentTime_View', applyTo: '#tfDocumentTime', selector: '#actCheckWitnessTab',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //ActCheckWitness
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Witness.Create', applyTo: 'b4addbutton', selector: 'actCheckWitnessGrid' },
        //здесь именно #actCheckEditPanel, а не селектор грида
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Witness.Edit', applyTo: 'b4savebutton', selector: 'actCheckWitnessGrid' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Witness.Delete', applyTo: 'b4deletecolumn', selector: 'actCheckWitnessGrid',
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
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Witness.View', applyTo: 'actCheckWitnessGrid', selector: '#actCheckTabPanel',
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

        // Постановление Роспотребнадзора
        {
            name: 'GkhGji.DocumentsGji.ActCheck.CreateResolutionRospotrebnadzor_View',
            applyTo: 'menuitem[ruleId=ActCheckToResolutionRospotrebnadzorRule]',
            selector: '#actCheckEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.manualAllowed = allowed;
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ActCheck.ResolutionRospotrebnadzor_View',
            applyTo: 'menuitem[ruleId=ActCheckToResolutionRospotrebnadzorRule]',
            selector: '#actCheckEditPanel',
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
        {
            name: 'GkhGji.DocumentsGji.ActCheck.ResolutionRospotrebnadzor_Edit',
            applyTo: 'menuitem[ruleId=ActCheckToResolutionRospotrebnadzorRule]',
            selector: '#actCheckEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        }
    ]
});