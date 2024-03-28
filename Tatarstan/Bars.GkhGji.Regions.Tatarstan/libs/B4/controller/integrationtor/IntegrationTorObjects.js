Ext.define('B4.controller.integrationtor.IntegrationTorObjects', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.model.integrationtor.Object',
        'B4.store.integrationtor.Object',
        'B4.view.integrationtor.ObjectsGrid'
        ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['integrationtor.Object'],
    stores: ['integrationtor.Object'],
    views: ['integrationtor.ObjectsGrid'],

    mainView: 'integrationtor.ObjectsGrid',
    mainViewSelector: 'integrationtorobjectsgrid',

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    },

});