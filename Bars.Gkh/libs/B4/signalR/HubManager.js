Ext.define('B4.signalR.HubManager', {
    singleton: true,

    // Сюда прописываем хабы явно, т.к. в случае отключенного бандлера они не будут заранее загружены
    requires: [
        'B4.signalR.hub.CountCache',
        'B4.signalR.hub.GkhConfig',
        'B4.signalR.hub.GkhParams',
        'B4.signalR.hub.Notify',
        'B4.signalR.hub.ProsecutorsOffice',
        'B4.signalR.hub.ReportStatus',
    ],

    hubs: {},
    hubsCollection: new Ext.util.MixedCollection(),

    init: function () {
        const me = this;

        me.hubs = Ext
            .ClassManager
            .get('B4.signalR.hub') || {};

        Ext.iterate(me.hubs, (name, hub) => {
            hub.init();
            me.hubsCollection.add(hub.name || name, hub);
        });
    },

    get: function (hubName) {
        hubName = hubName || '';

        return this.hubs[hubName]
            || this.hubsCollection.get(hubName);
    },
    
    start: function () {
        Ext.iterate(this.hubs, (name, hub) => hub.start());
    },

    stop: function () {
        Ext.iterate(this.hubs, (name, hub) => hub.stop());
    },
});
