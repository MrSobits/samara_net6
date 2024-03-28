Ext.define('B4.model.dict.EfficiencyRatingPeriod', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EfficiencyRatingPeriod'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'Group' }
    ]
});