Ext.define('B4.controller.regop.paymentdocument.SberbankPaymentDoc', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.BackForward',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.StateContextMenu',
        'B4.aspects.ButtonDataExport',
        'B4.view.regop.paymentdocument.SberbankPaymentDocGrid',
        'B4.Ajax',
        'B4.Url'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores: [
        'regop.paymentdocument.SberbankPaymentDoc'
    ],

    models: [
        'regop.paymentdocument.SberbankPaymentDoc'
    ],

    views: [
        'regop.paymentdocument.SberbankPaymentDocGrid',
    ],

    mainView: 'regop.paymentdocument.SberbankPaymentDocGrid',
    mainViewSelector: 'sberbankpaymentdocgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'sberbankpaymentdocgrid'
        }
    ],

    aspects: [

    ],

    index: function () {
        var me = this;
        var view = this.getMainView() || Ext.widget('sberbankpaymentdocgrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore('SberbankPaymentDoc').load();
    },


    init: function () {
        var me = this;
        var view = this.getMainView() || Ext.widget('sberbankpaymentdocgrid');
        me.params = {};
        view.getStore('SberbankPaymentDoc').load();
        me.control({

            'sberbankpaymentdocgrid #createReestrButton': { click: { fn: me.createReestr, scope: this } },

        });
        me.callParent(arguments);
    },

    createReestr: function (btn) {
        var me = this;
        var view = me.getMainView() || Ext.widget('sberbankpaymentdocgrid');
        me.mask('Формирование');
        B4.Ajax.request({
            url: B4.Url.action('CreateReestr', 'SberbankPaymentDoc'),
            timeout: 60000
        }).next(function (response) {
            me.unmask();
            var resp = Ext.decode(response.responseText);
            Ext.Msg.alert('Успешно!', resp.data);
            view.getStore('SberbankPaymentDoc').load();
        }).error(function (response) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', response.message);
        });
    }
});