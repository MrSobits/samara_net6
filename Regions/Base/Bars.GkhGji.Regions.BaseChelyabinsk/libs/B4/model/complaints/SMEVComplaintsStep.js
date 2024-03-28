Ext.define('B4.model.complaints.SMEVComplaintsStep', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVComplaintsStep'
    },
    fields: [
        { name: 'Id' },
        { name: 'SMEVComplaints'},
        { name: 'Reason' },
        { name: 'AddDocList' },
        { name: 'NewDate' }, 
        { name: 'FileInfo' },
        { name: 'DOPetitionResult' },
        { name: 'DOTypeStep', defaultValue: 0 },
        { name: 'DOPetitionResult', defaultValue: 0 },       
        { name: 'YesNo', defaultValue: 20 }       
    ]
});