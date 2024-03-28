Ext.define('B4.controller.Billing', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.view.params.Panel',
        'B4.Ajax',
        'B4.Url',
        'B4.view.BillingPanel'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },
    
    refs: [
        { ref: 'mainPanel', selector: 'billingpanel' }
    ],
    
    index: function () {
        var me = this;

        B4.Ajax.request({ url: B4.Url.action('GetUrl', 'Billing') })
            .next(function (response) {
                var url = response.responseText;
                if (!Ext.isEmpty(url)) {

                    var view = me.getMainPanel() || Ext.widget('billingpanel');

                    Ext.ComponentQuery.query('[name="overhaulBillingFrame"]')[0].autoEl.src = url.replace(/\"/g, '');
                    
                    me.bindContext(view);
                    me.application.deployView(view);                   
                }
            });
    }
});