Ext.define('B4.controller.import.MskDpkrImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.MskDpkrPanel'],

    mainView: 'import.MskDpkrPanel',
    mainViewSelector: 'mskdpkrimportpanel',

    aspects: [
        {
            xtype: 'gkhimportaspect',
            viewSelector: 'mskdpkrimportpanel',
            importId: 'Bars.Gkh.Regions.Msk.Import.CommonRealtyObjectImport.MskDpkrImport',
            maxFileSize: 52428800,
            getUserParams: function () {
                var me = this;
                me.params = me.params || {};

                me.params['isLiftInfo'] = me.controller.getMainView().down('checkbox[name=IsLiftInfo]').getValue();
            }
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});