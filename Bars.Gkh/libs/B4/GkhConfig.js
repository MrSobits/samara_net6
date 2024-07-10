Ext.define('Gkh',
{
    singleton: true,

    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
    bgColor: '#DFE9F6',
    borderColor: '#99BCE8',

    Object: {
        toBool: function(v) {
            return Ext.data.Types.BOOL.convert(v);
        }
    },

    signalR: {},
    config: {},

    constructor: function() {
        var me = this,
            connection = $.connection,
            gkhConfigHub = connection.gkhConfigHub,
            cacheCountHub = connection.countCacheHub,
            setConfig = function(config) {
                me.config = me.config || {};

                Ext.iterate(config,
                    function(k, v) {
                        var parts = k.split('.'),
                            cfg = me.config;

                        for (var i = 0; i < parts.length - 1; i++) {
                            var part = parts[i];
                            cfg[part] = cfg[part] || {};
                            cfg = cfg[part];
                        }

                        cfg[parts[parts.length - 1]] = v;
                    });
            };

        gkhConfigHub.client.updateParams = function(params) {
            setConfig($.parseJSON(params));
        };

        cacheCountHub.client.clearCache = function(params) {
            B4.CountCacheQueue.enqueue(params);
        }

        Ext.applyIf(me.signalR, {
          
            start: function () {
                
               // connection.hub.start();
            },
            stop: function() {
            //    connection.hub.stop();
            }
        });

        $.get(rootUrl + 'action/GkhConfig/GetAllConfigs', setConfig);
    }
});
