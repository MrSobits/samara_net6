Ext.define('B4.aspects.permission.BelayManOrgActivity', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.belaymanorgactivityperm',

    permissions: [
        { name: 'Gkh.BelayManOrgActivity.Create', applyTo: 'b4addbutton', selector: '#belayManOrgActivityGrid' },
        { name: 'Gkh.BelayManOrgActivity.Edit', applyTo: 'b4savebutton', selector: '#belayManOrgActivityEditWindow' },
        { name: 'Gkh.BelayManOrgActivity.Delete', applyTo: 'b4deletecolumn', selector: '#belayManOrgActivityGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        { name: 'Gkh.BelayManOrgActivity.Create', applyTo: 'b4addbutton', selector: '#belayPolicyGrid' },
        { name: 'Gkh.BelayManOrgActivity.Edit', applyTo: 'b4savebutton', selector: '#belayPolicyEditPanel' },
        { name: 'Gkh.BelayManOrgActivity.Delete', applyTo: 'b4deletecolumn', selector: '#belayPolicyGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        { name: 'Gkh.BelayManOrgActivity.Create', applyTo: 'b4addbutton', selector: '#belayPolicyInsuredEventGrid' },
        { name: 'Gkh.BelayManOrgActivity.Edit', applyTo: 'b4savebutton', selector: '#belayPolicyInsuredEventEditWindow' },
        { name: 'Gkh.BelayManOrgActivity.Delete', applyTo: 'b4deletecolumn', selector: '#belayPolicyInsuredEventGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        { name: 'Gkh.BelayManOrgActivity.Edit', applyTo: '#belayPolicyMkdExcludeSaveButton', selector: '#belayPolicyMkdExcludeGrid' },
        { name: 'Gkh.BelayManOrgActivity.Delete', applyTo: 'b4deletecolumn', selector: '#belayPolicyMkdExcludeGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        
        { name: 'Gkh.BelayManOrgActivity.Create', applyTo: 'b4addbutton', selector: '#belayPolicyMkdIncludeGrid' },
        { name: 'Gkh.BelayManOrgActivity.Delete', applyTo: 'b4deletecolumn', selector: '#belayPolicyMkdIncludeGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        
        { name: 'Gkh.BelayManOrgActivity.Create', applyTo: 'b4addbutton', selector: '#belayPolicyMkdIncludeGrid' },
        { name: 'Gkh.BelayManOrgActivity.Edit', applyTo: '#belayPolicyMkdIncludeSaveButton', selector: '#belayPolicyMkdIncludeGrid' },
        { name: 'Gkh.BelayManOrgActivity.Delete', applyTo: 'b4deletecolumn', selector: '#belayPolicyMkdIncludeGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        { name: 'Gkh.BelayManOrgActivity.Create', applyTo: 'b4addbutton', selector: '#belayPolicyPaymentGrid' },
        { name: 'Gkh.BelayManOrgActivity.Edit', applyTo: 'b4savebutton', selector: '#belayPolicyPaymentEditWindow' },
        { name: 'Gkh.BelayManOrgActivity.Delete', applyTo: 'b4deletecolumn', selector: '#belayPolicyPaymentGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        { name: 'Gkh.BelayManOrgActivity.Create', applyTo: 'b4addbutton', selector: '#belayPolicyRiskGrid' },
        { name: 'Gkh.BelayManOrgActivity.Delete', applyTo: 'b4deletecolumn', selector: '#belayPolicyRiskGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});