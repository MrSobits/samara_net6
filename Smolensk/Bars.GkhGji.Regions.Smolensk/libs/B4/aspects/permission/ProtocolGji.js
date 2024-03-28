Ext.define('B4.aspects.permission.ProtocolGji', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.protocolgjiperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */

    //поля панели редактирования
        { name: 'GkhGji.DocumentsGji.Protocol.Field.NoticeDocNumber_Edit', applyTo: '[name=NoticeDocNumber]', selector: '#protocolgjiEditPanel' },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.NoticeDocDate_Edit', applyTo: '[name=NoticeDocDate]', selector: '#protocolgjiEditPanel' },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.DocumentNum_Edit', applyTo: '#nfDocumentNum', selector: '#protocolgjiEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Field.DocumentNumber_View', applyTo: '#tfDocumentNumber', selector: '#protocolgjiEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.setReadOnly(false);
                    else component.setReadOnly(true);
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.DocumentNum_View', applyTo: '#nfDocumentNum', selector: '#protocolgjiEditPanel',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },

        { name: 'GkhGji.DocumentsGji.Protocol.Field.DocumentYear_Edit', applyTo: '#nfDocumentYear', selector: '#protocolgjiEditPanel' },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.DocumentYear_View', applyTo: '#nfDocumentYear', selector: '#protocolgjiEditPanel',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },

        { name: 'GkhGji.DocumentsGji.Protocol.Field.LiteralNum_Edit', applyTo: '#nfLiteralNum', selector: '#protocolgjiEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Field.LiteralNum_View', applyTo: '#nfLiteralNum', selector: '#protocolgjiEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.Protocol.Field.DocumentSubNum_Edit', applyTo: '#nfDocumentSubNum', selector: '#protocolgjiEditPanel' },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.DocumentSubNum_View', applyTo: '#nfDocumentSubNum', selector: '#protocolgjiEditPanel',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },

        { name: 'GkhGji.DocumentsGji.Protocol.Field.Inspectors_Edit', applyTo: '#trigfInspector', selector: '#protocolgjiEditPanel' },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.ToCourt_Edit', applyTo: '#cbToCourt', selector: '#protocolgjiEditPanel' },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.DateToCourt_Edit', applyTo: '#dfDateToCourt', selector: '#protocolgjiEditPanel' },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.Description_Edit', applyTo: '#taDescriptionProtocol', selector: '#protocolgjiEditPanel' },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.Executant_Edit', applyTo: '#cbExecutant', selector: '#protocolgjiEditPanel' },

        //Violations
        { name: 'GkhGji.DocumentsGji.Protocol.Register.Violation.View_DateFactRemoval', applyTo: '#cdfDateFactRemoval', selector: '#protocolgjiViolationGrid',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },
        { name: 'GkhGji.DocumentsGji.Protocol.Register.Violation.View_DatePlanRemoval', applyTo: '#cdfDatePlanRemoval', selector: '#protocolgjiViolationGrid',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },
        { name: 'GkhGji.DocumentsGji.Protocol.Register.Violation.Edit', applyTo: '#protocolViolationSaveButton', selector: '#protocolgjiViolationGrid' },

        //ArticleLaw
        { name: 'GkhGji.DocumentsGji.Protocol.Register.ArticleLaw.Create', applyTo: 'b4addbutton', selector: '#protocolgjiArticleLawGrid' },
        { name: 'GkhGji.DocumentsGji.Protocol.Register.ArticleLaw.Edit', applyTo: '#protocolSaveButton', selector: '#protocolgjiArticleLawGrid' },
        { name: 'GkhGji.DocumentsGji.Protocol.Register.ArticleLaw.Delete', applyTo: 'b4deletecolumn', selector: '#protocolgjiArticleLawGrid',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        },

        //Definition
        { name: 'GkhGji.DocumentsGji.Protocol.Register.Definition.Create', applyTo: 'b4addbutton', selector: '#protocolgjiDefinitionGrid' },
        { name: 'GkhGji.DocumentsGji.Protocol.Register.Definition.Edit', applyTo: 'b4savebutton', selector: '#protocolgjiDefinitionEditWindow' },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Register.Definition.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#protocolgjiDefinitionGrid',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        
        { name: 'GkhGji.DocumentsGji.Protocol.Register.Definition.DescriptionSet', applyTo: '[name=DescriptionSet]', selector: '#protocolgjiDefinitionEditWindow' },
        { name: 'GkhGji.DocumentsGji.Protocol.Register.Definition.DefinitionResult', applyTo: '[name=DefinitionResult]', selector: '#protocolgjiDefinitionEditWindow' },
        { name: 'GkhGji.DocumentsGji.Protocol.Register.Definition.ExtendUntil', applyTo: '[name=ExtendUntil]', selector: '#protocolgjiDefinitionEditWindow' },

        //Annex
        { name: 'GkhGji.DocumentsGji.Protocol.Register.Annex.Create', applyTo: 'b4addbutton', selector: '#protocolgjiAnnexGrid' },
        { name: 'GkhGji.DocumentsGji.Protocol.Register.Annex.Edit', applyTo: 'b4savebutton', selector: '#protocolgjiAnnexEditWindow' },
        { name: 'GkhGji.DocumentsGji.Protocol.Register.Annex.Delete', applyTo: 'b4deletecolumn', selector: '#protocolgjiAnnexGrid',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        }
    ]
});