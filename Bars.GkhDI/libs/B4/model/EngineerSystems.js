Ext.define('B4.model.EngineerSystems', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EngineerSystems'
    },
    fields: [
        { name: 'Id', useNull: true },

        { name: 'TypeHeating', defaultValue: null },
        { name: 'TypeHotWater', defaultValue: null },
        { name: 'TypeColdWater', defaultValue: null },
        { name: 'TypeGas', defaultValue: null },
        { name: 'TypeVentilation', defaultValue: null },
        { name: 'Firefighting', defaultValue: null },
        { name: 'TypeDrainage', defaultValue: null },

        { name: 'TypePower', defaultValue: null },
        { name: 'TypePowerPoints', defaultValue: null },

        { name: 'TypeSewage', defaultValue: null },
        { name: 'SewageVolume', defaultValue: null }
    ]
});