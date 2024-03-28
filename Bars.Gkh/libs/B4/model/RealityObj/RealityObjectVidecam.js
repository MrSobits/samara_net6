Ext.define('B4.model.realityobj.RealityObjectVidecam', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: [
        'B4.enums.YesNoNotSet'
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectVidecam'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject' },
        { name: 'Workability', defaultValue: 30 },
        { name: 'UnicalNumber'},
        { name: 'InstallPlace' },
        { name: 'TypeVidecam' },
        { name: 'VidecamAddress' }
    ]
});