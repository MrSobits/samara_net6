Ext.define('B4.view.cscalculation.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.cscalculationeditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 1200,
    minWidth: 800,
    height: 650,
    resizable: true,
    bodyPadding: 3,
    itemId: 'cscalculationEditWindow',
    title: 'Расчет',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.form.EnumCombo',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.store.RealityObject',
        'B4.view.Control.GkhButtonPrint',
        'B4.form.SelectField',
        'B4.store.cscalculation.CSFormula',
        'B4.view.cscalculation.RowGrid',
        'B4.store.cscalculation.RoomList',
        'B4.form.ComboBox'     
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelAlign: 'right'
                    },
                    title: 'Расчет',
                    items: [                        
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            //      padding: '5',
                            defaults: {
                                labelWidth: 80,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Формула',
                                    name: 'CSFormula',
                                    labelWidth: 80,
                                    width: 800,
                                    store: 'B4.store.cscalculation.CSFormula',
                                    editable: false,
                                    allowBlank: false,
                                    columns: [
                                        {
                                            text: 'Наименование', dataIndex: 'Name', flex: 1,
                                            filter: {
                                                xtype: 'textfield'
                                            }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'CalcDate',
                                    fieldLabel: 'на дату',
                                    format: 'd.m.Y',
                                    itemId: 'dfDateTo',
                                    allowBlank: true
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                      //      padding: '5',
                            defaults: {
                                labelWidth: 80,
                                labelAlign: 'right'
                            },
                            items: [                                   
                                {
                                    xtype: 'textfield',
                                    name: 'Name',
                                    allowBlank: true,
                                    hidden: false,
                                    flex: 1,
                                    labelWidth: 80,
                                    fieldLabel: 'Наименование'
                                },
                                {
                                    xtype: 'numberfield',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    name: 'Result',
                                    itemId:'nfResult',
                                    fieldLabel: 'Результат',
                                    labelWidth: 70
                                },
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '5 0 0 0',
                            defaults: {
                                labelWidth: 80,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'RealityObject',
                                    fieldLabel: 'Жилой дом',
                                    textProperty: 'Address',
                                    store: 'B4.store.RealityObject',
                                    editable: false,
                                    flex: 1,
                                    itemId: 'sfRealityObject',
                                    allowBlank: false,
                                    columns: [
                                        {
                                            text: 'Муниципальное образование',
                                            dataIndex: 'Municipality',
                                            flex: 1,
                                            filter: {
                                                xtype: 'b4combobox',
                                                operand: CondExpr.operands.eq,
                                                storeAutoLoad: false,
                                                hideLabel: true,
                                                editable: false,
                                                valueField: 'Name',
                                                emptyItem: { Name: '-' },
                                                url: '/Municipality/ListWithoutPaging'
                                            }
                                        },
                                        {
                                            text: 'Адрес',
                                            dataIndex: 'Address',
                                            flex: 1,
                                            filter: { xtype: 'textfield' }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'Room',
                                    fieldLabel: 'Помещение',
                                    textProperty: 'RoomNum',
                                    store: 'B4.store.cscalculation.RoomList',
                                    editable: false,
                                    flex: 1,
                                    itemId: 'sfRoom',
                                    allowBlank: true,
                                    disabled: true,
                                    columns: [
                                        {
                                            text: 'Номер помещения',
                                            dataIndex: 'RoomNum',
                                            flex: 1,
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            text: 'Кадастровый номер',
                                            dataIndex: 'CadastralNumber',
                                            flex: 1,
                                            filter: { xtype: 'textfield' }
                                        }
                                    ]
                                } 
                            ]
                        },
                        {
                            xtype: 'textarea',
                            padding: '5 0 0 0',
                            name: 'Description',
                            labelWidth: 80,
                            fieldLabel: 'Подробности',
                            width: 800,
                            maxLength: 1500
                        }         
                        
                    ]
                },         
                {
                    xtype: 'cscalculationRowGrid',
                    disabled: true,
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Рассчитать',
                                    tooltip: 'Рассчитать',
                                    iconCls: 'icon-laptop',
                                    itemId: 'btncalculate'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
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