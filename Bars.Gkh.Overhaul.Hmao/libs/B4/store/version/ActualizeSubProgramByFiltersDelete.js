Ext.define('B4.store.version.ActualizeSubProgramByFiltersDelete', {
    extend: 'B4.base.Store',
    requires: ['B4.model.version.ActualizeSubProgramByFiltersDelete'],
    autoLoad: false,
    model: 'B4.model.version.ActualizeSubProgramByFiltersDelete',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActualizeSubDPKR',
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