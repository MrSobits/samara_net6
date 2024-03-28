Ext.define('B4.model.gisGkh.PremisesGridModel', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    fields: [
        { name: 'CadastralNumber' },
        { name: 'EntranceNum' },
        { name: 'Floor' },
        { name: 'PremisesGUID' },
        { name: 'RealityObject' },
        { name: 'PremisesNum' },
        { name: 'RoomType' },
        { name: 'TotalArea' },
        { name: 'GrossArea' },
        { name: 'Matched' }
    ]
});