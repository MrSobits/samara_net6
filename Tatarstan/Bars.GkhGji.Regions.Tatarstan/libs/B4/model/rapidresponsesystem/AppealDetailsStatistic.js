Ext.define('B4.model.rapidresponsesystem.AppealDetailsStatistic', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RapidResponseSystemAppeal',
        readAction: 'GetAppealDetailsStatistic'
    },
    fields: [
        { name: 'NewAppealsCount', defaultValue: 0 },
        { name: 'AppealsInWorkCount', defaultValue: 0 },
        { name: 'ProcessedAppealsCount', defaultValue: 0 },
        { name: 'NotProcessedAppealsCount', defaultValue: 0 }
    ]
});