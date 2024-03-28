Ext.define('B4.store.version.Stage1GroupingForSelect', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'Id' },
        { name: 'Stage1Id' },
        { name: 'WorkId' },
        { name: 'WorkName' },
        { name: 'CorrectionYear' },
        { name: 'RealityObjectId' },
        { name: 'CeoName' },
        { name: 'StructElementName' },
        { name: 'Sum' },
        { name: 'Volume' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'Dpkr',
        listAction: 'GetListForAddInObjectCr'
    }
});