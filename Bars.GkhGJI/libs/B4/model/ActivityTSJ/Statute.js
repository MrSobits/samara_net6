Ext.define('B4.model.activitytsj.Statute', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActivityTsjStatute'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActivityTsj', defaultValue: null },
        { name: 'DocNumber' },
        { name: 'StatuteProvisionDate', defaultValue: null },
        { name: 'StatuteApprovalDate', defaultValue: null },
        { name: 'StatuteFile', defaultValue: null },
        { name: 'ConclusionDate', defaultValue: null },
        { name: 'ConclusionNum' },
        { name: 'ConclusionDescription' },
        { name: 'TypeConclusion', defaultValue: 10 },
        { name: 'ConclusionFile', defaultValue: null },
        { name: 'State', defaultValue: null }
    ]
});