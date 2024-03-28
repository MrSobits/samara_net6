Ext.define('B4.aspects.permission.ProtocolRSO', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.protocolrsoperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Create', applyTo: 'b4addbutton', selector: '#protocolRSOGrid' },
        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Edit', applyTo: 'b4savebutton', selector: '#protocolRSOEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolRSO.Delete', applyTo: 'b4deletecolumn', selector: '#protocolRSOGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        }
    
    ]
});