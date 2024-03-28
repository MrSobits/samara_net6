Ext.define('B4.model.activitytsj.RealObj', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActivityTsjRealObj'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'ActivityTsj', defaultValue: null },
        { name: 'Address' },
        { name: 'Municipality' }
    ]
});