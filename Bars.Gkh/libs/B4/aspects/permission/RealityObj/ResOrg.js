Ext.define('B4.aspects.permission.realityobj.ResOrg', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.resorgperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    //основной грид и панель редактирования жилого дома
        { name: 'Gkh.RealityObject.Register.ResOrg.Create', applyTo: 'b4addbutton', selector: 'realityobjresorggrid' },
        { name: 'Gkh.RealityObject.Register.ResOrg.Edit', applyTo: 'b4savebutton', selector: 'realityobjresorggrid' },
        { name: 'Gkh.RealityObject.Register.ResOrg.Delete', applyTo: 'b4deletecolumn', selector: 'realityobjresorggrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});