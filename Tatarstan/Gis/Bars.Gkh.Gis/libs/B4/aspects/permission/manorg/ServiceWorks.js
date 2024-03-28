Ext.define('B4.aspects.permission.manorg.ServiceWorks', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.manorgserviceworksperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
        { name: 'Gkh.Orgs.Managing.Register.Service.Works.Create', applyTo: 'b4addbutton', selector: 'manorgworkservicegrid' },
        { name: 'Gkh.Orgs.Managing.Register.Service.Works.Edit', applyTo: 'b4savebutton', selector: 'manorgworkserviceeditwindow' },
        {
            name: 'Gkh.Orgs.Managing.Register.Service.Works.Delete', applyTo: 'b4deletecolumn', selector: 'manorgworkservicegrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});