/*
Перекрывается в модуле GkhGji.Regions.Smolensk
*/

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
        { name: 'GkhGji.DocumentsGji.Protocol.Field.DocumentNum_Edit', applyTo: '#nfDocumentNum', selector: '#protocolgjiEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Field.DocumentNumber_Edit',
            applyTo: '#tfDocumentNumber',
            selector: '#protocolgjiEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) component.setReadOnly(false);
                    else component.setReadOnly(true);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Field.DocumentNum_View',
            applyTo: '#nfDocumentNum',
            selector: '#protocolgjiEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.DocumentYear_Edit', applyTo: '#nfDocumentYear', selector: '#protocolgjiEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Field.DocumentYear_View',
            applyTo: '#nfDocumentYear',
            selector: '#protocolgjiEditPanel',
            applyBy: function(component, allowed) {
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
        {
            name: 'GkhGji.DocumentsGji.Protocol.Field.DocumentSubNum_View',
            applyTo: '#nfDocumentSubNum',
            selector: '#protocolgjiEditPanel',
            applyBy: function(component, allowed) {
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

        { name: 'GkhGji.DocumentsGji.Protocol.Field.FormatPlace_Edit', applyTo: 'textfield[name=FormatPlace]', selector: '#protocolgjiEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Field.FormatPlace_View',
            applyTo: 'textfield[name=FormatPlace]',
            selector: '#protocolgjiEditPanel',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.NotifDeliveredThroughOffice_Edit', applyTo: '#cbNotifDeliveredThroughOffice', selector: '#protocolgjiEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Field.NotifDeliveredThroughOffice_View',
            applyTo: '#cbNotifDeliveredThroughOffice',
            selector: '#protocolgjiEditPanel',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.FormatDate_Edit', applyTo: 'dfNotifDeliveryDate', selector: '#protocolgjiEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Field.FormatDate_View',
            applyTo: 'dfNotifDeliveryDate',
            selector: '#protocolgjiEditPanel',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.NotifNumber_Edit', applyTo: 'nfNotifNum', selector: '#protocolgjiEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Field.NotifNumber_View',
            applyTo: 'nfNotifNum',
            selector: '#protocolgjiEditPanel',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.DateOfProceedings_Edit', applyTo: 'datefield[name=DateOfProceedings]', selector: '#protocolgjiEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Field.DateOfProceedings_View',
            applyTo: 'datefield[name=DateOfProceedings]',
            selector: '#protocolgjiEditPanel',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.DateOfProceedings_Edit', applyTo: 'numberfield[name=HourOfProceedings]', selector: '#protocolgjiEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Field.DateOfProceedings_View',
            applyTo: 'numberfield[name=HourOfProceedings]',
            selector: '#protocolgjiEditPanel',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.DateOfProceedings_Edit', applyTo: 'numberfield[name=MinuteOfProceedings]', selector: '#protocolgjiEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Field.DateOfProceedings_View',
            applyTo: 'numberfield[name=MinuteOfProceedings]',
            selector: '#protocolgjiEditPanel',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.ProceedingCopyNum_Edit', applyTo: 'numberfield[name=ProceedingCopyNum]', selector: '#protocolgjiEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Field.ProceedingCopyNum_View',
            applyTo: 'numberfield[name=ProceedingCopyNum]',
            selector: '#protocolgjiEditPanel',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.ProceedingsPlace_Edit', applyTo: 'textfield[name=ProceedingsPlace]', selector: '#protocolgjiEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Field.ProceedingsPlace_View',
            applyTo: 'textfield[name=ProceedingsPlace]',
            selector: '#protocolgjiEditPanel',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.Remarks_Edit', applyTo: 'textarea[name=Remarks]', selector: '#protocolgjiEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Field.Remarks_View',
            applyTo: 'textarea[name=Remarks]',
            selector: '#protocolgjiEditPanel',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        { name: 'GkhGji.DocumentsGji.Protocol.Field.Snils_Edit', applyTo: '#tfSnils', selector: '#protocolgjiEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Field.Snils_View',
            applyTo: '#tfSnils',
            selector: '#protocolgjiEditPanel',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
            }
        },

        //Violations
        {
            name: 'GkhGji.DocumentsGji.Protocol.Register.Violation.View_DateFactRemoval',
            applyTo: '#cdfDateFactRemoval',
            selector: '#protocolgjiViolationGrid',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Register.Violation.View_DatePlanRemoval',
            applyTo: '#cdfDatePlanRemoval',
            selector: '#protocolgjiViolationGrid',
            applyBy: function(component, allowed) {
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
        {
            name: 'GkhGji.DocumentsGji.Protocol.Register.ArticleLaw.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#protocolgjiArticleLawGrid',
            applyBy: function(component, allowed) {
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

        //Annex
        { name: 'GkhGji.DocumentsGji.Protocol.Register.Annex.Create', applyTo: 'b4addbutton', selector: '#protocolgjiAnnexGrid' },
        { name: 'GkhGji.DocumentsGji.Protocol.Register.Annex.Edit', applyTo: 'b4savebutton', selector: '#protocolgjiAnnexEditWindow' },
        {
            name: 'GkhGji.DocumentsGji.Protocol.Register.Annex.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#protocolgjiAnnexGrid',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        }
    ]
});