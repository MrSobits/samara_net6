Ext.define('B4.controller.RoleTypeHousePermission', {
    extend: 'B4.base.Controller',
    views: ['roletypehousepermission.Panel'],
    models: ['RoleTypeHousePermission'],
    stores: ['RoleTypeHousePermission'],
   
    mainView: 'roletypehousepermission.Panel',
    mainViewSelector: '#roletypehousepermissionpanel',
    
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('roletypehousepermissionpanel');
        this.bindContext(view);
        this.application.deployView(view);
    },

    init: function () {
        this.control({
            'roletypehousepermissionpanel #cbRole': {
                change: this.onRoleChanged
            },

            'roletypehousepermissionpanel #btnSavePermissions': {
                click: this.savePermissions
            },

            'roletypehousepermissionpanel #btnMarkAll': {
                click: this.markAllPermissions
            },

            'roletypehousepermissionpanel #btnUnmarkAll': {
                click: this.unmarkAllPermissions
            }
        });
        
        this.callParent(arguments);
    },

    savePermissions: function (button) {
        var me = this,
            panel = button.up('roletypehousepermissionpanel'),
            store = panel.down('roletypehousepermissionGrid').getStore('RoleTypeHousePermission'),
            combo = panel.down('#cbRole'),
            data = [];

        me.mask('Сохранение...', panel);
        
        store.each(function (rec) {
            data.push({ Code: rec.get('Code'), Allowed: rec.get('Allowed') });
        });
        
        B4.Ajax.request({
            url: B4.Url.action('/RoleTypeHousePermission/Update'),
            params: {
                roleId: combo.getValue(),
                permissions: Ext.encode(data)
            },
            method: 'POST'
        }).next(function () {
            me.unmask();
            store.reload();
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка', 'При сохранении произошла ошибка!');
        });
    },

    markAllPermissions: function (button) {
        var panel = button.up('roletypehousepermissionpanel'),
            store = panel.down('roletypehousepermissionGrid').getStore('RoleTypeHousePermission');
        
        store.each(function (rec) {
            rec.set('Allowed', true);;
        });
    },

    unmarkAllPermissions: function (button) {
        var panel = button.up('roletypehousepermissionpanel'),
            store = panel.down('roletypehousepermissionGrid').getStore('RoleTypeHousePermission');
        
        store.each(function (rec) {
            rec.set('Allowed', false);
        });
    },

    onRoleChanged: function (field, newValue) {
        this.getStore('RoleTypeHousePermission').load({ params: { roleId: newValue } });
    }
});