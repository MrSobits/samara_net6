Ext.define('B4.controller.realityobj.ExistingSolutions', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.view.realityobj.ExistingSolutionsPanel'
    ],

    mainView: 'realityobj.ExistingSolutionsPanel',
    mainViewSelector: '#realityobjExistingSolutionsPanel',

    stores: ['MonthlyFeeAmountDecHistory'],

    views: [
        'realityobj.MonthlyFeeHistoryGrid'
    ],

    models: [
        'ExistingSolutionsModel'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'existingsolutionspanel'
        }
    ],

    init: function () {
        var me = this;

        me.getStore('MonthlyFeeAmountDecHistory').on('beforeload', me.onBeforeLoad, me);
        
        this.control({
            'monthlyfeehistorygrid b4updatebutton': { 'click': { fn: this.updateHistoryGrid } },
            'existingsolutionspanel': {
                render: function(panel) {
                    me.updatePanel(panel);
                }
            },
            'existingsolutionspanel b4updatebutton': {
                click: function(b) {
                    me.updatePanel(b.up('existingsolutionspanel'));
                }
            }
        });

        this.callParent(arguments);
    },

    onLaunch: function () {
        var me = this,
            store = me.getStore('MonthlyFeeAmountDecHistory');
        store.load();
    },
    
    updateHistoryGrid: function (btn) {
        var store = btn.up('monthlyfeehistorygrid').getStore();       
        store.load();
    },
    

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.realityObjId = this.params.realityObjectId;
        }
    },

    updatePanel: function (panel) {
        var me = this,
            model = me.getModel('ExistingSolutionsModel');
        me.mask();
        model.load(me.params.realityObjectId, {
            success: function (rec) {
                panel.down('form').getForm().loadRecord(rec);
            },
            failure: function() {
                B4.QuickMsg.msg('Ошибка','Ошибка при получении данных. Попоробуйте перезагрузить страницу.','error');
            },
            callback: function () {
                me.unmask();
            }
        });
    }
});