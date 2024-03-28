Ext.define('B4.aspects.permission.TransferRf', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.transferrfperm',

    permissions: [
        { name: 'GkhRf.TransferRf.EditData.TrasferRfRec', applyTo: '#transferRfRecSaveButton', selector: '#transferRfRecordEditWindow' },
        { name: 'GkhRf.TransferRf.EditData.TrasferRfRecObj', applyTo: '#transferRfRecObjSaveButton', selector: '#transferRfRecordEditWindow' }
    ]
});