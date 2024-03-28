Ext.define('B4.signalR.hub.CountCache', {
    extend: 'B4.signalR.Hub',

    singleton: true,

    requires: [
        'B4.CountCacheQueue',
    ],

    initHandlers: function (handlers) {
        handlers['ClearCache'] = function (key) {
            B4.CountCacheQueue.enqueue(key);
        };
    }
});
