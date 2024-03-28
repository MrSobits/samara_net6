Ext.define('B4.view.protocolgji.RealityObjListPanel', {
    extend: 'Ext.panel.Panel',
    storeName: null,
    title: 'Нарушения',
    itemId: 'protocolRealityObjListPanel',
    layout: {
        type: 'border'
    },

    alias: 'widget.protocolgjiRealityObjListPanel',

    requires: [
        'B4.view.protocolgji.ViolationGrid',
        'B4.view.protocolgji.RealityObjViolationGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    region: 'north',
                    itemId: 'protocolNorthPanel',
                    height: 37,
                    border: false,
                    unstyled: true,
                    layout: 'hbox',
                    padding: 5,
                    defaults: {
                        labelAlign: 'right'
                    },
                    shrinkWrap: true,
                    items: [
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата правонарушения',
                            name: 'DateOfViolation',
                            labelWidth: 150
                        },
                        {
                            xtype: 'numberfield',
                            name: 'HourOfViolation',
                            fieldLabel: 'Время правонарушения',
                            width: 210,
                            labelWidth: 150,
                            maxValue: 23,
                            minValue: 0
                        },
                        {
                            xtype: 'label',
                            text: ':',
                            margin: '5'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'MinuteOfViolation',
                            width: 45,
                            labelWidth: 150,
                            maxValue: 59,
                            minValue: 0
                        }
                    ]
                },

                {
                    xtype: 'panel',
                    region: 'west',
                    itemId: 'protocolWestPanel',
                    split: true,
                    collapsible: true,
                    border: false,
                    width: 400,
                    layout: 'fit',
                    items: [
                        {
                            xtype: 'protocolgjiRealityObjViolationGrid',
                            border: false
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    region: 'center',
                    layout: 'fit',
                    border: false,
                    items: [
                        {
                            xtype: 'protocolgjiViolationGrid',
                            border: false,
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});