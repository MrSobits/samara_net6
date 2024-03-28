Ext.define('B4.aspects.permission.realityobj.ApartInfo', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.realityobjapartinfoperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    //основной грид и панель редактирования жилого дома
        { name: 'Gkh.RealityObject.Register.ApartInfo.Create', applyTo: 'b4addbutton', selector: 'realityobjapartinfogrid' },
        { name: 'Gkh.RealityObject.Register.ApartInfo.Edit', applyTo: 'b4savebutton', selector: 'realityobjapartinfogrid' },
        {
            name: 'Gkh.RealityObject.Register.ApartInfo.Delete', applyTo: 'b4deletecolumn', selector: 'realityobjapartinfogrid',
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