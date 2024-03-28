Ext.define('B4.controller.Role', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.enums.YesNo'
    ],

    views: [
        'Role.Grid',
        'Role.EditWindow'
    ],

    models: ['Role'],
    stores: ['Role'],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'Role.Grid',
    mainViewSelector: 'rolegrid',
    refs: [
        {
            ref: 'mainView',
            selector: 'rolegrid'
        },
        {
            ref: 'editWindow',
            selector: 'roleeditwindow'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'roleGridAspect',
            gridSelector: 'rolegrid',
            editFormSelector: 'roleeditwindow',
            storeName: 'Role',
            modelName: 'Role',
            editWindowView: 'Role.EditWindow',
            listeners: {
                beforesave: function (asp, record) {
                    var id = record.get('Id') || 0,
                        roleList = record.get('RoleList') || [],
                        form = asp.getForm(),
                        isLocalAdmin = form.down('[name=IsLocalAdmin]');

                    if (roleList.indexOf(id) !== -1) {
                        Ext.Msg.alert('Ошибка', 'Выбранная роль локального администратора присутствует в списке настраиваемых ролей');
                        return false;
                    }

                    if (isLocalAdmin.checked === false) {
                        record.set('RoleList', []);
                    }

                    return true;
                },
                aftersetformdata: function (asp, record) {
                    var form = asp.getForm(),
                        isLocalAdmin = form.down('[name=IsLocalAdmin]');

                    isLocalAdmin.setValue(record.get('LocalAdmin') === B4.enums.YesNo.Yes)
                },
            }
        }
    ],

    init: function () {
        var me = this,
            actions = {
                'roleeditwindow checkbox[name=IsLocalAdmin]': {
                    change: me.onChangeLocalAdmin,
                    scope: me
                },
            };

        me.control(actions);
        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('rolegrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    },

    onChangeLocalAdmin: function (checkbox) {
        var me = this,
            win = me.getEditWindow(),
            roleList = win.down('b4selectfield[name=RoleList]'),
            localAdminRoleId = win.getForm().getRecord().get('Id'),
            store = roleList.getStore();

        roleList.setVisible(checkbox.getValue());
        roleList.setDisabled(!checkbox.getValue());
        store.on('load', function(store, records) {
            var me = this,
                excludeRecord = {};

            excludeRecord = records.filter(function(rec) {
                if (rec.get('Id') === localAdminRoleId) {
                    return true;
                } else {
                    return false;
                }
            });
            store.remove(excludeRecord);
        }, me);
    }
});