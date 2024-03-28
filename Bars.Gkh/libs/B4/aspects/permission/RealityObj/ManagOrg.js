Ext.define('B4.aspects.permission.realityobj.ManagOrg', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.realityobjmanagorgperm',

    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    permissions: [
        { name: 'Gkh.RealityObject.Register.ManagOrg.Create', applyTo: 'b4addbutton', selector: 'realobjcontractgrid' },
        { name: 'Gkh.RealityObject.Register.ManagOrg.Edit', applyTo: 'b4savebutton', selector: 'realobjdirmanageditwin' },
        { name: 'Gkh.RealityObject.Register.ManagOrg.Delete', applyTo: 'b4deletecolumn', selector: 'realobjcontractgrid',
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