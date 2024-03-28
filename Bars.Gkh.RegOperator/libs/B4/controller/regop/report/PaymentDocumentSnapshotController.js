Ext.define('B4.controller.regop.report.PaymentDocumentSnapshotController', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.Permission',
        'B4.aspects.GridEditCtxWindow'
    ],

    stores: [],
    views: [
        'regop.report.PaymentDocumentSnapshotGrid',
        'regop.report.PaymentDocumentSnapshotInfoWindow'
    ],
    models: [],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'paydocdatasnapshotgrid' },
        { ref: 'infoWindow', selector: 'paymentdocinfowindow' },
        { ref: 'period', selector: 'paydocdatasnapshotgrid b4selectfield[name=ChargePeriod]' }
    ],

    init: function() {
        var me = this;

        me.control({
            'paydocdatasnapshotgrid': {
                'beforeload': me.beforeSnapshotsLoad,
                'rowaction': {
                    fn: me.onRowAction,
                    scope: me
                },
                'gridaction': {
                    fn: me.onGridAction,
                    scope: me
                }
            },
            'paymentdocinfowindow': { 'beforeload': me.beforeAccountInfoLoad },
            'paymentdocinfowindow [action=download]': {
                'click': {
                    fn: me.onClickDownload,
                    scope: me
                }
            },
            'paydocdatasnapshotgrid b4selectfield[name=ChargePeriod]': {
                'change': {
                    fn: me.onChangeChargePeriod,
                    scope: me
                }
            },
            'paydocdatasnapshotgrid [action=multidownload]': {
                'click': {
                    fn: me.onClickMultiDownload,
                    scope: me
                }
            },
            'paydocdatasnapshotgrid [action=SendEmail]': {
                'click': {
                    fn: me.onSendEmailClick,
                    scope: me
                }
            },
            'paydocdatasnapshotgrid [action=SetEmail]': {
                'click': {
                    fn: me.onSetEmailClick,
                    scope: me
                }
            },
        });

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('paydocdatasnapshotgrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.down('b4selectfield[name = ChargePeriod]').getStore().load();
    },

    fieldDataMap: {
        DocNum: ['ИННСобственника', 'НомерДокумента' ],
        DocDate: ['ДатаОкончанияПериода'],
        PeriodName: ['НаименованиеПериода'],
        PayerName: ['Плательщик', 'ФИОСобственника'],
        AccountNumber: ['ЛицевойСчет'],
        OwnerInn: ['ИННСобственника', 'ИННПлательщика'],
        RoomAddress: ['АдресКвартиры'],
        ReceiverName: ['НаименованиеПолучателя'],
        ReceiverInn: ['ИннПолучателя'],
        ReceiverAddress: ['Адрес', 'АдресПолучателя'],
        ReceiverKpp: ['КппПолучателя'],
        ReceiverAccountNumber: ['РсчетПолучателя'],
        ReceiverBankAddress: ['АдресБанка'],
        Charge: ['Итого', 'Начислено'],
        Penalty: ['Пени', 'НачисленоПени'],
        Recalc: ['Перерасчет']
    },

    aspects: [
        {
            xtype: 'grideditctxwindowaspect',
            name: 'paymentDocSnapshotEditWin',
            gridSelector: 'paydocdatasnapshotgrid',
            editFormSelector: 'paymentdocinfowindow',
            editWindowView: 'B4.view.regop.report.PaymentDocumentSnapshotInfoWindow',
            modelName: 'B4.model.regop.report.PaymentDocumentSnapshot',
            listeners: {
                aftersetformdata: function(asp, rec, form) {
                    form.objectId = rec.get('Id');
                    var data = rec.get('Data'),
                        resultObj = {};
                    if (Ext.isString(data)) {
                        data = Ext.JSON.decode(data);

                        Ext.iterate(asp.controller.fieldDataMap, function(prop, names) {
                            for (var i = 0; i < names.length; ++i) {
                                if (data[names[i]]) {
                                    resultObj[prop] = data[names[i]];
                                }
                            }
                        });

                        if (rec.get('HolderType') === 'PersonalAccount') {
                            resultObj['DocDate'] = null;
                        }

                        form.getForm().setValues(resultObj);
                    }

                    asp.controller.getInfoWindow().loadAccounts();
                }
            }
        },
        {
            xtype: 'permissionaspect',
            applyOn: {
                event: 'render',
                selector: 'paydocdatasnapshotgrid'
            },
            permissions: [
                {
                    name: 'GkhRegOp.Accounts.PaymentDocumentSnapshot.Delete',
                    applyTo: 'b4deletebutton',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                            component.enable();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'GkhRegOp.Accounts.PaymentDocumentSnapshot.SendEmail',
                    applyTo: '[action=SendEmail]',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                }
            ]
        }
    ],

    //#region Event handlers
    beforeSnapshotsLoad: function(store, opts) {
        var periodId = this.getPeriod().getValue();
        if (!periodId) {
            Ext.Msg.alert('Ошибка', 'Не выбран период!');
            return false;
        }

        Ext.apply(opts.params, {
            periodId: periodId
        });
    },

    beforeAccountInfoLoad: function(win, store, opts) {
        Ext.apply(opts.params, {
            snapshotId: win.objectId
        });
    },

    onChangeChargePeriod: function() {
        this.getMainView().getStore().load();
    },

    onRowAction: function(grid, action, record) {
        switch (action.toLowerCase()) {
        case 'download':
            this.singleDownload(record);
            break;
        }
    },

    onGridAction: function(grid, action) {
        var me = this,
            period = me.getPeriod().getValue(),
            records = [];
        if (action === 'delete') {
            if (!period) {
                Ext.Msg.alert('Ошибка', 'Не выбран период!');
                return false;
            }

            records = Ext.Array.map(grid.getSelectionModel().getSelection(), function(record) { return record.get('Id') });
            records = Ext.Array.from(records);

            Ext.Msg.confirm('Внимание',
                'Вы действительно хотите удалить записи?'
                + (Ext.isEmpty(records) ? '<br>Будут удалены все записи за период' : ''), function(btn) {
                    if (btn !== 'yes') {
                        return false;
                    }

                    me.mask();
                    B4.Ajax.request({
                        url: B4.Url.action('Delete', 'PaymentDocumentSnapshot'),
                        timeout: 5 * 60 * 1000,
                        params: {
                            records: Ext.JSON.encode(records),
                            periodId: period
                        }
                    }).next(function() {
                        me.unmask();
                        grid.getStore().load();
                    }).error(function() {
                        me.unmask();
                    });
                });
        }
    },

    onClickDownload: function(btn) {
        var frm = btn.up('paymentdocinfowindow').getForm(),
            record;

        frm.updateRecord();
        record = frm.getRecord();

        this.singleDownload(record);
    },

    onClickMultiDownload: function() {
        var me = this,
            grid = me.getMainView(),
            selectedRecords = grid.getSelectionModel().selected;

        me.multiDownload(selectedRecords);
    },

    onSendEmailClick: function() {
        var me = this,
            periodId = me.getPeriod().getValue(),
            grid = me.getMainView(),
            filters = grid.getHeaderFilters(),
            records;

        if (!periodId) {
            Ext.Msg.alert('Ошибка', 'Не выбран период!');
            return false;
        }

        records = Ext.Array.map(grid.getSelectionModel().getSelection(), function(record) {
            return record.get('Id');
        });
        records = Ext.Array.from(records);

        var message = Ext.String.format('Вы действительно хотите отправить квитанции за выбранный период {0} абонентам, у которых указан адрес электронной почты в карточке абонента?',
            Ext.isEmpty(records) ? 'всем' : records.length);

        Ext.Msg.confirm('Внимание', message, function (btn) {
                if (btn !== 'yes') {
                    return false;
                }

                me.mask();
                B4.Ajax.request({
                    url: B4.Url.action('SendEmails', 'PaymentDocumentSnapshot'),
                    timeout: 5 * 60 * 1000,
                    params: {
                        periodId: periodId,
                        records: Ext.JSON.encode(records),
                        complexFilter: Ext.encode(filters)
                    }
                }).next(function (response) {
                    me.unmask();
                    var result = Ext.JSON.decode(response.responseText);
                    Ext.Msg.alert('Информация', result.message);
                }).error(function(e) {
                    me.unmask();
                    Ext.Msg.alert('Внимание', e.message || e);
                });
            });
    },

    onSetEmailClick: function () {
        var me = this,
            periodId = me.getPeriod().getValue(),
            grid = me.getMainView(),
            filters = grid.getHeaderFilters(),
            records;

        if (!periodId) {
            Ext.Msg.alert('Ошибка', 'Не выбран период!');
            return false;
        }

        records = Ext.Array.map(grid.getSelectionModel().getSelection(), function (record) {
            return record.get('Id');
        });
        records = Ext.Array.from(records);

        var message = Ext.String.format('Вы действительно хотите выполнить действие?');

        Ext.Msg.confirm('Внимание', message, function (btn) {
            if (btn !== 'yes') {
                return false;
            }

            me.mask();
            B4.Ajax.request({
                url: B4.Url.action('SetEmails', 'PaymentDocumentSnapshot'),
                timeout: 5 * 60 * 1000,
                params: {
                    periodId: periodId,
                    records: Ext.JSON.encode(records),
                    complexFilter: Ext.encode(filters)
                }
            }).next(function (response) {
                me.unmask();
                var result = Ext.JSON.decode(response.responseText);
                Ext.Msg.alert('Информация', result.message);
            }).error(function (e) {
                me.unmask();
                Ext.Msg.alert('Внимание', e.message || e);
            });
        });
    },

    //#endregion

    singleDownload: function(record) {
        if (Ext.isEmpty(record))
            return;

        var id = record.get('Id');

        window.open(B4.Url.action('/PaymentDocumentSnapshot/CreateDocumentFromSnapshot?snapshotId=' + id), '_blank');
    },

    multiDownload: function(records) {
        var me = this,
            grid = me.getMainView(),
            filters = { complexFilter: Ext.encode(grid.getHeaderFilters()) },
            period = me.getPeriod().getValue(),
            recordIds = [];

        if (!Ext.isEmpty(records)) {
            Ext.each(records.items, function(r) {
                recordIds.push(r.get('Id'));
            });
        }

        Ext.apply(filters, {
            ids: Ext.JSON.encode(recordIds),
            periodId: period
        });

        me.mask();
        B4.Ajax.request({
            url: B4.Url.action('CreateDocsFromSnapshots', 'PaymentDocumentSnapshot'),
            params: filters
        }).next(function(resp) {
            var tryDecoded;

            me.unmask();

            try {
                tryDecoded = Ext.JSON.decode(resp.responseText);
            } catch (e) {
                tryDecoded = {};
            }

            Ext.Msg.alert('Документы на оплату', tryDecoded.message || "Задача успешно поставлена в очередь на обработку. " +
                "Информация о статусе формирования документов на оплату содержится в пункте меню \"Задачи\"");
        }).error(function(err) {
            me.unmask();
            Ext.Msg.alert('Ошибка', err.message || err);
        });
    }
});