Ext.define('B4.model.smev.SMEVNDFLAnswer', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVNDFLAnswer'
    },
    fields: [
        { name: 'INNUL' },
        { name: 'KPP' },
        { name: 'OrgName' },
        { name: 'Rate' },
        { name: 'RevenueCode' },
        { name: 'Month' },
        { name: 'RevenueSum' },
        { name: 'RecoupmentCode' },
        { name: 'RecoupmentSum' },
        { name: 'DutyBase' },
        { name: 'DutySum' },
        { name: 'UnretentionSum' },
        { name: 'RevenueTotalSum' },
        { name: 'SMEVNDFL' }
    ]
});