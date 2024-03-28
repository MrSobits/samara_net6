Ext.define('B4.controller.realityobj.DecisionHistory', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.mixins.Context',
        'B4.Ajax',
        'B4.Url',
        'B4.QuickMsg'
    ],

    views: [
        'realityobj.decisionhistory.MainPanel',
        'realityobj.decisionhistory.TreePanel',
        'realityobj.decisionhistory.JobYearsGrid'
    ],
    parentCtrlCls: 'B4.controller.realityobj.Navi',

    models: [],

    stores: [
        'realityobj.decisionhistory.JobYears',
        'realityobj.decisionhistory.Tree'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'decisionhistorymainpanel'
        },
        {
            ref: 'treePanel',
            selector: 'decisionhistorytreepanel'
        },
        {
            ref: 'jobYearsGrid',
            selector: 'decisionhistoryjobyearsgrid'
        }
    ],

    init: function () {
        var me = this;
        me.control({
            'decisionhistorytreepanel': {
                afterrender: function (cmp) {
                    var store = cmp.getStore();
                    store.on('beforeload', function (s, operation) {
                        operation.params.roId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
                    });
                    store.load();
                }
            },
            'decisionhistoryjobyearsgrid' : {
                afterrender: function (cmp) {
                    var store = cmp.getStore();
                    store.on('beforeload', function (s, operation) {
                        operation.params.roId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
                    });
                    store.load();
                }
            },
            'decisionhistorytreepanel b4updatebutton': {
                click: function(btn) {
                    btn.up('treepanel').getStore().load();
                }
            },
            'decisionhistoryjobyearsgrid b4updatebutton' : {
                click: function (btn) {
                    btn.up('grid').getStore().load();
                }
            }
        });

        me.callParent(arguments);
    },
    
    index: function(id) {
        var me = this,
            view = me.getMainView() || Ext.widget('decisionhistorymainpanel');
        
        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
    }
    
});