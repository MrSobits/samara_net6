Ext.define('B4.model.claimwork.Notification', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'ClaimWork' },
        { name: 'DebtorType' },
        { name: 'BaseInfo' },
        { name: 'OwnerType' },
        { name: 'Address' },
        { name: 'ClaimWorkTypeBase' },
        { name: 'DocumentType' },
        { name: 'DocumentDate' },
        { name: 'DocumentNumber' },
        { name: 'State' },
        { name: 'SendDate' },
        { name: 'File' },
        { name: 'DateElimination' },
        { name: 'EliminationMethod' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'NotificationClw'
    }
});
