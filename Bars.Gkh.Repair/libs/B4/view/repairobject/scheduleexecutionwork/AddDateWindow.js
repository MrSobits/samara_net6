Ext.define('B4.view.repairobject.scheduleexecutionwork.AddDateWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 700,
    height: 500,
    itemId: 'scheduleExecutionRepairWorkAddDateWindow',
    closeAction: 'destroy',
    closable: false,
    title: 'Дополнительный срок',
    requires: [
        'B4.view.repairobject.scheduleexecutionwork.AddDateGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'schexrepairworkdategrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});