Ext.define('B4.aspects.permission.manorg.RealityObject', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.manorgrealobjperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
        { name: 'Gkh.Orgs.Managing.Register.RealityObject.Create', applyTo: 'b4addbutton', selector: 'realityobjectgrid' },
        { name: 'Gkh.Orgs.Managing.Register.RealityObject.Edit', applyTo: 'b4savebutton', selector: 'realityobjectgrid' },
        {
            name: 'Gkh.Orgs.Managing.Register.RealityObject.Delete', applyTo: 'b4deletecolumn', selector: 'realityobjectgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }

    ]
});