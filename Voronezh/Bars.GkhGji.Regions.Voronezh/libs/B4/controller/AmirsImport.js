Ext.define('B4.controller.AmirsImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.AmirsImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['AmirsImport'],

    mainView: 'AmirsImport',
    mainViewSelector: 'AmirsImport',

    aspects: [
        {
            xtype: 'amirsimportaspect',
            viewSelector: 'AmirsImport',
            importId: 'Bars.GkhGji.Regions.Voronezh.Imports.AmirsImport',
            initComponent: function () {
                var me = this;
                Ext.apply(me,
                    {
                        maxFileSize: Gkh.config.General.MaxUploadFileSize * 1048576
                    });

                me.callParent(arguments);
            }
        }
    ],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    },
  
});


