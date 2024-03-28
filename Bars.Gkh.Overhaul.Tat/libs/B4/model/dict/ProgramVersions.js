Ext.define('B4.model.dict.ProgramVersions', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramCr'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'PeriodName'},
        { name: 'TypeVisibilityProgramCr'},
        { name: 'TypeProgramStateCr'}
    ]
});