Ext.define('B4.controller.InfoAboutPaymentCommunal', {
    extend: 'B4.base.Controller',
    views:
    [
        'infoaboutpaymentcommunal.Panel',
        'infoaboutpaymentcommunal.Grid',
        'infoaboutpaymentcommunal.InlineGrid',
        'infoaboutpaymentcommunal.EditWindow'
    ],

    requires:
    [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.InlineGrid',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.infoaboutpaymentcommunal.State'
    ],

    models:
    [
        'InfoAboutPaymentCommunal'
    ],
    stores:
    [
        'InfoAboutPaymentCommunal'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },
    
    mainView: 'infoaboutpaymentcommunal.Panel',
    mainViewSelector: '#infoAboutPaymentCommunalGridPanel',

    aspects: [
        {
            xtype: 'infoaboutpaymentcommunalstateperm',
            name: 'infoAboutPaymentCommunalPermissionAspect'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'infoAboutPaymentCommunalGridAspect',
            gridSelector: '#infoAboutPaymentCommunalGrid',
            editFormSelector: '#infoAboutPaymentCommunalEditWindow',
            storeName: 'InfoAboutPaymentCommunal',
            modelName: 'InfoAboutPaymentCommunal',
            editWindowView: 'infoaboutpaymentcommunal.EditWindow',
            saveRecordHasNotUpload: function (modifRecords) {
                var asp = this;
                var records = [];
                Ext.Array.each(modifRecords, function (rec) {
                    records.push({
                        Id: rec.getId(),
                        Accrual: rec.get('Accrual'),
                        Payed: rec.get('Payed'),
                        Debt: rec.get('Debt'),
                        CounterValuePeriodStart: rec.get('CounterValuePeriodStart'),
                        CounterValuePeriodEnd: rec.get('CounterValuePeriodEnd'),
                        TotalConsumption: rec.get('TotalConsumption'),
                        AccrualByProvider: rec.get('AccrualByProvider'),
                        PayedToProvider: rec.get('PayedToProvider'),
                        DebtToProvider: rec.get('DebtToProvider'),
                        ReceivedPenaltySum: rec.get('ReceivedPenaltySum')
                    });
                });

                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('SaveInfoAboutPaymentCommunal', 'InfoAboutPaymentCommunal', {
                    records: Ext.JSON.encode(records),
                    disclosureInfoRealityObjId: asp.controller.params.disclosureInfoRealityObjId
                })).next(function() {
                    asp.updateGrid();
                    B4.QuickMsg.msg('Сохранение', 'Сохранено успешно', 'success');
                    asp.controller.unmask();
                    return true;
                }).error(function () {
                    B4.QuickMsg.msg('Сохранение', 'Не удалось сохранить данные', 'error');
                    asp.controller.unmask();
                    return false;
                });
            }
        },
        {
            xtype: 'inlinegridaspect',
            name: 'infoAboutPaymentCommunalInlineGridAspect',
            gridSelector: '#infoAboutPaymentCommunalInlineGrid',
            storeName: 'InfoAboutPaymentCommunal',
            modelName: 'InfoAboutPaymentCommunal',
            save: function () {
                var asp = this;
                var store = this.controller.getStore(this.storeName);
                var modifRecords = store.getModifiedRecords();
                var records = [];
                Ext.Array.each(modifRecords, function (rec) {
                    records.push({
                        Id: rec.getId(),
                        Accrual: rec.get('Accrual'),
                        Payed: rec.get('Payed'),
                        Debt: rec.get('Debt'),
                        CounterValuePeriodStart: rec.get('CounterValuePeriodStart'),
                        CounterValuePeriodEnd: rec.get('CounterValuePeriodEnd')
                    });
                });

                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('SaveInfoAboutPaymentCommunal', 'InfoAboutPaymentCommunal', {
                    records: Ext.JSON.encode(records),
                    disclosureInfoRealityObjId: asp.controller.params.disclosureInfoRealityObjId
                })).next(function () {
                    asp.updateGrid();
                    B4.QuickMsg.msg('Сохранение', 'Сохранено успешно', 'success');
                    asp.controller.unmask();
                    return true;
                }).error(function () {
                    B4.QuickMsg.msg('Сохранение', 'Не удалось сохранить данные', 'error');
                    asp.controller.unmask();
                    return false;
                });
            }
        }
    ],
    
    init: function () {
        this.getStore('InfoAboutPaymentCommunal').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;

        me.getStore('InfoAboutPaymentCommunal').load();
        if (me.params) {
            me.params.getId = function () { return me.params.disclosureInfoId; };
            me.getAspect('infoAboutPaymentCommunalPermissionAspect').setPermissionsByRecord(me.params);

            if (parseInt(me.params.year.split('-').join('').substring(0, 8)) >= 20150101) {
                me.getMainView().items.items[0].show();
                me.getMainView().items.items[1].hide();
            } else {
                me.getMainView().items.items[0].hide();
                me.getMainView().items.items[1].show();
            }
        }
    },

    onBeforeLoad: function (store, operation) {
        operation.params.disclosureInfoRealityObjId = this.params.disclosureInfoRealityObjId;
    }
});
