Ext.define('B4.aspects.permission.ManOrg', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.manorgperm',

    permissions: [
        { name: 'Gkh.Orgs.Managing.Create', applyTo: 'b4addbutton', selector: 'manorgGrid' },
        { name: 'Gkh.Orgs.Managing.Create', applyTo: 'b4addbutton', selector: '#manorgClaimGrid' },
        { name: 'Gkh.Orgs.Managing.Create', applyTo: 'b4addbutton', selector: '#manorgDocumentationGrid' },
        { name: 'Gkh.Orgs.Managing.Create', applyTo: 'b4addbutton', selector: '#manorgMembershipGrid' },
        { name: 'Gkh.Orgs.Managing.Create', applyTo: 'b4addbutton', selector: '#manorgRealityObjectGrid' },
        { name: 'Gkh.Orgs.Managing.Create', applyTo: 'b4addbutton', selector: '#manorgServiceGrid' },

        { name: 'Gkh.Orgs.Managing.Edit', applyTo: 'b4savebutton', selector: '#manorgEditPanel' },
        { name: 'Gkh.Orgs.Managing.Edit', applyTo: 'b4savebutton', selector: '#manorgActivityPanel' },
        { name: 'Gkh.Orgs.Managing.Edit', applyTo: 'b4savebutton', selector: '#manorgClaimEditWindow' },

        { name: 'Gkh.Orgs.Managing.Edit', applyTo: 'b4savebutton', selector: '#jskTsjContractEditWindow' },
        { name: 'Gkh.Orgs.Managing.Edit', applyTo: 'b4savebutton', selector: '#manorgContractOwnersEditWindow' },
        { name: 'Gkh.Orgs.Managing.Edit', applyTo: 'b4savebutton', selector: '#manorgContractJskTsjEditWindow' },
        
        { name: 'Gkh.Orgs.Managing.Edit', applyTo: 'b4savebutton', selector: '#manorgDocumentationEditWindow' },
        { name: 'Gkh.Orgs.Managing.Edit', applyTo: 'b4savebutton', selector: '#manorgMembershipEditWindow' },
        { name: 'Gkh.Orgs.Managing.Edit', applyTo: 'b4savebutton', selector: '#manorgRealityObjectGrid' },
        { name: 'Gkh.Orgs.Managing.Edit', applyTo: 'b4savebutton', selector: '#manorgServiceEditWindow' },

        { name: 'Gkh.Orgs.Managing.Edit', applyTo: 'b4savebutton', selector: '#workModeGrid' },
        { name: 'Gkh.Orgs.Managing.Edit', applyTo: 'b4savebutton', selector: '#dispatcherWorkGrid' },
        { name: 'Gkh.Orgs.Managing.Edit', applyTo: 'b4savebutton', selector: '#receptionCitizensGrid' },

        { name: 'Gkh.Orgs.Managing.Delete', applyTo: 'b4deletecolumn', selector: 'manorgGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.Orgs.Managing.Delete', applyTo: 'b4deletecolumn', selector: '#manorgClaimGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.Orgs.Managing.Delete', applyTo: 'b4deletecolumn', selector: '#manorgDocumentationGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.Orgs.Managing.Delete', applyTo: 'b4deletecolumn', selector: '#manorgMembershipGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.Orgs.Managing.Delete', applyTo: 'b4deletecolumn', selector: '#manorgRealityObjectGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.Orgs.Managing.Delete', applyTo: 'b4deletecolumn', selector: '#manorgServiceGrid',
            applyBy: function(component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.Orgs.Managing.Delete', applyTo: 'b4deletecolumn', selector: '#manorgContractGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});