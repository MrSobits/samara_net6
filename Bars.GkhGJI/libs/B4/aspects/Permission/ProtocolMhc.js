Ext.define('B4.aspects.permission.ProtocolMhc', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.protocolmhcperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Create', applyTo: 'b4addbutton', selector: '#protocolMhcGrid' },
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Edit', applyTo: 'b4savebutton', selector: '#protocolMhcEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMhc.Delete', applyTo: 'b4deletecolumn', selector: '#protocolMhcGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        }
    
    ]
});