Ext.define('B4.model.realityobj.ConstructiveElement', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectConstructiveElement'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'LastYearOverhaul' },
        { name: 'ConstructiveElement', defaultValue: null },
        { name: 'ConstructiveElementName', defaultValue: null },
        { name: 'ConstructiveElementGroup' },
        { name: 'ConstructiveElementRepairPlanDate' }
    ]
});