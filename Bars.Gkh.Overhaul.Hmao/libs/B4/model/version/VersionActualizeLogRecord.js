Ext.define('B4.model.version.VersionActualizeLogRecord', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VersionActualizeLogRecord'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DateAction' },
        { name: 'ActualizeType' },
        { name: 'Action' },
        { name: 'InputParams' },
        { name: 'Address' },
        { name: 'WorkCode' },
        { name: 'Ceo' },
        { name: 'PlanYear' },
        { name: 'ChangePlanYear' },
        { name: 'PublishYear' },
        { name: 'ChangePublishYear' },
        { name: 'Volume' },
        { name: 'ChangeVolume' },
        { name: 'Sum' },
        { name: 'ChangeSum' },
        { name: 'Number' },
        { name: 'ChangeNumber' }
    ]
});