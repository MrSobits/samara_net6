Ext.define('B4.view.subsidy.SubsidyMuPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.subsidymunicipalitypanel',

    requires: [
        'B4.ux.button.Save',
        'B4.store.dict.Municipality',
        'B4.view.subsidy.SubsidyMuRecordGrid',
        'B4.store.dict.MunicipalityByOperator',
        'B4.form.ComboBox'
    ],

    bodyStyle: Gkh.bodyStyle,
    autoScroll: true,
    closable: true,
    
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    //layout: {
    //    type: 'border',
    //    padding: 3
    //},
    //defaults: {
    //    split: true
    //},
    title: 'Субсидирование',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            flex: 1,
                            items: [
                                {
                                    xtype: 'buttongroup',
                                    //maxWidth: 564,
                                    items: [
                                        {
                                            xtype: 'b4savebutton'
                                        },
                                        {
                                            xtype: 'tbseparator'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Рассчитать показатели',
                                            action: 'CalcValues',
                                            iconCls: 'icon-table-go'
                                        },
                                        {
                                            xtype: 'tbseparator'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Корректировка ДПКР',
                                            iconCls: 'icon-table-go',
                                            action: 'CorrectDpkr'
                                        },
                                        {
                                            xtype: 'tbseparator'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Результат корректировки',
                                            iconCls: 'icon-table-go',
                                            action: 'CorrectResult'
                                        },
                                        {
                                            xtype: 'tbseparator'
                                        },
                                        {
                                            xtype: 'button',
                                            iconCls: 'icon-table-go',
                                            text: 'Экспорт',
                                            textAlign: 'left',
                                            action: 'Export'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'tbseparator',
                                    width: 10
                                },
                                {
                                    xtype: 'b4combobox',
                                    url: '/Municipality/ListWithoutPaging',
                                    //listView: 'B4.view.dict.municipality.Grid',
                                    //store: 'B4.store.dict.MunicipalityByOperator',
                                    name: 'Municipality',
                                    fieldLabel: 'Муниципальное образование',
                                    align: 'stretch',
                                    labelWidth: 152,
                                    editable: false,
                                    flex: 1
                                }
                            ]
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'subsidymunicipalityrecordgrid',
                    flex: 1,
                    border: 0
                }
            ]
        });

        me.callParent(arguments);
    }
});