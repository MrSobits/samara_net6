Ext.define('B4.model.rapidresponsesystem.AppealsStatistic', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RapidResponseSystemAppeal',
        readAction: 'GetAppealsStatistic'
    },
    fields: [
        { name: 'NewAppealsCount', defaultValue: 0},
        { name: 'AppealsInWorkCount', defaultValue: 0},
        { name: 'ClosedAppealsCount', defaultValue: 0}
    ]
});