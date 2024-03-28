Ext.define('B4.controller.ExtractImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.ExtractImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['ExtractImport'],

    mainView: 'ExtractImport',
    mainViewSelector: 'ExtractImport',

    aspects: [
        {
            xtype: 'extractimportaspect',
            viewSelector: 'ExtractImport',
            importId: 'Sobits.RosReg.Imports.ExtractImport',
            initComponent: function () {
                var me = this;
                Ext.apply(me,
                    {
                        maxFileSize: 209715200 //200mb
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
    }
});