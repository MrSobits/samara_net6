Ext.define('B4.view.priorityparam.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.priorityparamgrid',

    requires: [],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.base.Store', {
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'PriorityParam'
                },
                fields: [{ name: 'Id' }, { name: 'Name' }, { name: 'Type' }]
            });

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Параметр'
                }
            ],
            viewConfig: {
                loadMask: true
            }
        });
        me.callParent(arguments);
    }
});