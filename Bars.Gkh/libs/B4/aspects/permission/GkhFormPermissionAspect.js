Ext.define('B4.aspects.permission.GkhFormPermissionAspect', {
    extend: 'B4.base.Aspect',
    alias: 'widget.formpermissionaspect',
    requires: [
        'B4.view.formpermission.Grid',
        'B4.view.formpermission.Window'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    buttonSelector: null,

    currentController: null,

    init: function (controller) {
        var me = this,
            actions = {};

        actions[me.buttonSelector] = { 'click': { fn: me.showWindow, scope: me }};
        actions['formpermissionwindow'] = {
             'typeStore.beforeload': { fn: me.onStoreBeforeLoad, scope: me },
             'typeStore.load': { fn: me.onTypeStoreLoad, scope: me }
        };
        actions['formpermissionwindow formpermissiongrid'] = { 'store.beforeload': { fn: me.onStoreBeforeLoad, scope: me } };
        actions['formpermissionwindow b4savebutton'] = { 'click': { fn: me.saveRecords, scope: me } };
        actions['formpermissionwindow b4updatebutton'] = { 'click': { fn: me.loadStore, scope: me } };
        actions['formpermissionwindow button[action=selectAll]'] = { 'click': { fn: me.selectAll, scope: me } };
        actions['formpermissionwindow button[action=deselectAll]'] = { 'click': { fn: me.deselectAll, scope: me } };
        actions['formpermissionwindow combobox[name=Role]'] = { 'change': { fn: me.onRoleChange, scope: me } };
        actions['formpermissionwindow combobox[name=EntityType]'] = { 'change': { fn: me.onTypeChange, scope: me } };
        actions['formpermissionwindow combobox[name=State]'] = { 'change': { fn: me.loadStore, scope: me } };
        actions['formpermissionwindow checkbox[name=Stateful]'] = { 'change': { fn: me.onStatefulChange, scope: me } };

        controller.control(actions);
        me.callParent(arguments);
    },

    getCurrentController: function () {
        var me = this;

        if (!me.currentController) {
            var router = b4app.getRouter(),
                history = router.getHistory(),
                currentToken = history.currentToken || 'B4.controller.PortalController',
                currentRoute = router.recognize(currentToken),
                controllerName = currentRoute.controller,
                controller = router.getController(controllerName || currentToken);

            me.currentController = controller;
        }

        return me.currentController;
    },

    showWindow: function (btn) {
        var me = this,
        token = Ext.History.currentToken,
        existsWindow = Ext.ComponentQuery.query('formpermissionwindow')[0];

        if (existsWindow) {
            if (existsWindow.token === token) {
                return;
            } else {
                existsWindow.destroy();
            }
        }

        var permissionAspects = me.getPermissionAspects();

        if (permissionAspects.length > 0) {
            var window = Ext.widget('formpermissionwindow');
            window.token = token;
            var stateFull = false;
            Ext.each(permissionAspects,
            function (asp) {
                if (asp.xtypesMap.statepermissionaspect) {
                    stateFull = true;
                }
            });

            window.down('checkbox[name = Stateful]').setVisible(stateFull);
            window.show();
        } else {
            B4.QuickMsg.msg('Внимание!',
                'Для настройки прав доступа перейдите в раздел "Администрирование/Настройка прав доступа/Настройки ограничений"', 'warning');
        }
    },
     
    onStoreBeforeLoad: function(store, operation) {
        Ext.apply(operation.params, this.getLoadParams());
    },

    onTypeStoreLoad: function(store, records) {
        var field = Ext.ComponentQuery.query('formpermissionwindow combobox[name=EntityType]')[0];

        if (records.length > 0) {
            var typeId = records[0].get('TypeId');
            field.setValue(typeId);
            field.fireEvent('change', field, typeId);
        }
    },

    getLoadParams: function() {
        var me = this,
            permissionAspects = me.getPermissionAspects(),
            permissionKeys = [];

        Ext.each(permissionAspects,
            function(asp) {
                permissionKeys = asp.xtypesMap.gkhgridpermissionaspect 
                    ? permissionKeys.concat(asp.collectPermissions(asp.addPrefix()))
                    : permissionKeys.concat(asp.collectPermissions());
            });

        var params = {
            stateful: me.getParam('Stateful'),
            roleId: me.getParam('Role'),
            typeId: me.getParam('EntityType'),
            stateId: me.getParam('State'),
            formPermissions: Ext.encode(permissionKeys)
        };

        return params;
    },

    getPermissionAspects: function() {
        return this.getCurrentController().aspectCollection
            .filterBy(function(asp) {
                return asp.xtypesMap.permissionaspect;
            })
            .getRange();
    },

    saveRecords: function (btn) {
        var me = this,
            grid = btn.up('window').down('grid'),
            permissionsData = this.collectPermissionsData(grid);

        if (me.getParam('Stateful')) {
            me.saveStateful(permissionsData, grid);
        } else {
            me.saveStateless(permissionsData, grid);
        }
    },

    saveStateless: function (permissionsData, grid) {
        var me = this;

        me.mask('Загрузка', grid.up('window'));
        Ext.Ajax.request({
            url: B4.Url.action('/Permission/UpdatePermissions'),
            params: {
                roleId: me.getParam('Role'),
                permissions: Ext.encode(permissionsData)
            },
            success: function () {
                grid.getStore().load();
                me.unmask();
            },
            failure: function () {
                Ext.Msg.alert('Ошибка', 'При сохранении произошла ошибка');
                me.unmask();
            }
        });
    },

    saveStateful: function (permissionsData, grid) {
        var me = this;

        me.mask('Загрузка', grid.up('window'));
        Ext.Ajax.request({
            url: B4.Url.action('/StatePermission/UpdatePermissions'),
            params: {
                roleId: me.getParam('Role'),
                stateId: me.getParam('State'),
                permissions: Ext.encode(permissionsData)
            },
            success: function () {
                grid.getStore().load();
                me.unmask();
            },
            failure: function () {
                Ext.Msg.alert('Ошибка', 'При сохранении произошла ошибка');
                me.unmask();
            }
        });
    },

    selectAll: function(btn) {
        var me = this,
            grid = btn.up('window').down('grid'),
            store = grid.getStore();

        Ext.each(store.getRange(), function(rec) { rec.set('Grant', true); });
    },

    deselectAll: function (btn) {
        var me = this,
            grid = btn.up('window').down('grid'),
            store = grid.getStore();

        Ext.each(store.getRange(), function (rec) { rec.set('Grant', false); });
    },

    loadStore: function(component) {
        var store = component.up('window').down('grid').getStore();
        
        store.load();
    },

    collectPermissionsData: function (grid) {
        var result = {},
            records = grid.getStore().getModifiedRecords();

        Ext.each(records,
            function (rec) {
                result[rec.get('PermissionId')] = rec.get('Grant');
            });

        return result;
    },

    onRoleChange: function (field, newValue) {
        var window = field.up('window'),
            store = window.down('grid').getStore(),
            selectButtonGroup = window.down('buttongroup[name=selectButtonGroup]');

        store.load();
        selectButtonGroup.enable();
    },

    onStatefulChange: function(checkbox, newValue) {
        var me = this,
            window = checkbox.up('window'),
            entityTypeField = window.down('combobox[name=EntityType]'),
            stateField = window.down('combobox[name=State]'),
            grid = window.down('grid'),
            store = grid.getStore();

        store.clearData();
        grid.getView().refresh();

        if (newValue === true) {
            entityTypeField.getStore().load();
        }

        entityTypeField.setVisible(newValue);
        stateField.setVisible(newValue);
    },

    getParam: function(fieldName) {
        return Ext.ComponentQuery.query('formpermissionwindow field[name=' + fieldName + ']')[0].getValue();
    },

    onTypeChange: function(comboBox, newValue) {
        var stateField = comboBox.up().down('combobox[name=State]'),
            store = stateField.getStore();

        stateField.setValue(null);
        store.load({
            params: {
                entityTypeId: newValue
            }
        });
        stateField.enable();
    }
});