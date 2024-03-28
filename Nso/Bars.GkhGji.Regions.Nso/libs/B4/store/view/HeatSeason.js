//Todo файл перенесен поскольку добавляется новые колонки
Ext.define('B4.store.view.HeatSeason', {
    extend: 'B4.base.Store',
    fields: ['Id', 'HeatingSeasonId', 'Period', 'MU', 'Address', 'HeatSys', 'Type', 'MaxFl',
             'MinFl', 'NumEntr', 'AreaMkd', 'DateHeat', 'ActF', 'ActP', 'ActV',
             'ActC', 'ActR', 'Passport', 'Morg'],
    autoLoad: false,
    storeId: 'viewHeatSeasonStore',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HeatSeason',
        listAction: 'ListView'
    }
});