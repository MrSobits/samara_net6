Ext.define('B4.notify.NotifyController', {
    singleton: true,

    requires: [
        'B4.view.administration.notify.Window'
    ],

    getWindow: function(id) {
        return Ext.ComponentQuery.query(Ext.String.format('notifywindow[messageId={0}]', id))[0];
    },

    sendMessage: function (params) {
        var me = this,
            data = Ext.decode(params);

        if (Ext.isObject(data) && Ext.isString(data.Text)) {
            me.createWindow(data.Id, data.Title || 'Уведомление системы', data.Text, data.ButtonSet);
        }
    },

    closeWindow: function(params) {
        var me = this,
            data = Ext.decode(params),
            window = me.getWindow(data.messageId);

        if (window) {
            window.close();
        }
    },

    createWindow: function (id, title, message, buttonSet) {
        var me = this,
            window = me.getWindow(id) ||
                Ext.create('B4.view.administration.notify.Window', {
                    messageId: id,
                    title: title,
                    text: decodeURI(message),
                    buttonSet: buttonSet
                });

        window.show();
    }
});