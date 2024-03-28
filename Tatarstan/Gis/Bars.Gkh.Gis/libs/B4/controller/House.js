Ext.define('B4.controller.House', {
    extend: 'B4.base.Controller',
    
    mixins: { context: 'B4.mixins.Context' },
    views: ['house.Grid'],
    mainView: 'house.Grid',
    mainViewSelector: 'house_grid',
    
    init: function () {
        this.control({
            'house_grid b4updatebutton': {
                'click': function() {
                    this.getMainView().getStore().reload();
                },
                scope: this
            },
            'house_grid': {
                'rowaction': {
                    fn: this.rowAction,
                    scope: this
                },
                'itemdblclick': {
                    fn: this.rowDblClick,
                    scope: this
                }
            }

        });
        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('house_grid');

        this.bindContext(view);
        this.application.deployView(view);
    },

    rowAction: function (grid, action, record) {
        if (!grid || grid.isDestroyed) return;
        if (action.toLowerCase() == 'edit' && this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
            this.editRecord(record);
        }
    },

    rowDblClick: function (view, record) {
        if (!view || view.isDestroyed) return;
        this.editRecord(record);
    },

    editRecord: function (record) {
        var me = this,
            id = record ? record.getId() : null;

        me.application.redirectTo(Ext.String.format('#houseinfo/{0}', id));
    }
});