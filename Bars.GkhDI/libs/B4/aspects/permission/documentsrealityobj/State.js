Ext.define('B4.aspects.permission.documentsrealityobj.State', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.documentsrealityobjstateperm',

    permissions: [
        // Документы
        { name: 'GkhDi.DisinfoRealObj.DocumentsRealityObj.Edit', applyTo: 'b4savebutton', selector: '#documentsRealityObjEditPanel' },
        { name: 'GkhDi.DisinfoRealObj.DocumentsRealityObj.AddCopy', applyTo: '#copyDocsRealityObjButton', selector: '#documentsRealityObjEditPanel' },
        { name: 'GkhDi.DisinfoRealObj.DocumentsRealityObj.ByYear.Add', applyTo: 'b4addbutton', selector: '#realityObjProtocolGrid' },
        { name: 'GkhDi.DisinfoRealObj.DocumentsRealityObj.ByYear.Edit', applyTo: 'b4savebutton', selector: '#realityObjProtocolEditWindow' },
        {
            name: 'GkhDi.DisinfoRealObj.DocumentsRealityObj.ByYear.Delete', applyTo: 'b4deletecolumn', selector: '#realityObjProtocolGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});