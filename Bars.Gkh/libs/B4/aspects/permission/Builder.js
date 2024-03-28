Ext.define('B4.aspects.permission.Builder', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.builderperm',

    permissions: [
        { name: 'Gkh.Orgs.Builder.Create', applyTo: 'b4addbutton', selector: 'builderGrid' },
        { name: 'Gkh.Orgs.Builder.Create', applyTo: 'b4addbutton', selector: '#builderDocumentGrid' },
        { name: 'Gkh.Orgs.Builder.Create', applyTo: 'b4addbutton', selector: '#builderFeedbackGrid' },
        { name: 'Gkh.Orgs.Builder.Create', applyTo: 'b4addbutton', selector: '#builderLoanGrid' },
        { name: 'Gkh.Orgs.Builder.Create', applyTo: 'b4addbutton', selector: '#builderProductionBaseGrid' },
        { name: 'Gkh.Orgs.Builder.Create', applyTo: 'b4addbutton', selector: '#builderSroInfoGrid' },
        { name: 'Gkh.Orgs.Builder.Create', applyTo: 'b4addbutton', selector: '#builderTechniqueGrid' },
        { name: 'Gkh.Orgs.Builder.Create', applyTo: 'b4addbutton', selector: '#builderWorkforceGrid' },

        { name: 'Gkh.Orgs.Builder.Edit', applyTo: 'b4savebutton', selector: '#builderEditPanel' },
        { name: 'Gkh.Orgs.Builder.Edit', applyTo: 'b4savebutton', selector: '#builderActivityPanel' },
        { name: 'Gkh.Orgs.Builder.Edit', applyTo: 'b4savebutton', selector: '#builderDocumentEditWindow' },
        { name: 'Gkh.Orgs.Builder.Edit', applyTo: 'b4savebutton', selector: '#builderFeedbackEditWindow' },
        { name: 'Gkh.Orgs.Builder.Edit', applyTo: 'b4savebutton', selector: '#builderLoanEditWindow' },
        { name: 'Gkh.Orgs.Builder.Edit', applyTo: 'b4savebutton', selector: '#builderProductionBaseEditWindow' },
        { name: 'Gkh.Orgs.Builder.Edit', applyTo: 'b4savebutton', selector: '#builderSroInfoEditWindow' },
        { name: 'Gkh.Orgs.Builder.Edit', applyTo: 'b4savebutton', selector: '#builderTechniqueEditWindow' },
        { name: 'Gkh.Orgs.Builder.Edit', applyTo: 'b4savebutton', selector: '#builderWorkforceEditWindow' },

        { name: 'Gkh.Orgs.Builder.Delete', applyTo: 'b4deletecolumn', selector: 'builderGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.Orgs.Builder.Delete', applyTo: 'b4deletecolumn', selector: '#builderDocumentGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.Orgs.Builder.Delete', applyTo: 'b4deletecolumn', selector: '#builderFeedbackGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.Orgs.Builder.Delete', applyTo: 'b4deletecolumn', selector: '#builderLoanGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.Orgs.Builder.Delete', applyTo: 'b4deletecolumn', selector: '#builderProductionBaseGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.Orgs.Builder.Delete', applyTo: 'b4deletecolumn', selector: '#builderSroInfoGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.Orgs.Builder.Delete', applyTo: 'b4deletecolumn', selector: '#builderTechniqueGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.Orgs.Builder.Delete', applyTo: 'b4deletecolumn', selector: '#builderWorkforceGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});