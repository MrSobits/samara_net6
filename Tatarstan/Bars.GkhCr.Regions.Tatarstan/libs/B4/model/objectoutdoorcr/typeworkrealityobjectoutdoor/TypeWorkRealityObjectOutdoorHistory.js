Ext.define('B4.model.objectoutdoorcr.typeworkrealityobjectoutdoor.TypeWorkRealityObjectOutdoorHistory', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeWorkRealityObjectOutdoorHistory'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeWorkRealityObjectOutdoor' },
        { name: 'ObjectCreateDate' },
        { name: 'TypeAction' },
        { name: 'Volume' },
        { name: 'Sum' },
        { name: 'UserName' }
    ]
});