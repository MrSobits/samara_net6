Ext.define('B4.aspects.permission.realityobj.Image', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.realityobjimageperm',

    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    permissions: [
        { name: 'Gkh.RealityObject.Register.Image.Create', applyTo: 'b4addbutton', selector: 'realityobjimagegrid' },
        { name: 'Gkh.RealityObject.Register.Image.Edit', applyTo: 'b4savebutton', selector: 'realityobjimagegrid' },
        {
            name: 'Gkh.RealityObject.Register.Image.Delete', applyTo: 'b4deletecolumn', selector: 'realityobjimagegrid',
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