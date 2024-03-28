Ext.define('B4.view.appealcits.PrescriptionFondMainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Реестр предписаний ФКР',
    alias: 'widget.appealcitsPrescriptionFondMainPanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.appealcits.PrescriptionFondGrid',
        'B4.view.appealcits.PrescriptionFondFilterPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'appealcitsPrescriptionFondFilterPanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6'
                },
                {
                    xtype: 'prescriptionfondgrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
