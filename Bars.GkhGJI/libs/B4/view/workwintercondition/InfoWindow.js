Ext.define('B4.view.workwintercondition.InfoWindow', {
    extend: 'Ext.window.Window',
    title: 'Сведения о подготовке жилищно-коммунального хозяйства к работе в зимних условиях',
    alias: 'widget.workWinterConditionInfoWindow',
    closable: true,
    bodyStyle: Gkh.bodyStyle,
    maximized: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    requires: [
        'B4.view.workwintercondition.InfoGrid',
        'B4.store.workwintercondition.Information',
        'B4.model.workwintercondition.Information',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],
    
    initComponent: function () {
        var me = this;
        var months = {};

        months[1] = "Январь";
        months[2] = "Февраль";
        months[3] = "Март";
        months[4] = "Апрель";
        months[5] = "Май";
        months[6] = "Июнь";
        months[7] = "Июль";
        months[8] = "Август";
        months[9] = "Сентябрь";
        months[10] = "Октябрь";
        months[11] = "Ноябрь";
        months[12] = "Декабрь";

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    padding: 5,
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    layout: 'vbox',
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Municipality',
                            width: 500,
                            labelWidth: 196,
                            fieldLabel: 'Муниципальное образование',
                            editable: false,
                            readOnly: true,
                            valueToRaw: function (value) {
                                return value && Ext.isString(value) ? value : value && value.Name ? value.Name : '';
                            }
                        },
                        {
                            xtype: 'container',
                            width: 500,
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'label',
                                    text: 'Отчетный период:',
                                    width: 200
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Month',
                                    labelWidth: 50,
                                    editable: false,
                                    readOnly: true,
                                    flex: 1,
                                    valueToRaw: function(value) {
                                        return months[value] ? months[value] : ('' + Ext.value(value, ''));
                                    }
                                },
                                {
                                    xtype: 'textfield',
                                    margin: '0 0 0 10',
                                    name: 'Year',
                                    labelWidth: 50,
                                    editable: false,
                                    readOnly: true,
                                    flex: 1
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'workWinterConditionInfoGrid',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Копирование данных из периода',
                                    icon: B4.Url.content('content/img/icons/page_copy.png'),
                                    action: 'CopyWorkWinterPeriod'
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