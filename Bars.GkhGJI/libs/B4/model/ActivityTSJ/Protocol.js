Ext.define('B4.model.activitytsj.Protocol', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActivityTsjProtocol'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActivityTsj', defaultValue: null },
        { name: 'KindProtocolTsj', defaultValue: null },
        { name: 'PercentageParticipant' },
        { name: 'CountVotes' },
        { name: 'GeneralCountVotes' },
        { name: 'DocumentNum' },
        { name: 'DocumentDate' },
        { name: 'VotesDate' },
        { name: 'KindProtocolTsjName' },
        { name: 'FileBulletin', defaultValue: null },
        { name: 'File', defaultValue: null }
    ]
});