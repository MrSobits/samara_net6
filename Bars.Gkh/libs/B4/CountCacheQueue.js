Ext.define('B4.CountCacheQueue', {
    singleton: true,
    _freeze: {},
    _update: {},

    enqueue: function(params) {
        var me = this,
            obj = params ? Ext.decode(params) : null,
            key = obj ? obj.key : null;

        if (!key) {
            return;
        }

        me._sendRequest(key);
    },

    _sendRequest: function (key) {
        var me = this;

        if (me._freeze[key]) {
            if (!me._update[key]) {
                me._update[key] = true;

                setTimeout(function () {
                    me._sendRequest(key);
                }, 2000);
            }
            return;
        }

        me._freeze[key] = true;
        delete me._update[key];

        B4.Ajax.request({
            url: rootUrl + 'action/CountCache/Invalidate',
            params: {
                key: key
            }
        }).next(function () {
            setTimeout(function() {
                delete me._freeze[key];
            }, 2000);
        });
    }
});