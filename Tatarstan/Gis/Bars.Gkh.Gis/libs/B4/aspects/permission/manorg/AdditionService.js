Ext.define('B4.aspects.permission.manorg.AdditionService', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.manorgadditionserviceperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
        { name: 'Gkh.Orgs.Managing.Register.Service.Addition.Create', applyTo: 'b4addbutton', selector: '#manorgAdditionServiceGrid' },
        { name: 'Gkh.Orgs.Managing.Register.Service.Addition.Edit', applyTo: 'b4savebutton', selector: '#manorgAdditionServiceEditWindow' },
        {
            name: 'Gkh.Orgs.Managing.Register.Service.Addition.Delete', applyTo: 'b4deletecolumn', selector: '#manorgAdditionServiceGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});