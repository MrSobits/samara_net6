Ext.define('B4.view.report.CountSumActAcceptedPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'countSumActAcceptedPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            },
            items: [
                {
                    xtype: 'gkhtriggerfield',
                    name: 'ProgramsCr',
                    itemId: 'tfProgramsCr',
                    fieldLabel: 'Программы кап.ремонта',
                    allowBlank: false
                }
            ]
        });

        me.callParent(arguments);
    }
});