Ext.define('B4.controller.import.PersonalAccountDigitalReceiptImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: {
         context: 'B4.mixins.Context'
    },

    views: [
        'Import.PersonalAccountDigitalReceiptImportPanel'
    ],

    mainView: 'Import.PersonalAccountDigitalReceiptImportPanel',
    mainViewSelector: 'personalaccountdigitalreceiptimportpanel',

    aspects: [
        {
            xtype: 'gkhimportaspect',
            viewSelector: 'personalaccountdigitalreceiptimportpanel',
            importId: 'Bars.Gkh.RegOperator.Imports.PersonalAccountDigitalReceiptImport',
            initComponent: function() {
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
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});