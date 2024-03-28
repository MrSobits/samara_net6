Ext.define('B4.ux.grid.toolbar.Paging', {
    extend: 'Ext.toolbar.Paging',
    alias: 'widget.b4pagingtoolbar',

    initComponent: function () {
        var me = this;

        me.items = [
            {
                xtype: 'tbseparator'
            },
            {
                xtype: 'combo',
                editable: false,
                //hideTrigger: true,
                fieldLabel: 'Записей',
                width: 125,
                labelWidth: 50,
                mode: 'local',
                triggerAction: 'all',
                store: new Ext.data.SimpleStore({
                    fields: ['count'],
                    data: [[25], [50], [100], [200], [500], [1000], [1500], [2000], ['All']],
                    autoLoad: false
                }),
                valueField: 'count',
                displayField: 'count',
                listeners: {
                    select: function (cb) {
                        me.store.currentPage = 1;
                        me.store.pageSize = parseInt(cb.getRawValue(), 10);
                        me.doRefresh();
                    },
                    afterrender: function () {
                        this.setValue(me.store.pageSize);
                    }
                }
            }
        ];

        me.callParent(arguments);
    }
});