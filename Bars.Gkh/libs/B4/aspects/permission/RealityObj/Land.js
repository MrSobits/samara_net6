Ext.define('B4.aspects.permission.realityobj.Land', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.realityobjlandperm',

    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    permissions: [
        { name: 'Gkh.RealityObject.Register.Land.Create', applyTo: 'b4addbutton', selector: 'realityobjlandgrid' },
        { name: 'Gkh.RealityObject.Register.Land.Edit', applyTo: 'b4savebutton', selector: 'realityobjlandeditwindow' },
        {
            name: 'Gkh.RealityObject.Register.Land.Delete', applyTo: 'b4deletecolumn', selector: 'realityobjlandgrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        }
    ]
});