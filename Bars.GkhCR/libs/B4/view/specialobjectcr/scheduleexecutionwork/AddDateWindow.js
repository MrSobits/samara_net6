Ext.define('B4.view.specialobjectcr.scheduleexecutionwork.AddDateWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    height: 500,
    bodyPadding: 5,

    itemId: 'scheduleExecutionSpecialWorkAddDateWindow',
    alias: 'widget.specialobjectcrschexworkdatewin',

    closeAction: 'destroy',
    closable: false,
    requires: [
        'B4.view.specialobjectcr.scheduleexecutionwork.AddDateGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'specialobjectcrschexworkdategrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});