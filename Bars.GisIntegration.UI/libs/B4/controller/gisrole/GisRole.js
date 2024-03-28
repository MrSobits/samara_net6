Ext.define('B4.controller.gisrole.GisRole', {
    extend: 'B4.base.Controller',

    roleId: 0,

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GridEditWindow'
    ],

    models: [
        'gisrole.GisRole',
        'gisrole.GisRoleMethod'
    ],
    stores: [
        'gisrole.GisRole',
        'gisrole.GisRoleMethod',
        'gisrole.MethodForSelect',
        'gisrole.MethodSelected'

    ],
    views: [
        'gisrole.Grid',
        'gisrole.EditWindow',
        'gisrole.MethodGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'gisrolegrid' }
    ],

    mainView: 'gisrole.Grid',
    mainViewSelector: 'gisrolegrid',
    editWindowSelector: 'gisroleeditwindow',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'gisRoleGridWindowAspect',
            gridSelector: 'gisrolegrid',
            editFormSelector: 'gisroleeditwindow',
            modelName: 'gisrole.GisRole',
            editWindowView: 'gisrole.EditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentRoleId(record.getId());
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    asp.controller.setCurrentRoleId(record.getId());
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'gisrolemethodGridMultiSelectAspect',
            gridSelector: 'gisrolemethodgrid',
            storeName: 'gisrole.GisRoleMethod',
            modelName: 'gisrole.GisRoleMethod',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#gisrolemethodGridMultiSelectWindow',
            storeSelect: 'gisrole.MethodForSelect',
            storeSelected: 'gisrole.MethodSelected',
            titleSelectWindow: 'Выбор методов интеграции ГИС',
            titleGridSelect: 'Методы',
            titleGridSelected: 'Выбранные методы',
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
                    var methods = [];

                    records.each(function (rec) {
                        methods.push({ Name: rec.get('Name'), Id: rec.get('Id') });
                    });

                    if (methods.length > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());

                        B4.Ajax.request(B4.Url.action('AddRoleMetods', 'GisRoleMethod', {
                            methods: Ext.encode(methods),
                            roleId: asp.controller.roleId
                        })).next(function () {
                            asp.getGrid().getStore().load();
                            asp.controller.unmask();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка', 'Необходимо выбрать методы');
                        return false;
                    }

                    return true;
                }
            },

            onBeforeLoad: function (_, options) {
                options.params.showAll = true;
            }
        }
    ],

    init: function () {
        var me = this;

        me.getStore('gisrole.GisRoleMethod').on('beforeload', this.onBeforeLoad, this);

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('gisrolegrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    },

    setCurrentRoleId: function (id) {
        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0],
            roleMethodGrid = editWindow.down('gisrolemethodgrid');
        this.roleId = id;

        if (id > 0) {
            roleMethodGrid.setDisabled(false);
            roleMethodGrid.getStore().filter('roleId', id);
        } else {
            roleMethodGrid.setDisabled(true);
        }
    },

    onBeforeLoad: function(store, operation) {
        operation.params.roleId = this.roleId;
    }
});