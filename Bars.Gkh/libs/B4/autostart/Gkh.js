Ext.define('B4.autostart.Gkh', {
    singleton: true,

    requires: [
        'B4.signalR.HubManager',
    ],

    onBeforeCreate: {
        fn: function (b4Config) {
            B4.autostart.Gkh.initSignalR();
        }
    },

    onAfterLaunch: {
        fn: function (b4Config) {

        }
    },
    onAppReady: {
        fn: function (b4Config) {

        }
    },

    initSignalR: function() {
        B4.signalR.HubManager.init();

        window.Gkh = Ext.apply(window.Gkh || {}, {
            signalR: {
                hubs: B4.signalR.HubManager.hubs,
                getHub: (hubName) => B4.signalR.HubManager.get(hubName),
                start: B4.signalR.HubManager.start,
                stop: B4.signalR.HubManager.stop,

                onNotifyButtonClick: (id, button) => B4.signalR.HubManager.hubs.Notify.invoke('OnButtonClick', [id, button]),
            }
        });
    }
});