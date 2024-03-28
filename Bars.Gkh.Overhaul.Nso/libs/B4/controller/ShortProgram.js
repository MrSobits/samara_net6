Ext.define('B4.controller.ShortProgram', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.ButtonDataExport'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['ShortProgramRecord'],
    stores: ['ShortProgramRecord'],
    views: [
        'shortprogram.Grid'
    ],

    mainView: 'shortprogram.Grid',
    mainViewSelector: 'shortprogramgrid',

    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'buttonExportAspect',
            gridSelector: 'shortprogramgrid',
            buttonSelector: 'shortprogramgrid #btnExport',
            controllerName: 'ShortProgramRecord',
            actionName: 'Export'
        }
    ],
    
    init: function() {
        var me = this,
            actions = {
                'shortprogramgrid b4updatebutton': {
                    click: {
                        fn: function(btn) {
                            btn.up('shortprogramgrid').getStore().load();
                        }
                    }
                }
            };

        me.control(actions);

        me.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('shortprogramgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('ShortProgramRecord').load();
    }
});