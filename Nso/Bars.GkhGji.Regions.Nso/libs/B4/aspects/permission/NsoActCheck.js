Ext.define('B4.aspects.permission.NsoActCheck', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.nsoactcheckperm',

    permissions: [
        {
            name: 'GkhGji.DocumentsGji.ActCheck.Register.Violation.NotRevealedViolations_View', applyTo: '#taNotRevealedViolations', selector: '#actCheckRealityObjectEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ActCheck.Register.Violation.OfficialsGuiltyActions_View', applyTo: '#taOfficialsGuiltyActions', selector: '#actCheckRealityObjectEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ActCheck.Register.Violation.OfficialsGuiltyActions_Edit', applyTo: '#taOfficialsGuiltyActions', selector: '#actCheckRealityObjectEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ActCheck.Register.Violation.PersonsWhoHaveViolated_View', applyTo: '#taPersonsWhoHaveViolated', selector: '#actCheckRealityObjectEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ActCheck.Register.Violation.PersonsWhoHaveViolated_Edit', applyTo: '#taPersonsWhoHaveViolated', selector: '#actCheckRealityObjectEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ActCheck.MergeActs', applyTo: '#btnMerge', selector: '#actCheckEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ActCheck.Register.Violation.ViolationDescription_View', applyTo: '[name=ViolationDescription]', selector: 'actcheckinspectionresultpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.ActCheck.Register.Violation.ViolationDescription_Edit', applyTo: '[name=ViolationDescription]', selector: 'actcheckinspectionresultpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        }
    ]
});