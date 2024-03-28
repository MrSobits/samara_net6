Ext.define('B4.view.regop.cproc.types.ChargeWindow', {
    extend: 'B4.view.regop.cproc.types.ComPrBaseWindow',

    requires: [
        'B4.view.regop.unacceptedcharges.ChargesGrid'
    ],

    alias: 'widget.comprchargewindow',

    title: 'Расчет',

    taskId: null,

    initComponent: function() {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'unacceptedchargegrid'
                }
            ]
        });
        me.callParent(arguments);
    }
});