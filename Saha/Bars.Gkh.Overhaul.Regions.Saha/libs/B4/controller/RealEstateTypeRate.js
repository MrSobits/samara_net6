Ext.define('B4.controller.RealEstateTypeRate', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.InlineGrid',
        'B4.enums.RateCalcTypeArea'
    ],

    models: ['RealEstateTypeRate'],
    stores: ['RealEstateTypeRate'],
    views: [
        'RealEstateTypeRateGrid',
        'realestatetype.RealEstateTypeRateNotLivingAreaGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'realestatetyperategrid'
        },
        {
            ref: 'mainNotLivingAreaView',
            selector: 'realestatetyperatenotlivingareagrid'
        }
    ],

    notLivingArea: false,

    aspects: [
        {
            xtype: 'inlinegridaspect',
            name: 'realEstateTypeRateAspect',
            storeName: 'RealEstateTypeRate',
            modelName: 'RealEstateTypeRate',
            gridSelector: 'realestatetyperategrid'
        },
        {
            xtype: 'inlinegridaspect',
            name: 'realEstateTypeRateNotLivingArea',
            storeName: 'RealEstateTypeRate',
            modelName: 'RealEstateTypeRate',
            gridSelector: 'realestatetyperatenotlivingareagrid'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Ovrhl.RealEstateTypeRate.Edit', applyTo: 'b4savebutton', selector: 'realestatetyperategrid' },
                { name: 'Ovrhl.RealEstateTypeRate.Edit', applyTo: '#recalcButton', selector: 'realestatetyperategrid' },
                { name: 'Ovrhl.RealEstateTypeRate.Edit', applyTo: 'b4savebutton', selector: 'realestatetyperatenotlivingareagrid' },
                { name: 'Ovrhl.RealEstateTypeRate.Edit', applyTo: '#recalcButton', selector: 'realestatetyperatenotlivingareagrid' }
            ]
        }
    ],

    init: function () {
        var me = this,
            actions = {
                'realestatetyperategrid #recalcButton': {
                    click: me.recalculateRates,
                    scope: me
                },
                'realestatetyperatenotlivingareagrid #recalcButton': {
                    click: me.recalculateRates,
                    scope: me
                }
            };
        me.control(actions);
        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view;

        // Запрашиваем с сервера какой расчет тарифов и собираемости средств указан в параметрах
        B4.Ajax.request({
            url: B4.Url.action('GetParamByKey', 'GkhParam'),
            params: {
                key: 'RateCalcTypeArea'
            },
            timeout: 9999999
        }).next(function (resp) {
            // Если в настройках ДПКР параметр "Расчет тарифов и собираемости средств по" = "Жилая площадь"
            if (Ext.decode(resp.responseText) == B4.enums.RateCalcTypeArea.AreaLiving) {
                me.notLivingArea = true;
                view = me.getMainNotLivingAreaView() || Ext.widget('realestatetyperatenotlivingareagrid');
            } else {
                view = me.getMainView() || Ext.widget('realestatetyperategrid');
            }

            me.bindContext(view);
            me.application.deployView(view);

            view.getStore().load();
        }).error(function (e) {
            Ext.Msg.alert('Ошибка!', (e.message || e));
        });
    },

    recalculateRates: function () {
        var me = this,
            view;

        if (me.notLivingArea) {
            view = me.getMainNotLivingAreaView();
        } else {
            view = me.getMainView();
        }
        var store = view.getStore();

        var modifiedRowsCount = store.getNewRecords().length +
            store.getRemovedRecords().length +
            store.getUpdatedRecords().length;

        if (modifiedRowsCount > 0) {
            B4.QuickMsg.msg("Предупреждение", "Сначала необходимо сохранить измененые записи", "warning");
            return;
        }

        me.mask('Расчет показателей', view);

        B4.Ajax.request({
            url: B4.Url.action('CalculateRates', 'RealEstateTypeRate'),
            timeout: 9999999 // 5 минут
        }).next(function () {
            me.unmask();
            B4.QuickMsg.msg('Успешно', 'Расчет показателей завершен', 'success');
            store.load();
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'При расчете показателей произошла ошибка!');
        });
    }
});