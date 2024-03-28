Ext.define('B4.model.dict.RealityObjectOutdoorProgramChangeJournal', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectOutdoorProgramChangeJournal'
    },
    fields: [
        { name: 'RealityObjectOutdoorProgram' },
        { name: 'MuCount' },
        { name: 'ChangeDate' },
        { name: 'UserName' },
        { name: 'Description' }
    ]
});