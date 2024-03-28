Ext.define('B4.model.baseintegration.RisTask', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RisTask'
    },
    fields: [
        { name: 'MethodName' },
        { name: 'StartTime' },
        { name: 'EndTime' },
        { name: 'UserName' },
        { name: 'TaskState' },
        { name: 'TriggerId' },
        { name: 'RequestXmlFileId' },
        { name: 'ResponseXmlFileId' }
    ]
});