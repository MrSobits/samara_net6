Ext.define('B4.aspects.permission.ProtocolMvd', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.protocolmvdperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Create', applyTo: 'b4addbutton', selector: '#protocolMvdGrid' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Edit', applyTo: 'b4savebutton', selector: '#protocolMvdEditPanel' },
        { name: 'GkhGji.DocumentsGji.ProtocolMvd.Delete', applyTo: 'b4deletecolumn', selector: '#protocolMvdGrid',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        }
    
    ]
});