Ext.define('B4.store.version.ActualizeSubProgramByFiltersAdd', {
    extend: 'B4.base.Store',
    requires: ['B4.model.version.ActualizeSubProgramByFiltersAdd'],
    autoLoad: false,
    model: 'B4.model.version.ActualizeSubProgramByFiltersAdd',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActualizeSubDPKR',
        listAction: 'getaddentrieslist',
        timeout: 30 * 60 * 1000, // 30 минут
        listeners: {
            exception: function (proxy, response, operation) {
                B4.QuickMsg.msg('Ошибка', 'Ошибка загруки списка домов на добавление', 'error');
                //this.clear();
            }
        }
    },
});