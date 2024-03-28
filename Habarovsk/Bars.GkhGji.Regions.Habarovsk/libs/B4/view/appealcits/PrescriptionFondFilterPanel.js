Ext.define('B4.view.appealcits.PrescriptionFondFilterPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.appealcitsPrescriptionFondFilterPanel',
    closable: false,
    header: false,
    layout: 'anchor',
    bodyPadding: 5,
    itemId: 'appealcitsPrescriptionFondFilterPanel',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.ux.button.Update',
        'B4.view.Control.GkhTriggerField'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 130,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    border: false,
                    width: 650,
                    layout: 'hbox',
                    defaults: {
                        format: 'd.m.Y',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            labelWidth: 130,
                            fieldLabel: 'Период с',
                            width: 290,
                            itemId: 'dfDateStart',
                            value: new Date(new Date().getFullYear(), 0, 1)
                        },
                        {
                            xtype: 'datefield',
                            labelWidth: 50,
                            fieldLabel: 'по',
                            width: 210,
                            itemId: 'dfDateEnd',
                            value: new Date(new Date().getFullYear(), 11, 31)
                        },
                        {
                            width: 100,
                            itemId: 'updateGrid',
                            xtype: 'b4updatebutton'
                        }
                    ]
                }             
            ]
        });
        me.callParent(arguments);
    }
});