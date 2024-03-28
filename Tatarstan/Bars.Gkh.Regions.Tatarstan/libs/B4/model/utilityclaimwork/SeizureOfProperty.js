Ext.define('B4.model.utilityclaimwork.SeizureOfProperty', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    fields: [
        { name: 'Id' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'Year' },
        { name: 'Number' },
        { name: 'SubNumber' },
        { name: 'Document' },
        { name: 'DeliveryDate' },
        { name: 'IsCanceled' },
        { name: 'CancelReason' },        
        { name: 'JurInstitution', defaultValue: null },
        { name: 'Official' },
        { name: 'Location' },     
        { name: 'Creditor' },
        { name: 'BankDetails' },
        { name: 'OwnerType', defaultValue: 20 },
        { name: 'AccountOwner' },
        { name: 'AccountOwnerBankDetails' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'SeizureOfProperty'
    }
});