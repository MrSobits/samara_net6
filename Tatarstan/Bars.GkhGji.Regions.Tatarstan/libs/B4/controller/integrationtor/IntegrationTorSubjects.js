Ext.define('B4.controller.integrationtor.IntegrationTorSubjects', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.model.integrationtor.Subject',
        'B4.store.integrationtor.Subject',
        'B4.view.integrationtor.SubjectsGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['integrationtor.Subject'],
    stores: ['integrationtor.Subject'],
    views: ['integrationtor.SubjectsGrid'],

    mainView: 'integrationtor.SubjectsGrid',
    mainViewSelector: 'integrationtorsubjectsgrid',

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    },

});