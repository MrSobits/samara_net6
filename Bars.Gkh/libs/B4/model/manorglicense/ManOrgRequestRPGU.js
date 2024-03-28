Ext.define('B4.model.manorglicense.ManOrgRequestRPGU', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgRequestRPGU'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AnswerFile' },
        { name: 'AnswerText' },
        { name: 'LicRequest' },
        { name: 'ObjectCreateDate' },
        { name: 'RequestRPGUState', defaultValue: 30},
        { name: 'RequestRPGUType' },
        { name: 'Text' },
        { name: 'MessageId' },
        { name: 'File' }
    ]
});