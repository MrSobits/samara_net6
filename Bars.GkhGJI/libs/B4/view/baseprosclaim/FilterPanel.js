Ext.define('B4.view.baseprosclaim.FilterPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.baseProsClaimFilterPanel',

    itemId: 'baseProsClaimFilterPanel',

    requires: [
        'B4.ux.button.Update'
    ],

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            defaults: {
                labelWidth: 130,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    border: false,
                    width: 650,
                    layout: {
                        pack: 'start',
                        type: 'hbox'
                    },
                    defaults: {
                        anchor: '100%',
                        format: 'd.m.Y',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            labelWidth: 125,
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
                            value: new Date(new Date().setDate(new Date().getDate() + 7))
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