Ext.define('B4.store.version.ActualizeByFiltersAdd', {
    extend: 'B4.base.Store',
    requires: ['B4.model.version.ActualizeByFiltersAdd'],
    autoLoad: false,
    model: 'B4.model.version.ActualizeByFiltersAdd',
    groupField: 'Address',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActualizeDPKR',
        listAction: 'getaddentrieslist',
        timeout: 30 * 60 * 1000, // 30 минут
        listeners: {            
            exception: function (proxy, response, operation)
            {       
                var me = this;

                B4.QuickMsg.msg('Ошибка', 'Ошибка загруки списка домов на добавление', 'error');
                //me.clear();
            }
        }
    },
});