Ext.define('B4.model.dict.ProgramCrChangeJournal', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramCrChangeJournal'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeChange', defaultValue: 0 },
        { name: 'ProgramCr'},
        { name: 'MuCount' },
        { name: 'ChangeDate' },
        { name: 'UserName' },
        { name: 'Description' }
    ]
});