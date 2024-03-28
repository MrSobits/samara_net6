Ext.define('B4.aspects.permission.realityobj.ServiceOrg', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.realityobjserviceorgperm',

    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    permissions: [
        { name: 'Gkh.RealityObject.Register.ServiceOrg.Create', applyTo: 'b4addbutton', selector: 'realityobjserviceorggrid' },
        { name: 'Gkh.RealityObject.Register.ServiceOrg.Edit', applyTo: 'b4savebutton', selector: 'realityobjserviceorggrid' },
        {
            name: 'Gkh.RealityObject.Register.ServiceOrg.Delete', applyTo: 'b4deletecolumn', selector: 'realityobjserviceorggrid',
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