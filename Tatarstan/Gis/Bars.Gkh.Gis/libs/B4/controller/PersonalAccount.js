Ext.define('B4.controller.PersonalAccount', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkuInfoToolbar'],

    mixins: { context: 'B4.mixins.Context', mask: 'B4.mixins.MaskBody' },
    views: ['personalAccount.Grid'],
    mainView: 'personalAccount.Grid',
    mainViewSelector: 'personal_account_grid',

    aspects: [
        {
            xtype: 'gkuInfoToolbar'
        }
    ],

    init: function () {
        var me = this;
        me.callParent(arguments);

        me.control({
            'personal_account_grid': {
                'rowaction': {
                    fn: me.rowAction,
                    scope: me
                },
                'itemdblclick': {
                    fn: me.rowDblClick,
                    scope: me
                }
            }
        });
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('personal_account_grid');
            
        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
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
            id = record ? record.getId() : null,
            realityObjectId = record ? record.get('RealityObjectId') : null;
        me.application.redirectTo(Ext.String.format('#personalaccountinfo/{0}/{1}', realityObjectId, id));
    }
});