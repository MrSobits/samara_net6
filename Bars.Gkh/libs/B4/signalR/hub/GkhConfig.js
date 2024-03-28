Ext.define('B4.signalR.hub.GkhConfig', {
    extend: 'B4.signalR.Hub',

    singleton: true,

    initHandlers: function (handlers) {
        handlers['UpdateParams'] = function (content) {
            Ext.merge(window.Gkh.config, Ext.decode(content));
        };
    }
});
