Ext.define('B4.aspects.permission.manorg.Membership', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.manorgmembershipperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
        { name: 'Gkh.Orgs.Managing.Register.Membership.Create', applyTo: 'b4addbutton', selector: '#manorgMembershipGrid' },
        { name: 'Gkh.Orgs.Managing.Register.Membership.Edit', applyTo: 'b4savebutton', selector: '#manorgMembershipEditWindow' },
        {
            name: 'Gkh.Orgs.Managing.Register.Membership.Delete', applyTo: 'b4deletecolumn', selector: '#manorgMembershipGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});