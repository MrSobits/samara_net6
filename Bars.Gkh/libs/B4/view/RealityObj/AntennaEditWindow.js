Ext.define('B4.view.realityobj.AntennaEditWindow', {
    extend: 'B4.form.Window',
    itemId: 'realityobjAntennaEditWindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 400,
    minHeight: 250,
    bodyPadding: 5,
    title: 'Сведения о СКПТ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.enums.YesNoMinus',
        'B4.enums.AntennaRange',
        'B4.enums.AntennaReason',
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                //{
                //    xtype: 'container',
                //    layout: 'hbox',
                //    margin: '10 0 0 0',
                //    defaults: {
                //        labelWidth: 200,
                //        width: 370,
                //        labelAlign: 'right'
                //    },
                //    items: [
                //        {
                //            xtype: 'combobox',
                //            name: 'Availability',
                //            fieldLabel: 'Наличие в составе общедомового имущества СКПТ',
                //            displayField: 'Display',
                //            itemId: 'cbavailfield',
                //            store: B4.enums.YesNo.getStore(),
                //            valueField: 'value'
                //        }
                //    ]
                //},
                {
                    xtype: 'container',
                    layout: 'hbox',
                    itemId: 'avail',
                    margin: '10 0 0 0',
                    hidden: false,
                    defaults: {
                        labelWidth: 200,
                        width: 370,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'Availability',
                            fieldLabel: 'Наличие в составе общедомового имущества СКПТ',
                            displayField: 'Display',
                            itemId: 'availchild',
                            store: B4.enums.YesNoNotSet.getStore(),
                            valueField: 'Value',
                            allowBlank: false,
                            editable: false
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 200,
                        width: 370,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'Workability',
                            fieldLabel: 'Работоспособность',
                            displayField: 'Display',
                            itemId: 'work',
                            hidden: true,
                            store: B4.enums.YesNoMinus.getStore(),
                            valueField: 'Value',
                            editable: false
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 200,
                        width: 370,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'Range',
                            itemId: 'range',
                            hidden: true,
                            fieldLabel: 'Диапазон',
                            displayField: 'Display',
                            store: B4.enums.AntennaRange.getStore(),
                            valueField: 'Value',
                            editable: false
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'FrequencyFrom',
                            fieldLabel: 'Частота, МГц: от',
                            maskRe: /[,0-9]/,
                            itemId: 'freq',
                            hidden: true,
                            labelWidth: 200,
                            width: 270,
                            maxLength: 10,
                            allowBlank: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'FrequencyTo',
                            fieldLabel: 'до',
                            itemId: 'freqto',
                            hidden: true,
                            maskRe: /[,0-9]/,
                            labelWidth: 30,
                            width: 100,
                            allowBlank: true
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 200,
                        width: 270,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'NumberApartments',
                            fieldLabel: 'Количество квартир, использующих СКПТ',
                            itemId: 'apart',
                            hidden: true,
                            maxLength: 7,
                            minValue: 0,
                            allowExponential: false,
                            allowDecimals: false,
                            allowBlank: true
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    margin: '-20 0 0 0',
                    defaults: {
                        labelWidth: 200,
                        width: 450,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'Reason',
                            itemId: 'reason',
                            fieldLabel: 'Причины отсутствия СКПТ',
                            displayField: 'Display',
                            store: B4.enums.AntennaReason.getStore(),
                            valueField: 'Value',
                            hidden: true,
                            allowBlank: false,
                            editable: false
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    margin: '0 0 10 0',
                    defaults: {
                        labelWidth: 200,
                        width: 450,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4filefield',
                            name: 'FileInfo',
                            fieldLabel: 'Файл',
                            itemId: 'file',
                            hidden: true,
                            allowBlank: false
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
                            columns: 2,
                            items: [
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});