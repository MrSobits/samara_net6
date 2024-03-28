Ext.define('B4.aspects.permission.ActRemoval', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.actremovalperm',

    permissions: [
        /*
        * name - имя пермишена в дереве, 
        * applyTo - селектор контрола, к которому применяется пермишен, 
        * selector - селектор формы, на которой находится контрол
        */

        //поля панели редактирования ActRemoval
        { name: 'GkhGji.DocumentsGji.ActRemoval.Field.DocumentNum_View', applyTo: '#nfDocumentNum', selector: '#actRemovalEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActRemoval.Field.DocumentNum_Edit', applyTo: '#nfDocumentNum', selector: '#actRemovalEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.ActRemoval.Field.DocumentNumber_Edit', applyTo: '#nfDocumentNum', selector: '#actRemovalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.setReadOnly(false);
                    else component.setReadOnly(true);
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActRemoval.Field.DocumentYear_View', applyTo: '#nfDocumentYear', selector: '#actRemovalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActRemoval.Field.DocumentYear_Edit', applyTo: '#nfDocumentYear', selector: '#actRemovalEditPanel' },

        { name: 'GkhGji.DocumentsGji.ActRemoval.Field.LiteralNum_Edit', applyTo: '#nfLiteralNum', selector: '#actRemovalEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.ActRemoval.Field.LiteralNum_View', applyTo: '#nfLiteralNum', selector: '#actRemovalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.ActRemoval.Field.DocumentSubNum_Edit', applyTo: '#nfDocumentSubNum', selector: '#actRemovalEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActRemoval.Field.DocumentSubNum_View', applyTo: '#nfDocumentSubNum', selector: '#actRemovalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.ActRemoval.Field.DocumentDate_Edit', applyTo: '#dfDocumentDate', selector: '#actRemovalEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActRemoval.Field.Inspectors_Edit', applyTo: '#actRemovalInspectorsTrigerField', selector: '#actRemovalEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActRemoval.Field.Area_Edit', applyTo: '#nfAreaActRemoval', selector: '#actRemovalEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActRemoval.Field.Flat_Edit', applyTo: '#nfFlatActRemoval', selector: '#actRemovalEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActRemoval.Field.TypeRemoval_Edit', applyTo: '#cbTypeRemoval', selector: '#actRemovalEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActRemoval.Field.Description_Edit', applyTo: '#taDescriptionActRemoval', selector: '#actRemovalEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActRemoval.Field.DateFactRemoval_Edit', applyTo: '#cdfDateFactRemoval', selector: '#actRemovalViolationGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.editor = { xtype: 'datefield', format: 'd.m.Y' };
                    else component.editor = null;
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ActRemoval.Field.CircumstancesDescription_View', applyTo: '#gcCircumstancesDescription', selector: '#actRemovalViolationGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ActRemoval.Field.CircumstancesDescription_Edit', applyTo: '#gcCircumstancesDescription', selector: '#actRemovalViolationGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.editor = { xtype: 'textfield' };
                    else component.editor = null;
                }
            }
        }
    ]
});