Ext.define('B4.view.transferctr.RequestPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Заявки на перечисление денежных средств подрядчикам',
    alias: 'widget.requestTransferCtrPanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    requires: [
        'B4.view.transferctr.RequestGrid',
        'B4.view.Control.GkhTriggerField'
    ],
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    itemId: 'requestTransferCtrFilterPanel',
                    closable: false,
                    border: false,
                    bodyPadding: 5,
                    bodyStyle: Gkh.bodyStyle,
                    layout: 'anchor',
                    defaults: {
                        labelWidth: 90,
                        labelAlign: 'right',
                        height: 25
                    },
                    items: [
                        {
                            xtype: 'container',
                            border: false,
                            width: 650,
                            layout: 'hbox',
                            defaults: {
                                format: 'd.m.Y',
                                labelAlign: 'right',
                                anchor: '100%'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    labelWidth: 170,
                                    fieldLabel: 'Период с',
                                    width: 290,
                                    itemId: 'dfDateStart'
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 50,
                                    fieldLabel: 'по',
                                    width: 210,
                                    itemId: 'dfDateEnd'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            border: false,
                            width: 650,
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'gkhtriggerfield',
                                    name: 'Municipality',
                                    itemId: 'tfMunicipality',
                                    fieldLabel: 'Муниципальное образование',
                                    width: 500,
                                    anchor: '98%',
                                    labelWidth: 170
                                },
                                 {
                                     width: 10,
                                     xtype: 'component'
                                 },
                                {
                                    width: 100,
                                    anchor: '100%',
                                    itemId: 'updateGrid',
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'requesttransferctrgrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});
