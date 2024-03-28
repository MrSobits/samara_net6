Ext.define('B4.view.regressionanalysis.Panel', {
    extend: 'Ext.form.Panel',
    requires: [
        'B4.form.MonthPicker',
        'B4.view.regressionanalysis.Chart',
        'B4.form.GisTreeSelectField',
        'B4.form.SelectField',
        'B4.store.regressionanalysis.HouseType',
        'B4.store.multipleAnalysis.MunicipalityFias',
        'B4.store.regressionanalysis.IndicatorType'
    ],

    title: 'Регрессионный анализ',
    alias: 'widget.regressionanalysispanel',
    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'center'
    },
    bodyPadding: 10,

    closable: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {                        
                    xtype: 'container',                                        
                    layout: {
                        type: 'hbox',
                        align: 'top',
                        pack: 'center'
                    },
                    items: [
                        {
                            xtype: 'container',
                            flex: 1,                            
                            maxWidth: 400,
                            layout: 'anchor',
                            defaults: {
                                labelAlign: 'right',
                                margin: 10,
                                anchor: '0'
                            },
                            items: [
                                {
                                    xtype: 'gistreeselectfield',
                                    name: 'HouseType',
                                    fieldLabel: 'Тип дома',
                                    labelWidth: 60,
                                    titleWindow: 'Выбор типа дома',
                                    store: 'B4.store.regressionanalysis.HouseType',
                                    allowBlank: false,
                                    editable: false
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4monthpicker',
                                            name: 'dateBegin',
                                            fieldLabel: 'с:',
                                            labelWidth: 60,
                                            format: 'F, Y',
                                            maxWidth: 200,
                                            flex: 1,
                                            allowBlank: false,
                                            editable: false
                                        },
                                        {
                                            xtype: 'b4monthpicker',
                                            name: 'dateEnd',
                                            fieldLabel: 'по:',
                                            labelWidth: 60,
                                            format: 'F, Y',
                                            maxWidth: 200,
                                            flex: 1,
                                            allowBlank: false,
                                            editable: false
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            flex: 1,
                            layout: 'anchor',
                            defaults: {
                                labelAlign: 'right',
                                margin: 10,
                                anchor: '0',
                                labelWidth: 70,
                            },
                            items: [
                                {
                                    xtype: 'gistreeselectfield',
                                    name: 'IndicatorSelectField',
                                    fieldLabel: 'Индикатор',
                                    titleWindow: 'Выбор индикатора',
                                    store: 'B4.store.regressionanalysis.IndicatorType',
                                    allowBlank: false,
                                    editable: false,
                                    multiSelect: true
                                },
                                {
                                    xtype: 'container',
                                    anchor: '0',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 70,
                                        flex: 1,
                                        labelAlign: 'right',
                                        editable: false,
                                        selectionMode: 'SINGLE',
                                        windowCfg: {
                                            modal: true
                                        },
                                        columns: [
                                            {
                                                text: 'Наименование',
                                                dataIndex: 'Name',
                                                flex: 1,
                                                filter: { xtype: 'textfield' }
                                            }
                                        ]
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            fieldLabel: 'МР',
                                            store: Ext.create('B4.store.multipleAnalysis.MunicipalityFias'),
                                            name: 'MunicipalArea'
                                        },
                                        //{
                                        //    xtype: 'b4selectfield',
                                        //    fieldLabel: 'Нас. пункт',
                                        //    store: Ext.create('B4.store.volumediscrepancy.Settlement'),
                                        //    disabled: true,
                                        //    name: 'Settlement'
                                        //},
                                        //{
                                        //    xtype: 'b4selectfield',
                                        //    fieldLabel: 'Улица',
                                        //    store: Ext.create('B4.store.volumediscrepancy.Street'),
                                        //    disabled: true,
                                        //    selectionMode: 'MULTI',
                                        //    onSelectAll: function() {
                                        //        this.fireEvent('selectAll', this);
                                        //    },
                                        //    name: 'Street'
                                        //}
                                    ]
                                }
                            ]
                        }
                    ]                        
                },
                {
                    xtype: 'container',
                    flex: 1,
                    layout: 'fit',
                    items: [
                        {
                            xtype: 'regressionanalysischart',
                            margin: 10
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
                                    xtype: 'button',
                                    name: 'MakeChart',
                                    iconCls: 'icon-accept',
                                    text: 'Сформировать'
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