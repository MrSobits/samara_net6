Ext.define('B4.controller.realityobj.ChangeValuesHistory', {
    extend: 'B4.base.Controller',
    
    models: ['realityobj.ChangeValuesHistoryDetail'],
    stores: ['realityobj.ChangeValuesHistory'],
    views: [
        'realityobj.changevalueshistory.Grid',
        'realityobj.changevalueshistory.Window',
        'realityobj.changevalueshistory.WindowsGrid'
    ],

    mainView: 'B4.view.realityobj.changevalueshistory.Grid',
    mainViewSelector: 'changeValuesHistoryGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'changeValuesHistoryGrid'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'changeValuesHistoryGrid': {
                'afterrender': {
                    fn: function(grid) {
                        grid.getStore().on('beforeload', me.onBeforeLoad, me);
                    },
                    scope: me
                }
            },
            'changeValuesHistoryGrid b4updatebutton': {
                click: function (btn) {
                    btn.up('changeValuesHistoryGrid').getStore().load();
                }
            },
            'changeValuesHistoryGrid b4editcolumn': {
                click: { fn: me.editHistoryGridBtnClick, scope: me }
            }
        });
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('changeValuesHistoryGrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        view.getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
    },

    onItemDblClickHistoryGrid: function (view, record) {
        var detailWindow = this.componentQuery('changeValuesHistoryWindows');
       
        if (detailWindow) {
            detailWindow.show();
        } else {
            detailWindow = Ext.create('B4.view.realityobj.changevalueshistory.Window',
            {
                constrain: true,
                renderTo: B4.getBody().getActiveTab().getEl(),
                closeAction: 'destroy',
                ctxKey: this.getCurrentContextKey()
            });
            detailWindow.show();
        }

        detailWindow.down('changeValuesHistoryWindowsGrid').getStore().filter('logEntityId', record.get('Id'));
    },

    editHistoryGridBtnClick: function (gridView, rowIndex, colIndex, el, e, rec) {
        this.onItemDblClickHistoryGrid(gridView, rec);
    },

    componentQuery: function(selector) {
        if (this.hasContext()) {
            return this.getCmpInContext.apply(this, [selector, this.getCurrentContextKey()]);
        } else {
            return Ext.ComponentQuery.query(selector)[0];
        }
    },

    hasContext: function () {
        var hasContext = false;
        Ext.Object.each(this.mixins, function (key, value) {
            if (value.$className == 'B4.mixins.Context') {
                hasContext = true;
            }
            return !hasContext;
        });
        return hasContext;
    },

});
