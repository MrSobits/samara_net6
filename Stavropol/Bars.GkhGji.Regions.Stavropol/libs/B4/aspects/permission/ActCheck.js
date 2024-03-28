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
        { name: 'GkhGji.DocumentsGji.ActCheck.Field.DocumentNum_View', applyTo: '#nfDocumentNum', selector: '#actCheckEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
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

        //ActCheckDefinition
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Definition.Create', applyTo: 'b4addbutton', selector: '#actCheckDefinitionGrid' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Definition.Edit', applyTo: 'b4savebutton', selector: '#actCheckDefinitionEditWindow' },
        {
            name: 'GkhGji.DocumentsGji.ActCheck.Register.Definition.Fields.DocumentNum_View', applyTo: 'textfield[name=DocumentNum]', selector: '#actCheckDefinitionEditWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ActCheck.Register.Definition.Fields.DocumentNumber_View', applyTo: 'numberfield[name=DocumentNumber]', selector: '#actCheckDefinitionEditWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Definition.Delete', applyTo: 'b4deletecolumn', selector: '#actCheckDefinitionGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
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

        //ActCheckWitness
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Witness.Create', applyTo: 'b4addbutton', selector: '#actCheckWitnessGrid' },
        //здесь именно #actCheckEditPanel, а не селектор грида
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Witness.Edit', applyTo: '#actCheckWitnessSaveButton', selector: '#actCheckEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActCheck.Register.Witness.Delete', applyTo: 'b4deletecolumn', selector: '#actCheckWitnessGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        }
    ]
});