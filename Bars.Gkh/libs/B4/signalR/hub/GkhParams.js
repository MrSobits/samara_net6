Ext.define('B4.signalR.hub.GkhParams', {
    extend: 'B4.signalR.Hub',

    singleton: true,

    initHandlers: function (handlers) {
        handlers['UpdateParams'] = function (parameters) {
            // TODO: не нашел обработчик
        };
    }
});
