Ext.define('B4.aspects.StateTreePermission', {
    extend: 'B4.aspects.TreePermission',
    alias: 'widget.statetreepermissionaspect',

    requires: [
        'B4.view.StatePermission.CopyWindow',
        'B4.model.Role',
        'B4.model.StateByType'
    ],

    getPermissionsTreePanel: function () {
        return Ext.ComponentQuery.query(this.permissionPanelView + ' treepanel[name=Permissions]')[0];
    },

    getObjectTypeComboBox: function () {
        return Ext.ComponentQuery.query(this.permissionPanelView + ' combobox[name=ObjectType]')[0];
    },

    getObjectStateComboBox: function () {
        return Ext.ComponentQuery.query(this.permissionPanelView + ' combobox[name=ObjectState]')[0];
    },

    getRolesComboBox: function () {
        return Ext.ComponentQuery.query(this.permissionPanelView + ' combobox[name=Role]')[0];
    },

    init: function (controller) {
        var asp = this,
            actions = {},
            winSelector = 'statepermissioncopywindow[ctxKey='
                + controller.getCurrentContextKey() + '][name=CopyStatePermissions]';

        actions[asp.permissionPanelView + ' combobox[name=ObjectType]'] = { 'change': { fn: asp.onObjectTypeChange, scope: asp } };
        actions[asp.permissionPanelView + ' combobox[name=ObjectState]'] = { 'change': { fn: asp.loadTree, scope: asp } };
        actions[asp.permissionPanelView + ' combobox[name=Role]'] = { 'change': { fn: asp.onRoleChange, scope: asp } };
        actions[asp.permissionPanelView + ' button[name=CopyRole]'] = { 'click': { fn: asp.onCopyPermissions, scope: asp } };
        actions[winSelector + ' b4savebutton'] = { 'click': { fn: asp.copyPermissions, scope: asp } };
        actions[winSelector + ' b4closebutton'] = { 'click': { fn: asp.closeCopyWindow, scope: asp } };

        asp.callParent(arguments);
        controller.control(actions);
    },

    savePermissions: function () {
        var asp = this,
            permissionsData = asp.collectPermissionsData(),
            tree = asp.getPermissionsTreePanel(),
            stateCb = asp.getObjectStateComboBox(),
            roleCb = asp.getRolesComboBox(),
            typeCb = asp.getObjectTypeComboBox(),
            url = asp.saveUrl || '/StatePermission/UpdatePermissions';

        tree.disable();
        stateCb.disable();
        roleCb.disable();
        typeCb.disable();

        Ext.Ajax.request({
            url: B4.Url.action(url),
            params: {
                roleId: roleCb.getValue(),
                stateId: stateCb.getValue(),
                permissions: Ext.encode(permissionsData)
            },
            success: function (response) {
                tree.enable();
                stateCb.enable();
                roleCb.enable();
                typeCb.enable();
                B4.QuickMsg.msg('Сохранение', 'Настройки успешно сохранены', 'success');
            },
            failure: function (response) {
                tree.enable();
                stateCb.enable();
                roleCb.enable();
                typeCb.enable();
                Ext.Msg.alert('Ошибка', 'При сохранении произошла ошибка!');
            }
        });
    },

    onRoleChange: function (comboBox, newValue) {
        var me = this;

        if (newValue > 0) {
            me.getObjectTypeComboBox().setValue(null);
            me.getStateObjects(newValue);
        }
    },

    getStateObjects: function (roleId) {
        var asp = this,
            typeComboboxStore = asp.getObjectTypeComboBox().getStore();

        Ext.Ajax.request({
            url: B4.Url.action('/State/FiltredStatefulEntityList'),
            params: {
                roleId: roleId
            },
            success: function (response) {
                var json = Ext.JSON.decode(response.responseText);
                if (json) {
                    typeComboboxStore.load(function () { typeComboboxStore.loadData(json.data) });
                };
            }
        });
    },

    onObjectTypeChange: function (comboBox, newValue, oldValue) {
        var asp = this,
            stateCb = asp.getObjectStateComboBox(),
            store = stateCb.getStore();

        stateCb.clearFilter(true);
        stateCb.clearValue();
        store.load({
            params: {
                entityTypeId : newValue
            }
        });

        stateCb.enable();
        asp.loadTree();
    },

    loadTree: function() {
        var asp = this,
            treePanel = asp.getPermissionsTreePanel(),
            stateValue = asp.getObjectStateComboBox().getValue() || 0,
            roleValue = asp.getRolesComboBox().getValue() || 0,
            typeValue = asp.getObjectTypeComboBox().getValue() || '',
            store = treePanel.getStore();

        if (roleValue > 0 && stateValue > 0 && typeValue.length > 0) {
            treePanel.disable();

            store.load({ params: { roleId: roleValue, stateId: stateValue, typeId: typeValue } });
            treePanel.getRootNode().expand(true, false);

            treePanel.enable();
        } else {
            treePanel.disable();
            store.getRootNode().removeAll();
        }
    },

    onCopyPermissions: function () {
        var asp = this,
            roleCombobox = asp.getRolesComboBox(),
            objectStateCombobox = asp.getObjectStateComboBox(),
            roleValue = roleCombobox.getValue(),
            roleStore = asp.copyToRoleFromStore
                            ? Ext.create('Ext.data.Store', {
                                model: 'B4.model.Role',
                                proxy: 'memory'
                            })
                            : null,
            statusStore = Ext.create('Ext.data.Store', {
                model: 'B4.model.StateByType',
                proxy: 'memory'
            }),
            win = Ext.widget('statepermissioncopywindow', {
                name: 'CopyStatePermissions',
                roleStore: roleStore,
                statusStore: statusStore,
                ctxKey: asp.controller.getCurrentContextKey()
            }),
            roleToCopyCombobox = win.down('combobox[name=Role]');

        if (roleStore) {
            if (roleToCopyCombobox) {
                roleToCopyCombobox.pageSize = 0;
                roleToCopyCombobox.queryMode = 'local';
            }

            roleStore.load(function () { roleStore.loadRecords(roleCombobox.getStore().getRange()) });
        }

        statusStore.on('load', function (store) {
            store.loadRecords(objectStateCombobox.getStore().getRange());
        });

        if (!roleValue) {
            Ext.Msg.alert('Сообщение', 'Для копирования настроек, необходимо выбрать роль');
            return;
        }

        win.show();
    },

    closeCopyWindow: function (btn) {
        btn.up('statepermissioncopywindow').close();
    },

    copyPermissions: function (btn) {
        var asp = this,
            copyRoleWin = btn.up('statepermissioncopywindow'),
            copyRoleForm = copyRoleWin.getForm(),
            formValues = copyRoleForm.getValues(),
            toRoleId = formValues.Role,
            toStateId = formValues.State,
            fromRoleId = asp.getRolesComboBox().value,
            fromStateId = asp.getObjectStateComboBox().value,
            fromTypeId = asp.getObjectTypeComboBox().value,
            url = asp.copyUrl || '/StatePermission/FiltredCopyRolePermission';;

        if (!copyRoleForm.isValid) {
            Ext.Msg.alert('Ошибка копирования', 'Не заполнены обязательные поля');
            return;
        }

        if (toRoleId == fromRoleId && toStateId == fromStateId) {
            Ext.Msg.alert('Ошибка копирования', 'Роль и статус назначения совпадают с объектами копирования');
            return;
        }

        asp.mask('Сохранение', asp.controller.getMainComponent());
        B4.Ajax.request({
            method: 'POST',
            timeout: 60 * 1000 * 5,
            url: B4.Url.action(url),
            params: {
                fromRoleId: fromRoleId,
                toRoleId: toRoleId,
                fromStateId: fromStateId,
                toStateId: toStateId,
                fromTypeId: fromTypeId
            }
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