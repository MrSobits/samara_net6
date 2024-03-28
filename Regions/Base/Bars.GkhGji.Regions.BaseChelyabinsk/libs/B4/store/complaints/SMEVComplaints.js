Ext.define('B4.store.complaints.SMEVComplaints', {
    extend: 'B4.base.Store',
    requires: ['B4.model.complaints.SMEVComplaints'],
    autoLoad: false,
    storeId: 'sMEVComplaintsStore',
    model: 'B4.model.complaints.SMEVComplaints'
});