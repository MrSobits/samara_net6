Ext.define('B4.aspects.permission.realityobj.HouseInfo', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.realityobjhouseinfoperm',

    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    permissions: [
        { name: 'Gkh.RealityObject.Register.HouseInfo.Create', applyTo: 'b4addbutton', selector: 'realityobjhouseinfogrid' },
        { name: 'Gkh.RealityObject.Register.HouseInfo.Edit', applyTo: 'b4savebutton', selector: 'realityobjhouseinfoeditwindow' },
        { name: 'Gkh.RealityObject.Register.HouseInfo.Delete', applyTo: 'b4deletecolumn', selector: 'realityobjhouseinfogrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});