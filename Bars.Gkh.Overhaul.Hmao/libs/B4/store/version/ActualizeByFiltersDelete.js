Ext.define('B4.store.version.ActualizeByFiltersDelete', {
    extend: 'B4.base.Store',
    requires: ['B4.model.version.ActualizeByFiltersDelete'],
    autoLoad: false,
    model: 'B4.model.version.ActualizeByFiltersDelete',
    groupField: 'Address',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActualizeDPKR',
        listAction: 'getdeleteentrieslist',
        timeout: 30 * 60 * 1000, // 30 минут
        listeners: {
            exception: function (proxy, response, operation) {
                B4.QuickMsg.msg('Ошибка', 'Ошибка загруки списка домов на удаление', 'error');
                //this.clear();
            }
        }
    },
});