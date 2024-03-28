Ext.define('B4.view.nonresidentialplacement.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.nonresidentialplacementeditwindow',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'fit',
    width: 700,
    height: 440,
    maxHeight: 440,
    bodyPadding: 5,
    itemId: 'nonResidentialPlacementEditWindow',
    title: 'Редактирование записи',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhDecimalField',
        'B4.view.nonresidentialplacement.MeteringDeviceGrid',
        
        'B4.enums.TypeContragentDi'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    margins: -1,
                    layout: 'fit',
                    items: [
                        {
                            xtype: 'panel',
                            region: 'north',
                            border: false,
                            title: 'Реквизиты',
                            bodyStyle: Gkh.bodyStyle,
                            items: [
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelAlign: 'right',
                                        labelWidth: 190
                                    },
                                    title: 'Общие сведения',
                                    items: [
                                        {
                                            xtype: 'combobox', editable: false,
                                            fieldLabel: 'Тип контрагента',
                                            store: B4.enums.TypeContragentDi.getStore(),
                                            displayField: 'Display',
                                            valueField: 'Value',
                                            name: 'TypeContragentDi',
                                            itemId: 'cbTypeContragentDi'
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'Area',
                                            fieldLabel: 'Площадь помещения (кв.м.)',
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'ContragentName',
                                            fieldLabel: 'Наименование контрагента',
                                            allowBlank: false,
                                            maxLength: 300
                                        },
                                        {
                                            xtype: 'container',
                                            anchor: '100%',
                                            defaults: {
                                                anchor: '100%',
                                                labelAlign: 'right',
                                                labelWidth: 190,
                                                flex: 1
                                            },
                                            layout: {
                                                type: 'hbox',
                                                pack: 'start'
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    format: 'd.m.Y',
                                                    name: 'DateStart',
                                                    fieldLabel: 'Дата начала'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    format: 'd.m.Y',
                                                    name: 'DateEnd',
                                                    fieldLabel: 'Дата окончания'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelAlign: 'right',
                                        labelWidth: 190
                                    },
                                    title: 'Предоставление жилищных услуг',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentNameApartment',
                                            fieldLabel: 'Документ',
                                            maxLength: 300
                                        },
                                        {
                                            xtype: 'container',
                                            anchor: '100%',
                                            defaults: {
                                                anchor: '100%',
                                                labelAlign: 'right',
                                                labelWidth: 190,
                                                flex: 1
                                            },
                                            layout: {
                                                type: 'hbox',
                                                pack: 'start'
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'DocumentNumApartment',
                                                    fieldLabel: 'Номер',
                                                    maxLength: 50
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    format: 'd.m.Y',
                                                    name: 'DocumentDateApartment',
                                                    fieldLabel: 'Дата'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelAlign: 'right',
                                        labelWidth: 190
                                    },
                                    title: 'Предоставление коммунальных услуг',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentNameCommunal',
                                            fieldLabel: 'Документ',
                                            maxLength: 300
                                        },
                                        {
                                            xtype: 'container',
                                            anchor: '100%',
                                            defaults: {
                                                anchor: '100%',
                                                labelAlign: 'right',
                                                labelWidth: 190,
                                                flex: 1
                                            },
                                            layout: {
                                                type: 'hbox',
                                                pack: 'start'
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'DocumentNumCommunal',
                                                    fieldLabel: 'Номер',
                                                    maxLength: 50
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    format: 'd.m.Y',
                                                    name: 'DocumentDateCommunal',
                                                    fieldLabel: 'Дата'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'meterdevicegrid',
                            region: 'center'
                        }
                    ]
                }
            ],
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
                        {
                            xtype: 'tbfill'
                        },
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