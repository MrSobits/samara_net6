Ext.define('B4.model.gasequipmentorg.Contract', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GasEquipmentOrgRealityObj'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' },
        { name: 'GasEquipmentOrg' },
        { name: 'RealityObject' },
        { name: 'Address' },
        { name: 'StartDate' },
        { name: 'EndDate' },
        { name: 'DocumentNum' },
        { name: 'File', defaultValue: null }
    ]
});