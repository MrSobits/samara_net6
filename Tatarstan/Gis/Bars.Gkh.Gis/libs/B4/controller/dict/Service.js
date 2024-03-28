Ext.define('B4.controller.dict.Service', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.enums.TypeServiceGis',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'dict.Service',
        'dict.service.BilServiceDictionary',
        'dict.service.BilTarifStorage',
        'dict.service.BilNormativStorage'
    ],

    stores: [
        'dict.Service',
        'dict.service.BilServiceDictionary',
        'dict.service.BilTarifStorage',
        'dict.service.BilNormativStorage'
    ],

    views: [
        'dict.service.Grid',
        'dict.service.EditWindow',
        'dict.service.BilServiceGrid',
        'dict.service.BilTarifStorageGrid',
        'dict.service.BilNormativStorageGrid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'dict.service.Grid',
    mainViewSelector: 'servicedictgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'servicedictgrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'serviceDictGridEditWindowAspect',
            gridSelector: 'servicedictgrid',
            editFormSelector: 'servicedicteditwindow',
            modelName: 'dict.Service',
            editWindowView: 'dict.service.EditWindow',
            listeners: {
                aftersetformdata: function (asp, record) {
                    var me = this,
                        editWin = me.getForm();

                    editWin.down('bilservicedictgrid').getStore().filter('serviceId', record.getId());
                    editWin.down('biltarifstoragedictgrid').getStore().filter('serviceId', record.getId());
                    editWin.down('bilnormativstoragedictgrid').getStore().filter('serviceId', record.getId());
                    me.controller.setContextValue(editWin, 'serviceId', record.getId());
                }
            },
            deleteRecord: function (record) {
                var me = this;

                Ext.Msg.confirm('Удаление записи!',
                    'У данной записи могут быть связанные нормативы или тарифы. При удалении они тоже будут удалены. Продолжить?',
                    function (result) {
                        if (result === 'yes') {
                            var model = this.getModel(record);

                        var rec = new model({ Id: record.getId() });
                        me.mask('Удаление', B4.getBody());
                        rec.destroy()
                            .next(function () {
                                this.fireEvent('deletesuccess', this);
                                me.updateGrid();
                                me.unmask();
                            }, this)
                            .error(function (result) {
                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                me.unmask();
                            }, this);
                    }
                }, me);
            },
            onSaveSuccess: function (aspect) {
                var form = aspect.getForm();
                form.down('#gisServiceDictTabPanel').setDisabled(false);
            },
            onAfterSetFormData: function (aspect, rec, form) {
                if (!rec.get('Id')) {
                    form.down('#gisServiceDictTabPanel').setDisabled(true);
                }
                if (form) {
                    form.show();
                }
            },
            otherActions: function (actions) {
                actions['servicedicteditwindow bilnormativstoragedictgrid b4monthpicker'] = {
                    'change': { fn: this.onNormativMonthChange, scope: this }
                };
                actions['servicedicteditwindow bilnormativstoragedictgrid b4updatebutton'] = {
                    'click': { fn: this.onNormativGridUpdate, scope: this }
                };
                actions['servicedicteditwindow biltarifstoragedictgrid b4monthpicker'] = {
                    'change': { fn: this.onTarifMonthChange, scope: this }
                };
                actions['servicedicteditwindow biltarifstoragedictgrid b4updatebutton'] = {
                    'click': { fn: this.onTarifGridUpdate, scope: this }
                };
            },
            onNormativMonthChange: function(monthPicker, newVal) {
                var normativGrid = this.getForm().down('bilnormativstoragedictgrid');
                normativGrid.getStore().filter('monthDate', newVal);
            },
            onNormativGridUpdate: function(btn) {
                btn.up('bilnormativstoragedictgrid').getStore().load();
            },
            onTarifMonthChange: function(monthPicker, newVal) {
                var tarifGrid = this.getForm().down('biltarifstoragedictgrid');
                tarifGrid.getStore().filter('monthDate', newVal);
            },
            onTarifGridUpdate: function(btn) {
                btn.up('biltarifstoragedictgrid').getStore().load();
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'serviceAddBilServiceGridAspect',
            gridSelector: 'bilservicedictgrid',
            storeName: 'dict.service.BilServiceDictionary',
            modelName: 'dict.service.BilServiceDictionary',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#serviceAddBilServiceMultiSelectWindow',
            storeSelect: 'dict.service.BilServiceDictionarySelect',
            storeSelected: 'dict.service.BilServiceDictionary',
            titleSelectWindow: 'Выбор услуг для связи',
            titleGridSelect: 'Услуги для связи',
            titleGridSelected: 'Связанные услуги',
            columnsGridSelect: [
                {
                    header: 'Организация', xtype: 'gridcolumn', dataIndex: 'Organization', flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    header: 'Услуга', xtype: 'gridcolumn', dataIndex: 'ServiceName', flex: 1, filter: { xtype: 'textfield' }
                },
                {
                    header: 'Код', xtype: 'gridcolumn', dataIndex: 'ServiceCode', flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        minValue: 1,
                        allowDecimals: false,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RelatedService',
                    flex: 1,
                    text: 'Связанная услуга',
                    filter: { xtype: 'textfield' }
                }
            ],
            columnsGridSelected: [
                { header: 'Организация', xtype: 'gridcolumn', dataIndex: 'Organization', flex: 1, sortable: false },
                { header: 'Услуга', xtype: 'gridcolumn', dataIndex: 'ServiceName', flex: 1, sortable: false }
            ],
            otherActions: function (actions) {
                var me = this;
                actions[me.addButtonSelector || (me.gridSelector + ' #addBillServRef')] = { 'click': { fn: me.btnAction, scope: me } };
            },
            onSelectedBeforeLoad: function(store, operation) {
                var me = this,
                    editWin = me.getGrid().up('servicedicteditwindow'),
                    serviceId = me.controller.getContextValue(editWin, 'serviceId');
                operation.params.serviceId = serviceId ? serviceId : 0;
            },
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        editWin = me.getGrid().up('servicedicteditwindow'),
                        store = me.getGrid().getStore();

                    me.controller.mask('Сохранение', me.getGrid());

                    store.removeAll();
                    records.each(function (rec) {
                        rec.set('Service', me.controller.getContextValue(editWin, 'serviceId'));
                        store.add(rec);
                    });

                    store.sync({
                        callback: function () {
                            me.controller.unmask();
                            store.load();
                        },
                        failure: function (result) {
                            me.controller.unmask();
                            if (result && result.exceptions[0] && result.exceptions[0].response) {
                                Ext.Msg.alert('Ошибка!', Ext.JSON.decode(result.exceptions[0].response.responseText).message);
                            }
                        },
                        scope:me
                    });
                    
                    return true;
                },
                panelrendered: function() {
                    this.getSelectedGrid().getStore().load();
                }
            }
        }
    ],

    init: function () {
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('servicedictgrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    }
});