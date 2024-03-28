Ext.define('B4.model.objectcr.HousekeeperReport', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HousekeeperReport'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectCr' },
        { name: 'RealityObjectHousekeeper' },
        { name: 'Description' },
        { name: 'ReportDate' },

        { name: 'Answer' },
        { name: 'CheckDate' },
        { name: 'PhoneNumber' },
        { name: 'CheckTime' },

        { name: 'IsArranged', defaultValue: false },
        { name: 'State', defaultValue: null },
        { name: 'ReportNumber'},
        { name: 'FIO' }
    ]
});