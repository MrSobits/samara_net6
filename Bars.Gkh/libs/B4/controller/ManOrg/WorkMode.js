Ext.define('B4.controller.manorg.WorkMode', {
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.aspects.InlineGrid',
        'B4.Url',
        'B4.Ajax',
        'B4.aspects.permission.manorg.WorkMode',
        'B4.aspects.GkhEditPanel'
    ],

    models: [
        'manorg.WorkMode',
        'ManagingOrganization'
    ],

    stores:
    [
        'manorg.WorkMode',
        'manorg.ReceptionCitizens',
        'manorg.DispatcherWork'
    ],

    views: [
        'manorg.WorkModePanel',
        'manorg.DispatchPanel'
    ],

    parentCtrlCls: 'B4.controller.manorg.Navigation',
    
    aspects: [
        {
            xtype: 'manorgworkmodeperm'
        },
        {
            xtype: 'inlinegridaspect',
            name: 'manorgWorkModeGridAspect',
            storeName: 'manorg.WorkMode',
            modelName: 'manorg.WorkMode',
            gridSelector: '#workModeGrid',

            save: function () {
                var asp = this;

                var store = this.controller.getStore(this.storeName);
                var modifRecords = store.getModifiedRecords();

                var records = [];
                Ext.Array.each(modifRecords, function (rec) { records.push(rec.data); });
                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('AddWorkMode', 'ManagingOrgWorkMode', {
                    records: Ext.JSON.encode(records),
                    manorgId: asp.controller.params.id
                })).next(function () {
                    asp.updateGrid();
                    B4.QuickMsg.msg('Сохранение!', 'Режимы сохранены успешно', 'success');
                    asp.controller.unmask();
                    return true;
                }).error(function () {
                    asp.controller.unmask();
                });
            }
        },
        {
            xtype: 'inlinegridaspect',
            name: 'manorgReceptionCitizensGridAspect',
            storeName: 'manorg.ReceptionCitizens',
            modelName: 'manorg.WorkMode',
            gridSelector: '#receptionCitizensGrid',

            save: function () {
                var asp = this;

                var store = this.controller.getStore(this.storeName);
                var modifRecords = store.getModifiedRecords();

                var records = [];
                Ext.Array.each(modifRecords, function (rec) { records.push(rec.data); });
                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('AddWorkMode', 'ManagingOrgWorkMode', {
                    records: Ext.JSON.encode(records),
                    manorgId: asp.controller.params.id
                })).next(function () {
                    asp.updateGrid();
                    B4.QuickMsg.msg('Сохранение!', 'Режимы сохранены успешно', 'success');
                    asp.controller.unmask();
                    return true;
                }).error(function () {
                    asp.controller.unmask();
                });
            }
        },
        {
            xtype: 'inlinegridaspect',
            name: 'manorgDispatcherWorkGridAspect',
            storeName: 'manorg.DispatcherWork',
            modelName: 'manorg.WorkMode',
            gridSelector: '#dispatcherWorkGrid',

            save: function () {
                var asp = this;

                var store = this.controller.getStore(this.storeName);
                var modifRecords = store.getModifiedRecords();

                var records = [];
                Ext.Array.each(modifRecords, function (rec) { records.push(rec.data); });
                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('AddWorkMode', 'ManagingOrgWorkMode', {
                    records: Ext.JSON.encode(records),
                    manorgId: asp.controller.params.id
                })).next(function () {
                    asp.updateGrid();
                    //  сообщение выходит тольк опосле сохранения полей адреса 
                    //B4.QuickMsg.msg('Сохранение!', 'Режимы сохранены успешно', 'success');
                    
                    asp.controller.getAspect('manorgDispatchAspect').saveRequestHandler();

                    //asp.controller.unmask();
                    return true;
                }).error(function () {
                    asp.controller.unmask();
                });
            }
        },
        {
            xtype: 'gkheditpanel',
            name: 'manorgDispatchAspect',
            editPanelSelector: 'manorgDispatchPanel[name=edit]',
            modelName: 'ManagingOrganization',
            listeners: {
                //нужен для того, чтобы информация о типе управления была всегда актуальной для управления домами
                savesuccess: function (asp, record) {
                    B4.QuickMsg.msg('Сохранение!', 'Режимы работы сохранены успешно', 'success');
                    asp.controller.unmask();
                },
                savefailure: function (record, message) {
                    B4.QuickMsg.msg('Ошибка!', message, 'error');
                    asp.controller.unmask();
                },
                aftersetpaneldata: function (asp, record) {
                    if (record.get('IsDispatchCrrespondedFact') === true) {
                        asp.getPanel().down('b4fiasselectaddress').disable();
                    } else {
                        asp.getPanel().down('b4fiasselectaddress').enable();
                    }

                }
            },
            onPreSaveSuccess: function (asp, record) {
                asp.fireEvent('savesuccess', asp, record);
            }
        }
    ],

    params: {},
    mainView: 'manorg.WorkModePanel',
    mainViewSelector: 'manorgworkmodepanel',
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    init: function () {
        this.getStore('manorg.WorkMode').on('beforeload', this.onBeforeLoad, this, 10);
        this.getStore('manorg.ReceptionCitizens').on('beforeload', this.onBeforeLoad, this, 20);
        this.getStore('manorg.DispatcherWork').on('beforeload', this.onBeforeLoad, this, 30);

        this.control({
            'checkbox[name=IsDispatchCrrespondedFact]': { 'change': this.onChangeCheckBox }
        });

        this.callParent(arguments);
    },

    onChangeCheckBox: function (field, newValue) {
        var form = field.up().up(), dispatchAddress = form.down('b4fiasselectaddress[name=DispatchAddress]');
        dispatchAddress.setDisabled(newValue);
    },

    onBeforeLoad: function (store, operation, type) {
        if (this.params) {
            operation.params.manorgId = this.params.id;
            operation.params.typeMode = type;
        }
    },

    index: function (id) {
        var me = this;
        var view = me.getMainView() || Ext.widget('manorgworkmodepanel');
        me.params.id = id;

        me.bindContext(view);
        me.setContextValue(view, 'manorgId', id);
        me.application.deployView(view, 'manorgId_info');

        me.getStore('manorg.WorkMode').load();
        me.getStore('manorg.ReceptionCitizens').load();
        me.getStore('manorg.DispatcherWork').load();
        me.getAspect('manorgDispatchAspect').setData(me.params.id);
    }
});