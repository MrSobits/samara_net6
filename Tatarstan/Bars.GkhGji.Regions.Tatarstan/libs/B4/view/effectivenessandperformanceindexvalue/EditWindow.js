Ext.define('B4.view.effectivenessandperformanceindexvalue.EditWindow', {
    extend: 'B4.form.Window',

    width: 390,
    height: 210,
    bodyPadding: 5,
    title: 'Значение показателя',
    alias: 'widget.effectivenessandperformanceindexvalueeditwindow',
    layout: 'border',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.store.dict.EffectivenessAndPerformanceIndex',
        'B4.form.SelectField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'left'
                    },
                    items:
                        [
                            {
                                xtype: 'b4selectfield',
                                name: 'EffectivenessAndPerformanceIndex',
                                fieldLabel: 'Показатель',
                                editable: false,
                                store: 'B4.store.dict.EffectivenessAndPerformanceIndex',
                                isGetOnlyIdProperty: false,
                                allowBlank: false,
                                columns: [
                                    { text: 'Наименование показателя', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                ]
                            },
                            {
                                xtype: 'datefield',
                                name: 'CalculationStartDate',
                                fieldLabel: 'Дата начала расчета',
                                allowBlank: false,
                                format: 'd.m.Y'
                            },
                            {
                                xtype: 'datefield',
                                name: 'CalculationEndDate',
                                fieldLabel: 'Дата окончания расчета',
                                allowBlank: false,
                                format: 'd.m.Y'
                            },
                            {
                                xtype: 'textfield',
                                name: 'Value',
                                fieldLabel: 'Значение',
                                allowBlank: false,
                                maxLength: 255
                            },
                        ]
                },
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
                                    xtype: 'tbfill'
                                },
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