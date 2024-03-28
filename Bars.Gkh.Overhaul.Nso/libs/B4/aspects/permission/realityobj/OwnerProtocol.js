Ext.define('B4.aspects.permission.realityobj.OwnerProtocol', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.ownerprotocolperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    //основной грид и панель редактирования жилого дома
        { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerProtocols.Create', applyTo: 'b4addbutton', selector: 'roprotocolgrid' },
        {
            name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerProtocols.Edit', applyTo: 'b4editcolumn', selector: 'roprotocolgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerProtocols.Delete', applyTo: 'b4deletecolumn', selector: 'roprotocolgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});