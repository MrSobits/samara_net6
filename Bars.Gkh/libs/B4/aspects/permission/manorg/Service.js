Ext.define('B4.aspects.permission.manorg.Service', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.manorgserviceperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
        { name: 'Gkh.Orgs.Managing.Register.Service.Create', applyTo: 'b4addbutton', selector: '#manorgServiceGrid' },
        { name: 'Gkh.Orgs.Managing.Register.Service.Edit', applyTo: 'b4savebutton', selector: '#manorgServiceEditWindow' },
        {
            name: 'Gkh.Orgs.Managing.Register.Service.Delete', applyTo: 'b4deletecolumn', selector: '#manorgServiceGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});