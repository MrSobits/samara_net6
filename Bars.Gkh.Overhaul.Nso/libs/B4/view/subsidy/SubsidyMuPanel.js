Ext.define('B4.view.subsidy.SubsidyMuPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.subsidymunicipalitypanel',

    requires: [
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.view.subsidy.SubsidyMuRecordGrid'
    ],

    minWidth: 750,
    width: 750,
    autoScroll: true,
    bodyPadding: 5,
    closable: true,
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'border',
        padding: 3
    },
    defaults: {
        split: true
    },
    title: 'Задать параметры',
    trackResetOnLoad: true,
    frame: true,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'tbseparator'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Потребность до финансирования',
                                    action: 'FinanceNeedBefore',
                                    iconCls: 'icon-table-go',
                                    disabled: true
                                },
                                {
                                    xtype: 'tbseparator'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Рассчитать показатели',
                                    action: 'CalcValues',
                                    iconCls: 'icon-table-go',
                                    disabled: true
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-calculator',
                                    text: 'Корректировка ДПКР',
                                    action: 'CorrectDpkr',
                                    disabled: true
                                },
                                {
                                    xtype: 'button',
                                    text: 'Результат корректировки',
                                    iconCls: 'icon-table-go',
                                    action: 'CorrectResult'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    action: 'Export'
                                }
                            ]
                        }
                    ]
                }
            ],
            items: [
                {
                    region: 'west',
                    title:'Параметры субсидирования',
                    width: 300,
                    collapsible: true,
                    frame: true,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'top',
                        labelWidth: 220,
                        margin: '5 5 5 5',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        allowDecimals: true,
                        allowBlank: false
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'StartTarif',
                            fieldLabel: 'Начальный тариф, руб',
                            decimalSeparator: ',',
                            minValue: 0
                        },
                        {
                            xtype: 'numberfield',
                            name: 'CoefSumRisk',
                            fieldLabel: 'Коэффициент снижения лимита средств собственников, от 0 до 1',
                            decimalSeparator: ',',
                            minValue: 0,
                            maxValue: 1
                        },
                        {
                            xtype: 'numberfield',
                            name: 'CoefGrowthTarif',
                            fieldLabel: 'Коэффициент роста тарифа, %',
                            decimalSeparator: ',',
                            minValue: 0
                        },
                        {
                            xtype: 'numberfield',
                            name: 'DateReturnLoan',
                            fieldLabel: 'Срок возврата займа, лет',
                            minValue: 0,
                            maxValue: 100,
                            allowDecimals: false
                        },
                        {
                            xtype: 'numberfield',
                            name: 'CoefAvgInflationPerYear',
                            fieldLabel: 'Средний коэф-т инфляции в год, %',
                            decimalSeparator: ',',
                            minValue: 0
                        },
                        {
                            xtype: 'checkbox',
                            fieldLabel: 'Учитывать инфляцию',
                            name: 'ConsiderInflation',
                            labelAlign: 'left',
                            labelWidth: 150
                        }
                    ]
                },
                {
                    region:'center',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }],
                            store: 'B4.store.dict.Municipality',
                            itemId: 'sfSubsidyMunicipality',
                            fieldLabel: 'Муниципальное образование',
                            editable: false,
                            allowBlank: false,
                            labelAlign: 'right',
                            labelWidth: 220,
                            margin: '5 5 5 5'
                        },
                        {
                            xtype: 'b4selectfield',
                            store: Ext.create('Ext.data.Store', {
                                autoLoad: false,
                                fields: ['Id', 'Name'],
                                proxy: {
                                    type: 'ajax',
                                    url: B4.Url.action('ListMunicipalityProgramVersions', 'ProgramVersion'),
                                    reader: {
                                        type: 'json',
                                        root: 'data'
                                    }
                                }
                            }),
                            itemId: 'sfVersion',
                            fieldLabel: 'Версия',
                            editable: false,
                            allowBlank: false,
                            labelAlign: 'right',
                            labelWidth: 220,
                            margin: '5 5 5 5'
                        },
                        {
                            xtype: 'subsidymunicipalityrecordgrid',
                            flex: 1,
                            border: 0
                        }
                    ]
                }
            ]
        });
        
        me.callParent(arguments);
    }
});