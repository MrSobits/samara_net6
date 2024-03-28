﻿Ext.define('B4.controller.dict.PaymentSizeCr', {
    /*
    * Контроллер справочника размера платежей по КР
    */
    extend: 'B4.base.Controller',

    requires: [
        'B4.view.dict.paymentsizecr.Grid',
        'B4.view.dict.paymentsizecr.EditWindow',

        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'dict.PaymentSizeCr',
        'dict.PaymentSizeMuRecord'
    ],
    
    stores: [
        'dict.PaymentSizeCr',
        'dict.PaymentSizeMuRecord'
    ],
    
    views: [
        'dict.paymentsizecr.Grid',
        'dict.paymentsizecr.EditWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'paymentsizecrpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Ovrhl.Dictionaries.PaymentSizeCr.Create', applyTo: 'b4addbutton', selector: 'paymentsizecrpanel' },
                { name: 'Ovrhl.Dictionaries.PaymentSizeCr.Edit', applyTo: 'b4savebutton', selector: 'paymentsizecrpanel' },
                { name: 'Ovrhl.Dictionaries.PaymentSizeCr.Delete', applyTo: 'b4deletecolumn', selector: 'paymentsizecrpanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Ovrhl.Dictionaries.PaymentSizeCr.Fields.TypeIndicator_Edit', applyTo: '#cbTypeIndicator', selector: '#paymentsSizeEditWindow' },
                { name: 'Ovrhl.Dictionaries.PaymentSizeCr.Fields.TypeIndicator_View', applyTo: '#cbTypeIndicator', selector: '#paymentsSizeEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Ovrhl.Dictionaries.PaymentSizeCr.Fields.PaymentSize_Edit', applyTo: '#tfPaymentSize', selector: '#paymentsSizeEditWindow' },
                { name: 'Ovrhl.Dictionaries.PaymentSizeCr.Fields.PaymentSize_View', applyTo: '#tfPaymentSize', selector: '#paymentsSizeEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Ovrhl.Dictionaries.PaymentSizeCr.Fields.DateStartPeriod_Edit', applyTo: '#dfDateStartPeriod', selector: '#paymentsSizeEditWindow' },
                { name: 'Ovrhl.Dictionaries.PaymentSizeCr.Fields.DateStartPeriod_View', applyTo: '#dfDateStartPeriod', selector: '#paymentsSizeEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Ovrhl.Dictionaries.PaymentSizeCr.Fields.DateEndPeriod_Edit', applyTo: '#dfDateEndPeriod', selector: '#paymentsSizeEditWindow' },
                { name: 'Ovrhl.Dictionaries.PaymentSizeCr.Fields.DateEndPeriod_View', applyTo: '#dfDateEndPeriod', selector: '#paymentsSizeEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Ovrhl.Dictionaries.PaymentSizeCr.Registry.Municipality.Create', applyTo: 'b4addbutton', selector: '#paymentsSizeEditWindow' },
                { name: 'Ovrhl.Dictionaries.PaymentSizeCr.Registry.Municipality.Delete', applyTo: 'b4deletecolumn', selector: '#paymentsSizeEditWindow' },
                { name: 'Ovrhl.Dictionaries.PaymentSizeCr.Registry.Municipality.View', applyTo: 'mupaysizecrpanel', selector: '#paymentsSizeEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'paymentSizeCrGridWindowAspect',
            gridSelector: 'paymentsizecrpanel',
            editFormSelector: '#paymentsSizeEditWindow',
            storeName: 'dict.PaymentSizeCr',
            modelName: 'dict.PaymentSizeCr',
            editWindowView: 'dict.paymentsizecr.EditWindow',
            onSaveSuccess: function(asp, record) {
                asp.controller.setPaymentSizeCrId(record.getId());
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    var editForm = asp.getForm(),
                        fsPeriod = editForm.down('#fsPeriod'),
                        fsPeriodVisible = false;

                    fsPeriod.items.items.forEach(function (item) {
                        if (!item.hidden)
                        {
                            // Отобразить fieldset только в том случае, когда
                            // хотя бы один из его компонентов доступен для просмотра
                            fsPeriodVisible = true;
                        }
                    });

                    fsPeriodVisible ? fsPeriod.show() : fsPeriod.hide();

                    asp.controller.setPaymentSizeCrId(record.getId());
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'paymentSizeMunicipalityAspect',
            gridSelector: 'mupaysizecrpanel',
            storeName: 'dict.PaymentSizeMuRecord',
            modelName: 'dict.PaymentSizeMuRecord',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#paymentSizeMunicipalitMultiSelectWindow',
            storeSelect: 'dict.MunicipalityByOperator',
            storeSelected: 'dict.MunicipalityByOperator',
            titleSelectWindow: 'Выбор муниципальных образований',
            titleGridSelect: 'Муниципальные образования',
            titleGridSelected: 'Выбранные муниципальные образования',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddMuRecords', 'PaymentSizeMuRecord', {
                            municipalityIds: Ext.encode(recordIds),
                            paymentSizeCrId: asp.controller.paymentSizeCrId
                        })).next(function () {
                            asp.controller.getStore(asp.storeName).load();
                            asp.controller.unmask();
                            Ext.Msg.alert('Сохранено!', 'Муниципальные образования сохранены успешно');

                            asp.controller.getStore('dict.PaymentSizeCr').load();
                            return true;
                        }).error(function (response) {
                            Ext.Msg.alert('Ошибка', response.message);
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать муниципальные образования');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function () {
        this.getStore('dict.PaymentSizeMuRecord').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('paymentsizecrpanel');

        this.bindContext(view);
        this.application.deployView(view);

        this.getStore('dict.PaymentSizeCr').load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.paymentSizeCrId = this.paymentSizeCrId;
    },

    setPaymentSizeCrId: function (id) {
        var editWindow = Ext.ComponentQuery.query('#paymentsSizeEditWindow')[0],
            grid = editWindow.down('mupaysizecrpanel'),
            store = grid.getStore();

        this.paymentSizeCrId = id;
        store.removeAll();
        if (id) {
            store.load();
        }

        grid.setDisabled(!id);
    }
});