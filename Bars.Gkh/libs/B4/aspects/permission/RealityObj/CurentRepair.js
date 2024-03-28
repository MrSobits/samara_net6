Ext.define('B4.aspects.permission.realityobj.CurentRepair', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.curentrepairperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    
        { name: 'Gkh.RealityObject.Register.CurentRepair.Create', applyTo: 'b4addbutton', selector: 'realityobjcurentrepairgrid' },
        { name: 'Gkh.RealityObject.Register.CurentRepair.Edit', applyTo: 'b4savebutton', selector: 'realityobjcurentrepairgrid' },
        {
            name: 'Gkh.RealityObject.Register.CurentRepair.Delete', applyTo: 'b4deletecolumn', selector: 'realityobjcurentrepairgrid',
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