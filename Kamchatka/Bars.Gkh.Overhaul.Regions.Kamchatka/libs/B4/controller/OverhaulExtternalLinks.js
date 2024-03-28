Ext.define('B4.controller.OverhaulExtternalLinks', {
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
   
    analytics: function() {
        this.loadPanel('Overhaul_AnalyticsUrl', 'Аналитика');
    },

    monitoring: function() {
        this.loadPanel('Overhaul_MonitoringUrl', 'Мониторинг');
    },
    
    ucp: function () {
        this.loadPanel('Overhaul_UcpUrl', 'УЦП');
    },

    loadPanel: function (code, title) {
        var me = this,
            view;

        B4.Ajax.request(
            {
                url: B4.Url.action('GetUrl', 'GkuExternalLinks'),
                params: {
                    code: code
                }
            })
            .next(function(response) {
                var url = response.responseText;

                if (Ext.isEmpty(url)) {
                    Ext.Msg.alert('Ошибка!', 'Не задан адрес "' + title + '"');
                    return;
                }

                view = me.getMainView() || Ext.widget('framepanel', {
                    title: title,
                    src: url.replace(/\"/g, '')
                });
                me.bindContext(view);
                me.application.deployView(view);
            });
    }
});