Ext.define('B4.aspects.permission.documents.State', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.documentsstateperm',

    permissions: [
        { name: 'GkhDi.Disinfo.Docs.Edit', applyTo: 'b4savebutton', selector: '#documentsEditPanel' },
        { name: 'GkhDi.Disinfo.Docs.AddCopy', applyTo: '#copyDocsButton', selector: '#documentsEditPanel' }
    ]
});