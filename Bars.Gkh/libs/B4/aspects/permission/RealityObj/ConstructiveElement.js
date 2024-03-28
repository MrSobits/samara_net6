Ext.define('B4.aspects.permission.realityobj.ConstructiveElement', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.constructiveelementperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    //основной грид и панель редактирования жилого дома
        {
            name: 'Gkh.RealityObject.Register.ConstructiveElement.Create',
            applyTo: 'b4addbutton',
            selector: 'realityobjconstructiveelementgrid'
        },
        {
            name: 'Gkh.RealityObject.Register.ConstructiveElement.Edit',
            applyTo: 'b4savebutton',
            selector: 'realityobjconstructiveelementgrid'
        },
        {
            name: 'Gkh.RealityObject.Register.ConstructiveElement.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'realityobjconstructiveelementgrid',
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