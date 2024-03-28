Ext.define('B4.view.objectcr.scheduleexecutionwork.AddDateWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 700,
    height: 500,
    bodyPadding: 5,
    itemId: 'scheduleExecutionWorkAddDateWindow',
    closeAction: 'destroy',
    closable: false,
    requires: [
        'B4.view.objectcr.scheduleexecutionwork.AddDateGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'schexworkdategrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});