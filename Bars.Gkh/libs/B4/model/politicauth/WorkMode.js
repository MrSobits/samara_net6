Ext.define('B4.model.politicauth.WorkMode', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: ['B4.enums.TypeMode',
                'B4.enums.TypeDayOfWeek'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'PoliticAuthorityWorkMode'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'TypeMode', defaultValue: 10 },
        { name: 'TypeDayOfWeek', defaultValue: 10 },
        { name: 'StartDate', defultValue: null },
        { name: 'EndDate', defultValue: null },
        { name: 'AroundClock', defultValue: false },
        { name: 'Pause' }
    ]
});