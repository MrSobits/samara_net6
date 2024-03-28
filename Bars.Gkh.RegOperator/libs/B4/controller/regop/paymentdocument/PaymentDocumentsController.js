Ext.define('B4.controller.regop.paymentdocument.PaymentDocumentsController', {
    extend: 'B4.base.Controller',
    views: ['regop.paymentdocument.PaymentDocumentsPanel'],
    stores: ['regop.paymentdocument.PaymentDocumentsStore'],

    requires:[
        'B4.mixins.Context',
        'B4.mixins.MaskBody'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'paymentdocumentspanel' }
    ],

    init: function() {
        var me = this;

        me.control({
            'paymentdocumentspanel button[action="getLink"]': {
                'click': {
                    fn: me.getLink,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    index: function() {
        var view = this.getMainView() || Ext.widget('paymentdocumentspanel');
        this.bindContext(view);
        this.application.deployView(view);

        view.down('b4selectfield[name = ChargePeriod]').getStore().load();
    },

    getLink: function () {
        var me = this,
            mainView = me.getMainView(),
            linkField = me.getMainView().down('[name="Link"]'),
            periodId = 0;
        if (mainView) {
            periodId = mainView.down('b4selectfield[name=ChargePeriod]').getValue();
        }

        me.mask();
        B4.Ajax.request({
            url: B4.Url.action('GetLink', 'PeriodPaymentDocuments'),
            params: {
                periodId: periodId
            }
        }).next(function(resp) {
            me.unmask();

            var json = Ext.JSON.decode(resp.responseText);
            linkField.setValue(json.data);
        }).error(function (resp) {
            var json = Ext.JSON.decode(resp.responseText);
            var message = (json && json.message) ? json.message : resp.message;
            
            Ext.Msg.alert('Ошибка', message);
            me.unmask();
        });
    },

    onStoreBeforeLoad: function (store, operation) {
        var mainView = this.getMainView();
        if (mainView) {
            operation.params.periodId = mainView.down('b4selectfield[name=ChargePeriod]').getValue();
        }
    }
});