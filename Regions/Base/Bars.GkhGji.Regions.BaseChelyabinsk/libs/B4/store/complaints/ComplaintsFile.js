Ext.define('B4.store.complaints.ComplaintsFile', {
    extend: 'B4.base.Store',
    requires: ['B4.model.complaints.ComplaintsFile'],
    autoLoad: false,
    storeId: 'sMEVComplaintsFileStore',
    model: 'B4.model.complaints.ComplaintsFile'
});