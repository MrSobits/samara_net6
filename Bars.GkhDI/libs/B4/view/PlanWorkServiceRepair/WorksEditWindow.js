Ext.define('B4.view.planworkservicerepair.WorksEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    minWidth: 520,
    width: 520,
    minHeight: 270,
    bodyPadding: 5,
    itemId: 'planWorkServiceRepairWorksEditWindow',
    title: 'Работа по содержанию и ремонту',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.form.SelectField',
        'B4.store.dict.PeriodicityTemplateService'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'WorkRepairListName',
                    fieldLabel: 'Наименование работы',
                    readOnly: true
                },
                {
                    xtype: 'b4selectfield',
                    name: 'PeriodicityTemplateService',
                    fieldLabel: 'Периодичность',
                    textProperty: 'Name',
                    store: 'B4.store.dict.PeriodicityTemplateService',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'DateComplete',
                    format: 'd.m.Y',
                    fieldLabel: 'Дата выполнения'
                },
                {
                    xtype: 'textfield',
                    name: 'DataComplete',
                    fieldLabel: 'Сведения о выполнении',
                    maxLength: 300
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        xtype: 'numberfield',
                        labelAlign: 'right',
                        labelWidth: 150,
                        hideTrigger: true,
                        flex: 1
                    },
                    items: [
                        {
                            name: 'FactCost',
                            fieldLabel: 'Фактическая стоимость'
                        },
                        {
                            name: 'Cost',
                            fieldLabel: 'Стоимость'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        xtype: 'datefield',
                        labelAlign: 'right',
                        labelWidth: 150,
                        format: 'd.m.Y',
                        flex: 1
                    },
                    items: [
                        {
                            name: 'DateStart',
                            fieldLabel: 'Дата начала'
                        },
                        {
                            name: 'DateEnd',
                            fieldLabel: 'Дата окончания'
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'ReasonRejection',
                    fieldLabel: 'Причина отклонения',
                    maxLength: 500,
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
                            //columns: 1,
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            //columns: 1,
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});