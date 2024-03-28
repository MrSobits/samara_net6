Ext.define('B4.model.complaints.SMEVComplaintsExecutant', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVComplaintsExecutant'
    },
    fields: [
        { name: 'SMEVComplaints', defaultValue: null },
        { name: 'Executant', defaultValue: null },
        { name: 'Author', defaultValue: null },
        { name: 'Controller', defaultValue: null },
        { name: 'OrderDate' },
        { name: 'PerformanceDate' },
        { name: 'ExecutantZji' },
        { name: 'IsResponsible', defaultValue: false },
        { name: 'OnApproval', defaultValue: false },
        { name: 'Description' }
    ]
});