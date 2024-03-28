Ext.define('B4.aspects.permission.TatDisposal', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.tatdisposalperm',

    permissions: [
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.Requisites.Field.PlannedActions_View',
            applyTo: 'gkhtriggerfield[name=PlannedActions]',
            selector: '#disposalTabPanel',
            applyBy: function (component, allowed) {
                var available = !this.controller.isDisposal() && allowed;

                // Указываем актуальную обязательность тут, поскольку аспект применяется
                // при каждом обновлении панели редактирования документа уже после onAfterSetPanelData
                component.allowBlank = !(available && component.permissionRequired);
                this.setVisible(component, available);
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.Requisites.Field.PlannedActions_Edit',
            applyTo: 'gkhtriggerfield[name=PlannedActions]',
            selector: '#disposalTabPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setHideTrigger(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.SubjectVerificationGrid.View',
            applyTo: '[name=SubjectVerificationGrid]',
            selector: '#disposalTabPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.tab.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.SubjectVerificationGrid.Create',
            applyTo: 'b4addbutton',
            selector: 'disposalsubjectverificationgrid',
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.SubjectVerificationGrid.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'disposalsubjectverificationgrid',
            applyBy: this.setVisible
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.SurveyPurposeGrid.View',
            applyTo: '[name=SurveyPurposeGrid]',
            selector: '#disposalTabPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.tab.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.SurveyPurposeGrid.Create',
            applyTo: 'b4addbutton',
            selector: 'disposalsurveypurposegrid'
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.SurveyPurposeGrid.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'disposalsurveypurposegrid',
            applyBy: this.setVisible
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.SurveyObjectiveGrid.View',
            applyTo: '[name=SurveyObjectiveGrid]',
            selector: '#disposalTabPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.tab.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.SurveyObjectiveGrid.Create',
            applyTo: 'b4addbutton',
            selector: 'disposalsurveyobjectivegrid'
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.SurveyObjectiveGrid.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'disposalsurveyobjectivegrid',
            applyBy: this.setVisible
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.InspFoundationCheckGrid.View',
            applyTo: '[name=InspFoundationCheckGrid]',
            selector: '#disposalTabPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.tab.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.InspFoundationCheckGrid.Create',
            applyTo: 'b4addbutton',
            selector: 'disposalinspfoundationcheckgrid'
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.InspFoundationCheckGrid.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'disposalinspfoundationcheckgrid',
            applyBy: this.setVisible
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Field.DocumentNumberWithResultAgreement_Edit', applyTo: '#cbDocumentNumberWithResultAgreement', selector: '#disposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.readOnly = !allowed;
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Field.DocumentNumberWithResultAgreement_View', applyTo: '#cbDocumentNumberWithResultAgreement', selector: '#disposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //поля панели редактирования
        {
            name: 'GkhGji.DocumentsGji.Disposal.Field.DocumentDateWithResultAgreement_Edit', applyTo: '#cbDocumentDateWithResultAgreement', selector: '#disposalEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.readOnly = !allowed;
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Field.DocumentDateWithResultAgreement_View', applyTo: '#cbDocumentDateWithResultAgreement', selector: '#disposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        }
    ]
});