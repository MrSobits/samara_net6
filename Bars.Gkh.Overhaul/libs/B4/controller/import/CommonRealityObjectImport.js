Ext.define('B4.controller.import.CommonRealityObjectImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.realityobj.CommonRoImportPanel'],

    mainView: 'import.realityobj.CommonRoImportPanel',
    mainViewSelector: 'commonroimportpanel',
    
    aspects: [
    {
        xtype: 'gkhimportaspect',
        viewSelector: 'commonroimportpanel',
        importId: 'Bars.Gkh.Overhaul.Import.CommonRealtyObjectImport.RoImport',
        maxFileSize: 52428800,
        getUserParams: function () {
            var me = this;

            me.params.createObjectsOnImport = me.controller.getMainComponent().down('#chbCreateObjects').getValue();
            me.params.allowEmptyStreets = me.controller.getMainComponent().down('#chbAllowEmptyStreets').getValue();
            me.params.overridExistRecsOnImport = me.controller.getMainComponent().down('#chbOverExRecs').getValue();
        }
    }],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});