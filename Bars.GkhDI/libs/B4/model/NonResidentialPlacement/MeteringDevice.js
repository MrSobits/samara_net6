Ext.define('B4.model.nonresidentialplacement.MeteringDevice', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'NonResidentialPlacementMeteringDevice'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'NonResidentialPlacement', defaultValue: null },
        { name: 'MeteringDevice', defaultValue: null },
        { name: 'Name' },
        { name: 'AccuracyClass' },
        { name: 'TypeAccounting' }
    ]
});