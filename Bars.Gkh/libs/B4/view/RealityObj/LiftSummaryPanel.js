Ext.define('B4.view.realityobj.LiftSummaryPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.roliftsummarypanel',
    layout: {
        type: 'hbox'
    },
    defaults: {
        flex: 1
    },
    trackResetOnLoad: true,
    autoScroll: true,
    border: false,
    bodyStyle: 'none 0px 0px repeat scroll transparent',
    requires: [
    ],

    initComponent: function () {
        var me = this;

        me.initialConfig = Ext.apply({
            trackResetOnLoad: true
        }, me.initialConfig);

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    margin: '0 30 0 0',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        flex: 1,
                        labelAlign: 'right',
                        labelWidth: 180,
                        readOnly: true,
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'fieldset',
                            magin: '0 0 10 0',
                            title: 'Количество лифтов',
                            defaults: {
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 180,
                                readOnly: true,
                                anchor: '100%'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'MainCount',
                                    fieldLabel: 'Общее'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'MainPassenger',
                                    fieldLabel: 'Пассажирских'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'MainCargo',
                                    fieldLabel: 'Грузовых'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'MainMixed',
                                    fieldLabel: 'Грузопассажирских'
                                }
                            ]
                        }
                    ]
                }   
            ]
        });

        me.callParent(arguments);
    }
});
