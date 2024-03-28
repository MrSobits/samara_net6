Ext.define('B4.controller.regop.paymentdocument.PaymentDocumentLogsController', {
    extend: 'B4.base.Controller',
    views: ['regop.paymentdocument.PaymentDocumentLogsPanel'],
    stores: ['regop.paymentdocument.PaymentDocumentLogsStore'],

    requires:[
        'B4.mixins.Context',
        'B4.mixins.MaskBody'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'paymentdocumentlogspanel' }
    ],

    init: function() {
        var me = this;

        me.callParent(arguments);
    },

    index: function() {
        var view = this.getMainView() || Ext.widget('paymentdocumentlogspanel');
        this.bindContext(view);
        this.application.deployView(view);
    }
});