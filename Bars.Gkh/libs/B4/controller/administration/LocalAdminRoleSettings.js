Ext.define('B4.controller.administration.LocalAdminRoleSettings', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.RoleTreePermission',
        'B4.aspects.StateObjectsPermission',
    ],

    views: [
        'administration.localadminrolesettings.Panel',
    ],

    mainView: 'administration.localadminrolesettings.Panel',
    mainViewSelector: 'localadminrolesettingspanel',

    aspects: [
        {
            xtype: 'roletreepermissionaspect',
            permissionPanelView: 'localadminrolesettingspanel rolepermissionpanel',
            saveUrl: '/Permission/UpdateNodePermissions',
            copyUrl: '/Permission/CopyNodePermission',
            copyToRoleFromStore: true,
        },
        //{
        //    xtype: 'statetreepermissionaspect',
        //    permissionPanelView: 'localadminrolesettingspanel statepermissionpanel',
        //    saveUrl: '/StatePermission/UpdateNodePermissions',
        //},
        {
            xtype: 'stateobjectspermissionaspect',
            permissionPanelView: 'localadminrolesettingspanel stateobjectspanel',
            copyToRoleFromStore: true,
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'localadminrolesettingspanel'
        },
        {
            ref: 'tabPanel',
            selector: 'localadminrolesettingspanel tabpanel'
        },
    ],

    init: function () {
        var me = this,
            actions = {
                'localadminrolesettingspanel': {
                    afterrender: me.onAfterRender,
                    scope: me
                },
                '[name=RolePermissionTab] b4combobox[name=LocalAdmin]': {
                    change: me.onLocalAdminRoleChanged,
                    scope: me
                },
                '[name=StatePermissionTab] b4combobox[name=LocalAdmin]': {
                    change: me.onLocalAdminRoleChanged,
                    scope: me
                }
            };

        me.control(actions);
        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('localadminrolesettingspanel');

        me.bindContext(view);
        me.application.deployView(view);
    },

    onAfterRender: function () {
        var me = this,
            rolePanel = me.getTabPanel().down('[name=RolePermissionTab] > fieldset'),
            statePanel = me.getTabPanel().down('[name=StatePermissionTab] > fieldset'),
            rolePermissionCombobox = rolePanel.down('combobox[name=Role]'),
            statePermissionCombobox = statePanel.down('combobox[name=Role]');

        rolePanel.setDisabled(true);
        statePanel.setDisabled(true);

        if (rolePermissionCombobox) {
            rolePermissionCombobox.pageSize = 0;
            rolePermissionCombobox.queryMode = 'local';
        }
        if (statePermissionCombobox) {
            statePermissionCombobox.pageSize = 0;
            statePermissionCombobox.queryMode = 'local';
        }
    },

    onLocalAdminRoleChanged: function(combobox, idValue) {
        var me = this,
            store = combobox.getStore(),
            activeTab = me.getTabPanel().getActiveTab(),
            panel = activeTab.down('fieldset'),
            roleCombobox = activeTab.down('combobox[name=Role]'),
            roleStore = roleCombobox.getStore(),
            childRoles = {};

        childRoles = store.findRecord('Id', idValue).get('ChildRoles');

        panel.setDisabled(false);

        roleCombobox.clearValue();

        roleStore.load(function() { roleStore.loadData(childRoles) });
    }
});