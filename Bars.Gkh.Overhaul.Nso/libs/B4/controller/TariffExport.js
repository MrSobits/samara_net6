Ext.define('B4.controller.TariffExport', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.view.TarifExportPanel',
        'B4.Ajax',
        'B4.Url'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },
    
    mainView: 'TarifExportPanel',
    mainViewSelector: '#overhaulTarifExportPane',
    
    onLaunch: function () {
        Ext.DomHelper.append(document.body, {
            tag: 'iframe',
            id: 'downloadIframe',
            frameBorder: 0,
            width: 0,
            height: 0,
            css: 'display:none;visibility:hidden;height:0px;',
            src: B4.Url.action('GetExportGkuTarif', 'ExportGku')
        });
    }
    
});