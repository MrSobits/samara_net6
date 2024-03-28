Ext.define('B4.controller.RolePermission', {
    extend: 'B4.base.Controller',
    views: ['Permission.MainPanel'],

    requires: [
        'B4.aspects.RoleTreePermission',
        'B4.view.Permission.CopyRoleWindow'
    ],

    aspects: [
        {
            xtype: 'roletreepermissionaspect',
            permissionPanelView: 'rolepermissionmainpanel'
        },
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'Permission.MainPanel',
    mainViewSelector: 'rolepermissionmainpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'rolepermissionmainpanel'
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('rolepermissionmainpanel');

        me.bindContext(view);
        me.application.deployView(view);
    },

});