Ext.define('B4.view.protocolgji.RealityObjListPanel', {
    extend: 'Ext.panel.Panel',
    storeName: null,
    title: 'Нарушения',
    itemId: 'protocolRealityObjListPanel',
    layout: {
        type: 'border'
    },
    minHeight: 300,
    border: false,
    alias: 'widget.protocolgjiRealityObjListPanel',

    requires: [
        'B4.view.protocolgji.ViolationGrid',
        'B4.view.protocolgji.RealityObjViolationGrid',
        'B4.form.SelectField',
        'B4.store.dict.ResolveViolationClaim'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    region: 'north',
                    itemId: 'protocolNorthPanel',
                    height: 100,
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
                            xtype: 'container',
                            layout: 'anchor',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 160
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата правонарушения',
                                    name: 'DateOfViolation'
                                },
                                {
                                    xtype: 'container',
                                    margin: '0 0 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 160
                                    },
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            name: 'HourOfViolation',
                                            fieldLabel: 'Время правонарушения',
                                            width: 210,
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
                                            maxValue: 59,
                                            minValue: 0
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'textarea',
                            fieldLabel: 'Свидетели',
                            flex: 1,
                            width: 350,
                            name: 'Witnesses'
                        },
                        {
                            xtype: 'textarea',
                            fieldLabel: 'Потерпевшие',
                            flex: 1,
                            width: 350,
                            name: 'Victims'
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.dict.ResolveViolationClaim',
                            fieldLabel: 'Наименование требования',
                            labelWidth: 160,
                            name: 'ResolveViolationClaim',
                            itemId: 'sfResolveViolationClaim',
                            hidden: true
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
                            border: false
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});