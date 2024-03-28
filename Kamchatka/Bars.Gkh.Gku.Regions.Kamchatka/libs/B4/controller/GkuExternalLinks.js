Ext.define('B4.controller.GkuExternalLinks', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.view.FramePanel'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [{
        ref: 'mainView',
        selector: 'framepanel'
    }],    
   
    billing: function () {
        var me = this,
            view;

        B4.Ajax.request(
            {
                url: B4.Url.action('GetUrl', 'GkuExternalLinks'),
                params: {
                    code: 'Overhaul_BillingUrl'
                }
            })
            .next(function(response) {
                var url = response.responseText;
                if (Ext.isEmpty(url)) {
                    Ext.Msg.alert('Ошибка!', 'Не задан адрес "Модуль начисления"');
                    return;
                }

                view = me.getMainView() || Ext.widget('framepanel', {
                    title: 'Модуль начисления',
                    src: url.replace(/\"/g, '')
                });
                me.bindContext(view);
                me.application.deployView(view);
            });
    }
});