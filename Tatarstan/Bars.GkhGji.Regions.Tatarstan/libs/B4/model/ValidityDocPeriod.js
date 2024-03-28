Ext.define('B4.model.ValidityDocPeriod', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GjiValidityDocPeriod'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeDocument' },
        { name: 'StartDate' },
        { name: 'EndDate', defaultValue: null }
    ]
});