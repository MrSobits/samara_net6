Ext.define('B4.model.rapidresponsesystem.AppealResponse', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RapidResponseSystemAppealResponse'
    },
    fields: [
        { name: 'Id'},
        { name: 'RapidResponseSystemAppealDetails'},
        { name: 'Theme'},
        { name: 'Response'},
        { name: 'ResponseDate'},
        { name: 'CarriedOutWork'},
        { name: 'ResponseFile'}
    ]
});