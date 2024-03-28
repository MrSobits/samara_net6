Ext.define('B4.controller.RosRegExtract', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.RosRegExtractAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['RosRegExtract'],

    mainView: 'RosRegExtract',
    mainViewSelector: 'RosRegExtract',

    aspects: [
        {
            xtype: 'rosregextractaspect',
            viewSelector: 'RosRegExtract',
            importId: 'Bars.Gkh.Regions.Chelyabinsk.Imports.RosRegExtractImport',
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
