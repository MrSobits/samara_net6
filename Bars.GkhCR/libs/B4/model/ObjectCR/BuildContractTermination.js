Ext.define('B4.model.objectcr.BuildContractTermination', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BuildContractTermination'
    },
    fields: [
        { name: 'Id' },
        { name: 'TerminationDate' },
        { name: 'DocumentFile' },
        { name: 'Reason' },
        { name: 'DocumentNumber' },
        { name: 'DictReason' }
    ]
});