Ext.define('B4.view.dict.workprice.EditWindow', {
    extend: 'B4.form.Window',
    
    alias: 'widget.workpricewindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        
        'B4.form.SelectField',
        'B4.view.dict.job.Grid',
        'B4.store.dict.Job',

        'B4.store.dict.JobTreeStore',
        'B4.form.TreeSelectField',
        'B4.store.dict.CapitalGroup',
        'B4.store.dict.municipality.ListByParamAndOperator'
    ],

    layout: 'form',
    modal: true,
    width: 500,
    bodyPadding: 5,
    title: 'Редактирование',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'treeselectfield',
                    name: 'Job',
                    fieldLabel: 'Работа',
                    titleWindow: 'Выбор работы',
                    store: 'B4.store.dict.JobTreeStore',
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.CapitalGroup',
                    name: 'CapitalGroup',
                    fieldLabel: 'Группа капитальности'
                },
                {
                    xtype: 'numberfield',
                    name: 'NormativeCost',
                    fieldLabel: 'Стоимость на единицу объема КЭ, руб.',
                    minValue: 0,
                    hideTrigger: true,
                    allowDecimals: true,
                    negativeText: 'Значение не может быть отрицательным',
                    anchor: '100%'
                },
                {
                    xtype: 'numberfield',
                    name: 'SquareMeterCost',
                    fieldLabel: 'Стоимость на единицу площади МКД, руб.',
                    minValue: 0,
                    hideTrigger: true,
                    allowDecimals: true,
                    negativeText: 'Значение не может быть отрицательным',
                    anchor: '100%'
                },
                {
                    xtype: 'numberfield',
                    name: 'Year',
                    fieldLabel: 'Год',
                    anchor: '100%',
                    minValue: 0,
                    hideTrigger: true,
                    allowDecimals: false,
                    negativeText: 'Значение не может быть отрицательным',
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Муниципальное образование',
                    name: 'Municipality',                   
                    store: 'B4.store.dict.municipality.ListByParamAndOperator',
                    editable: false,
                    allowBlank: true,
                    columns: [
                        {
                            text: 'Наименование', dataIndex: 'Name', flex: 1,
                            filter: {
                                xtype: 'textfield'
                            }
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