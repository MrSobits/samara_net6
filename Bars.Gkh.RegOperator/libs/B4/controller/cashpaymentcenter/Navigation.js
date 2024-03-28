Ext.define('B4.controller.cashpaymentcenter.Navigation', {
    extend: 'B4.base.Controller',
    views: ['cashpaymentcenter.NavigationPanel'],

    params: {},
    title: 'Расчетно-кассовый центр',

    stores: ['cashpaymentcenter.NavigationMenu'],
    requires: ['B4.aspects.TreeNavigationMenu'],
    
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'cashpaymentcenternavpanel' },
        { ref: 'menuTree', selector: 'cashpaymentcenternavpanel menutreepanel' },
        { ref: 'infoLabel', selector: 'cashpaymentcenternavpanel breadcrumbs' },
        { ref: 'mainTab', selector: 'cashpaymentcenternavpanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeNavigationAspect',
            mainPanel: 'cashpaymentcenternavpanel',
            menuContainer: 'cashpaymentcenternavpanel menutreepanel',
            tabContainer: 'cashpaymentcenternavpanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'cashpaymentcenternavpanel breadcrumbs',
            storeName: 'cashpaymentcenter.NavigationMenu',
            deployKey: 'cashpayment_center',
            contextKey: 'cashPaymentCenterId',
            prepareTitle: function (comp) {
                B4.Ajax.request({
                    url: B4.Url.action('Get', 'CashPaymentCenter'),
                    method: 'POST',
                    params: { id: this.getObjectId() }
                }).next(function (response) {
                    var data = Ext.decode(response.responseText);
                    if (data.data && data.data.Name) {
                        comp.update({ text: data.data.Name });
                    }
                });
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('cashpaymentcenternavpanel');

        me.bindContext(view);
        me.setContextValue(view, 'cashPaymentCenterId', id);
        me.application.deployView(view);
    }
});