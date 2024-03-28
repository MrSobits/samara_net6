Ext.define('B4.view.heatinputperiod.Panel', {
    extend: 'Ext.window.Window',
    
    alias: 'widget.heatinputinfoPanel',

    requires: [
        'B4.view.heatinputperiod.HeatInputInfoGrid',
        'B4.view.heatinputperiod.BoilerGrid',
        'B4.store.heatinputperiod.Boiler',
        'B4.store.heatinputperiod.Information',
        'B4.model.heatinputperiod.Information',
        'B4.model.heatinputperiod.Boiler',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],
    
    closable: true,
    modal: true,
    width: 1000,
    height: 400,
    bodyPadding: 5,
    border: false,
    title: 'Информация о подаче тепла',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    padding: 5,
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            labelAlign: 'right',
                            labelWidth: 170,
                            editable: false,
                            readOnly: true,
                            name: 'Municipality',
                            fieldLabel: 'Муниципальное образование',
                            flex: 1
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch',
                                pack: 'start'
                            },
                            defaults: {
                                editable: false,
                                readOnly: true,
                                padding: '0 5 0 0'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    labelAlign: 'right',
                                    labelWidth: 170,
                                    width: 270,
                                    name: 'MonthStr',
                                    fieldLabel: 'Отчетный период'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Year',
                                    width: 50
                                }
                            ]
                        }]
                },
                {
                    xtype: 'heatInputInfoGrid',
                    scroll: 'vertical',
                    flex: 1
                },
                {
                    xtype: 'boilerGrid',
                    flex: 0,
                    height: 70
                }],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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