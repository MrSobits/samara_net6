Ext.define('B4.aspects.RoleTreePermission', {
    extend: 'B4.aspects.TreePermission',
    alias: 'widget.roletreepermissionaspect',

    requires: [
        'B4.view.Permission.CopyRoleWindow',
        'B4.model.Role'
    ],

    getRolesComboBox: function () {
        return Ext.ComponentQuery.query(this.permissionPanelView + ' combobox[name=Role]')[0];
    },

    init: function (controller) {
        var asp = this,
            actions = {},
            winSelector = 'copyrolewindow[ctxKey='
                + controller.getCurrentContextKey() + '][name=CopyRolePermissions]';

        actions[asp.permissionPanelView + ' button[name=CopyRole]'] = { 'click': { fn: asp.onCopyRole, scope: asp } };
        actions[asp.permissionPanelView + ' combobox[name=Role]'] = { 'change': { fn: asp.onRoleChanged, scope: asp } };
        actions[winSelector + ' b4savebutton'] = { 'click': { fn: asp.copyRolePermissions, scope: asp } };
        actions[winSelector + ' b4closebutton'] = { 'click': { fn: asp.closeCopyRoleWindow, scope: asp } };

        asp.callParent(arguments);
        controller.control(actions);
    },

    savePermissions: function () {
        var asp = this,
            permissionsData = asp.collectPermissionsData(),
            tree = asp.getPermissionsTreePanel(),
            combo = asp.getRolesComboBox(),
            url = asp.saveUrl || '/Permission/UpdatePermissions';

        tree.disable();
        combo.disable();

        Ext.Ajax.request({
            url: B4.Url.action(url),
            params: {
                roleId: combo.getValue(),
                permissions: Ext.encode(permissionsData)
            },
            success: function () {
                tree.enable();
                combo.enable();
                B4.QuickMsg.msg('Сохранение', 'Настройки успешно сохранены', 'success');
            },
            failure: function () {
                tree.enable();
                combo.enable();
                Ext.Msg.alert('Ошибка', 'При сохранении произошла ошибка!');
            }
        });
    },

    onRoleChanged: function (field, newValue) {
        var asp = this,
            treePanel = asp.getPermissionsTreePanel(),
            store = treePanel.getStore();

        treePanel.setDisabled(newValue === null);

        store.load({ params: { roleId: newValue } });
    },

    onCopyRole: function () {
        var asp = this,
            roleCombobox = asp.getRolesComboBox(),
            roleValue = roleCombobox.getValue(),
            store = asp.copyToRoleFromStore
                        ? Ext.create('Ext.data.Store', {
                            model: 'B4.model.Role',
                            proxy: 'memory'
                        })
                        : null,
            win = Ext.widget('copyrolewindow', {
                name: 'CopyRolePermissions',
                roleStore: store,
                ctxKey: asp.controller.getCurrentContextKey()
            }),
            roleToCopyCombobox = win.down('combobox[name=Role]');

        if (store) {
            if (roleToCopyCombobox) {
                roleToCopyCombobox.pageSize = 0;
                roleToCopyCombobox.queryMode = 'local';
            }

            store.load(function () { store.loadRecords(roleCombobox.getStore().getRange()) });
        }

        if (!roleValue) {
            Ext.Msg.alert('Сообщение', 'Для копирования настроек, необходимо выбрать роль');
            return;
        }

        win.show();
    },

    closeCopyRoleWindow: function (btn) {
        btn.up('copyrolewindow').close();
    },

    copyRolePermissions: function (btn) {
        var asp = this,
            copyRoleWin = btn.up('copyrolewindow'),
            copyRoleForm = copyRoleWin.getForm(),
            toRoleId = copyRoleForm.getValues().Role,
            fromRoleId = asp.getRolesComboBox().value,
            url = asp.copyUrl || '/Permission/FiltredCopyRolePermission';

        if (!copyRoleForm.isValid) {
            Ext.Msg.alert('Ошибка копирования', 'Не заполнено обязательное поле "Роль"');
            return;
        }

        if (toRoleId == fromRoleId) {
            Ext.Msg.alert('Ошибка копирования', 'Роль из которой производится копирование и в которую копируют совпадают');
            return;
        }

        asp.mask('Сохранение', asp.controller.getMainComponent());
        B4.Ajax.request({
            method: 'POST',
            timeout: 3600,
            url: url = B4.Url.action(url),
            params: {
                fromRoleId: fromRoleId,
                toRoleId: toRoleId
            },
        }).next(function () {
            Ext.Msg.alert('Сохранение!', 'Настройки роли успешно скопированы');
            asp.unmask();
            copyRoleWin.close();
        }).error(function (response) {
            var msg = response.message || 'Произошла ошибка при копировании';
            Ext.Msg.alert("Ошибка", msg);
            asp.unmask();
        });
    }
});