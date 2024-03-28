Ext.define('B4.controller.RealEstateTypeRate', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.InlineGrid'
    ],

    models: ['RealEstateTypeRate'],
    stores: ['RealEstateTypeRate'],
    views: [ 'RealEstateTypeRateGrid' ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    
    mainView: 'RealEstateTypeRateGrid',
    mainViewSelector: 'realestatetyperategrid',

    aspects: [
        {
            xtype: 'inlinegridaspect',
            name: 'realEstateTypeRateAspect',
            storeName: 'RealEstateTypeRate',
            modelName: 'RealEstateTypeRate',
            gridSelector: 'realestatetyperategrid'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Ovrhl.RealEstateTypeRate.Edit', applyTo: 'b4savebutton', selector: 'realestatetyperategrid' },
                { name: 'Ovrhl.RealEstateTypeRate.Edit', applyTo: '#recalcButton', selector: 'realestatetyperategrid' }
            ]
        }
    ],

    init: function () {
        var me = this,
            actions = {
                'realestatetyperategrid #recalcButton': {
                    click: me.recalculateRates,
                    scope: me
                }
            };
        me.control(actions);
        me.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('realestatetyperategrid');
        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    },
    
    recalculateRates: function () {
        var me = this,
            view = me.getMainView(),
            store = view.getStore();
        
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
        }).error(function (response) {
            me.unmask();
            var message = 'При расчете показателей произошла ошибка!';
            if (response && response.message) {
                message = response.message;
            }
            Ext.Msg.alert('Ошибка!', message);
        });
    }
});