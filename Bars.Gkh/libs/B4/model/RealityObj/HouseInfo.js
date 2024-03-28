Ext.define('B4.model.realityobj.HouseInfo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: ['B4.enums.KindRightToObject'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectHouseInfo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'NumObject' },
        { name: 'NumRegistrationOwner' },
        { name: 'DateRegistration' },
        { name: 'DateRegistrationOwner' },
        { name: 'TotalArea' },
        { name: 'Owner' },
        { name: 'Name' },
        { name: 'UnitMeasure', defaultValue: null },
        { name: 'KindRight', defaultValue: 10 }
    ]
});