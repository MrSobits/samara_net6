Ext.define('B4.view.objectcr.DefectListEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    minWidth: 700,
    maxWidth: 700,
    maxHeight: 500,
    minHeight: 220,
    bodyPadding: 5,
    
    alias: 'widget.objectcrdefectlistwin',
    itemId: 'defectListEditWindow',
    title: 'Дефектная ведомость',

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Work',
        'B4.enums.PriceCalculateBy',
        'B4.enums.TypeDefectList',
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.enums.YesNo'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'TypeDefectList',
                    fieldLabel: 'Тип ведомости',
                    items: B4.enums.TypeDefectList.getItems(),
                    editable: false,
                    operand: CondExpr.operands.eq,
                    valueField: 'Value',
                    displayField: 'Display'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Work',
                    fieldLabel: 'Вид работы',
                    store: 'B4.store.dict.Work',
                    editable: false,
                    itemId: 'sfWork',
                    allowBlank: false
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Документ',
                    itemId: 'tfDocumentName',
                    maxLength: 300,
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'DocumentDate',
                    fieldLabel: 'Дата документа',
                    format: 'd.m.Y',
                    width: 250,
                    anchor: null,
                    itemId: 'dfDocumentDate',
                    allowBlank: false
                },
                {
                    xtype: 'b4filefield',
                    editable: false,
                    name: 'File',
                    fieldLabel: 'Файл',
                    itemId: 'ffFile',
                    allowBlank: false
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    name: 'Sum',
                    fieldLabel: 'Сумма по ведомости, руб',
                    type: 'CrSum',
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    decimalSeparator: ',',
                    allowBlank: true
                },
                {
                    xtype: 'b4combobox',
                    name: 'CalculateBy',
                    fieldLabel: 'Показатель для расчета стоимости',
                    readOnly: true,
                    items: B4.enums.PriceCalculateBy.getItems(),
                    editable: false
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    fieldLabel: 'Выводить документ на портал',
                    name: 'UsedInExport',
                    store: B4.enums.YesNo.getStore(),
                    displayField: 'Display',
                    valueField: 'Value'
                },
                {
                    xtype: 'fieldset',
                    title: 'Исходные данные',
                    fieldsetType: 'DpkrInfo',
                    layout: { type: 'vbox', align: 'stretch' },
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    items: [
                            {
                                xtype: 'container',
                                fieldLabel: 'Объем',
                                layout: { type: 'hbox', align: 'stretch' },
                                defaults: {
                                    xtype: 'numberfield',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    decimalSeparator: ',',
                                    labelAlign: 'right',
                                    readOnly: true,
                                    flex: 1
                                },
                                items: [
                                    {
                                        labelWidth: 150,
                                        fieldLabel: 'Объем по ДПКР',
                                        name: 'DpkrVolume'
                                    },
                                    {
                                        labelWidth: 180,
                                        fieldLabel: 'Предельная стоимость на единицу объема, руб',
                                        name: 'MargCost'
                                    }
                                ]
                            },
                            {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                fieldLabel: 'Общая стоимость работы по ДПКР, руб.',
                                name: 'DpkrCost',
                                keyNavEnabled: false,
                                mouseWheelEnabled: false,
                                readOnly: true,
                                decimalSeparator: ',',
                                flex: 1
                            }
                    ]
                },
                {
                    xtype: 'fieldset',
                    fieldsetType: 'CrInfo',
                    title: 'Расчет стоимости',
                    layout: { type: 'vbox', align: 'stretch' },
                    items: [
                        {
                            xtype: 'container',
                            fieldLabel: 'Объем',
                            layout: { type: 'hbox', align: 'stretch' },
                            defaults: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                keyNavEnabled: false,
                                mouseWheelEnabled: false,
                                decimalSeparator: ',',
                                labelAlign: 'right',
                                minValue: 0,
                                flex: 1
                            },
                            items: [
                                {
                                    labelWidth: 150,
                                    fieldLabel: 'Объем по ведомости',
                                    name: 'Volume'
                                },
                                {
                                    labelWidth: 180,
                                    fieldLabel: 'Стоимость на единицу объема по ведомости, руб.:',
                                    name: 'CostPerUnitVolume'
                                }
                            ]
                        },
                        {
                            xtype: 'fieldcontainer',
                            labelWidth: 150,
                            labelAlign: 'right',
                            margin: '0 0 15 0',
                            fieldLabel: 'Общая стоимость работы по ведомости, руб.',
                            layout: { type: 'hbox', align: 'stretch' },
                            items: [
                                {
                                    xtype: 'numberfield',
                                    hideTrigger: true,
                                    name: 'SumTotal',
                                    type: 'DpkrSum',
                                    margin: '0 15 0 0',
                                    minValue: 0,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    decimalSeparator: ',',
                                    flex: 1
                                },
                                {
                                    xtype: 'button',
                                    text: 'Расчет',
                                    action: 'Calculate'
                                }
                            ]
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
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    disabled: true,
                                    menu: []
                                }
                            ]
                        },
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