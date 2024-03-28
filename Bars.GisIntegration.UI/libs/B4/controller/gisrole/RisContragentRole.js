Ext.define('B4.controller.gisrole.RisContragentRole', {
    extend: 'B4.base.Controller',

    operatorId: 0,

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    models: [
        'gisrole.GisOperator',
        'gisrole.RisContragentRole'
    ],
    stores: [
        'gisrole.GisOperator',
        'gisrole.GisRoleForSelect',
        'gisrole.GisRoleSelected'

    ],
    views: [
        'riscontragentrole.Grid',
        'riscontragentrole.EditWindow',
        'riscontragentrole.RoleGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'gisoperatorgrid' }
    ],

    mainView: 'riscontragentrole.Grid',
    mainViewSelector: 'gisoperatorgrid',
    editWindowSelector: 'riscontragentroleeditwindow',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'gisoperatorgridWindowAspect',
            gridSelector: 'gisoperatorgrid',
            editFormSelector: 'riscontragentroleeditwindow',
            modelName: 'gisrole.GisOperator',
            editWindowView: 'riscontragentrole.EditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.setOperatorId(record.getId());
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    asp.controller.setOperatorId(record.getId());
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'risContragentRoleGridMultiSelectAspect',
            gridSelector: 'riscontragentrolegrid',
            modelName: 'gisrole.RisContragentRole',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#risContragentRoleGridMultiSelectWindow',
            storeSelect: 'gisrole.GisRoleForSelect',
            storeSelected: 'gisrole.GisRoleSelected',
            titleSelectWindow: 'Выбор ролей ГИС',
            titleGridSelect: 'Роли',
            titleGridSelected: 'Выбранные роли',
            columnsGridSelect: [
                {
                    text: 'Наименование',
                    flex: 2,
                    dataIndex: 'Name',
                    filter: { xtype: 'textfield' }
                }
            ],
            columnsGridSelected: [
                {
                    text: 'Наименование',
                    flex: 2,
                    dataIndex: 'Name',
                    filter: { xtype: 'textfield' }
                }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var roleIds = [];

                    records.each(function (rec) {
                        roleIds.push(rec.getId());
                    });

                    if (roleIds.length > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());

                        B4.Ajax.request(B4.Url.action('AddContragentRoles', 'RisContragentRole', {
                            roleIds: Ext.encode(roleIds),
                            operatorId: asp.controller.operatorId
                        })).next(function () {
                            asp.getGrid().getStore().load();
                            asp.controller.unmask();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка', 'Необходимо выбрать роли ГИС');
                        return false;
                    }

                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this;

        me.getStore('gisrole.GisOperator').on('beforeload', this.onBeforeLoad, this);

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('gisoperatorgrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    },

    setOperatorId: function (id) {
        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0],
            contragentrole = editWindow.down('riscontragentrolegrid');
        this.operatorId = id;

        if (id > 0) {
            contragentrole.setDisabled(false);
            contragentrole.getStore().filter('operatorId', id);
        } else {
            contragentrole.setDisabled(true);
        }
    },

    onBeforeLoad: function(store, operation) {
        operation.params.operatorId = this.operatorId;
    }
});