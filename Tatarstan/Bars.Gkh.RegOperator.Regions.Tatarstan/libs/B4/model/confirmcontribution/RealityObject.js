Ext.define('B4.model.confirmcontribution.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ConfirmContributionRealityObject',
        listAction: 'ListRealObj'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality', defaultValue: null },
        { name: 'Address'}
    ]
});