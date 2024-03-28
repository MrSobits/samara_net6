Ext.define('B4.controller.StatePermission', {
    extend: 'B4.base.Controller',
    views: ['StatePermission.MainPanel'],

    requires: [
        'B4.aspects.StateTreePermission',
    ],

    aspects: [
        {
            xtype: 'statetreepermissionaspect',
            permissionPanelView: 'statepermissionmainpanel'
        },
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'StatePermission.MainPanel',
    mainViewSelector: 'statepermissionmainpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'statepermissionmainpanel'
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('statepermissionmainpanel');

        me.bindContext(view);
        me.application.deployView(view);
    },

});