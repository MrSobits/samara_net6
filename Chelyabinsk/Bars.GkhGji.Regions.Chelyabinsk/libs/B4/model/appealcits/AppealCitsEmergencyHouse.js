Ext.define('B4.model.appealcits.AppealCitsEmergencyHouse', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsEmergencyHouse'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AppealCits', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'DocumentName', defaultValue: '' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'OMSDate' },
        { name: 'Inspector', defaultValue: null },
        { name: 'File', defaultValue: null },
    ]
});