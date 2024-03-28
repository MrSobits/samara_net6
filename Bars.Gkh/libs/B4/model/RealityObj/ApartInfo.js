Ext.define('B4.model.realityobj.ApartInfo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: ['B4.enums.YesNoNotSet'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectApartInfo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'NumApartment' },
        { name: 'AreaLiving' },
        { name: 'AreaTotal' },
        { name: 'CountPeople' },
        { name: 'Phone' },
        { name: 'FioOwner' },
        { name: 'Privatized', defaultValue: 30 }
    ]
});