Ext.define('B4.model.objectcr.AdditionalParameters', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AdditionalParameters'
    },
    fields: [
        { name: 'Id' },
        { name: 'ObjectCr' },
        { name: 'RequestKtsDate' },
        { name: 'TechConditionKtsDate' },
        { name: 'TechConditionKtsRecipient' },
        { name: 'RequestVodokanalDate' },
        { name: 'TechConditionVodokanalDate' },
        { name: 'TechConditionVodokanalRecipient' },
        { name: 'EntryForApprovalDate' },
        { name: 'ApprovalKtsDate' },
        { name: 'ApprovalVodokanalDate' },
        { name: 'InstallationPercentage' },
        { name: 'ClientAccepted' },
        { name: 'ClientAcceptedChangeDate' },
        { name: 'InspectorAccepted' },
        { name: 'InspectorAcceptedChangeDate' }
    ]
});