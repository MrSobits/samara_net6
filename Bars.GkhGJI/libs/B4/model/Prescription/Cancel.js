Ext.define('B4.model.prescription.Cancel', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'PrescriptionCancel'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Prescription', defaultValue: null },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'DateCancel' },
        { name: 'IssuedCancel', defaultValue: null },
        { name: 'IsCourt', defaultValue: 30 },
        { name: 'Reason' },
        { name: 'TypeCancel', defaultValue: 10 },
        { name: 'DecisionMakingAuthority', defaultValue: null },
        { name: 'DateDecisionCourt' },
        { name: 'PetitionNumber' },
        { name: 'PetitionDate' },
        { name: 'DescriptionSet' },
        { name: 'Prolongation', defaultValue: null },
        { name: 'DateProlongation' }
    ]
});