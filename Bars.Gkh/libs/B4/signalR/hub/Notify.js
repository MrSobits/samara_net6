Ext.define('B4.signalR.hub.Notify', {
    extend: 'B4.signalR.Hub',

    singleton: true,

    requires: [
        'B4.notify.NotifyController',
    ],

    initHandlers: function (handlers) {
        handlers['SendMessage'] = function (message) {
            B4.notify.NotifyController.sendMessage(message);
        };
        
        handlers['CloseWindow'] = function (messageId) {
            B4.notify.NotifyController.closeWindow(messageId);
        };
    }
});
