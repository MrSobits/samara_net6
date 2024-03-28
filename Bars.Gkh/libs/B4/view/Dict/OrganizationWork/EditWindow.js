Ext.define('B4.view.dict.OrganizationWork.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 560,
    minHeight: 350,
    bodyPadding: 5,
    itemId: 'organizationWorkEditWindow',
    title: 'Форма редактирования работы',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.UnitMeasure',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.dict.unitmeasure.Grid',
        'B4.enums.TypeWork',
        'B4.enums.WorkAssignment',
        'B4.view.dict.OrganizationWork.ContentRepairMkdWorkGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 190,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    padding: 0,
                    frame: true,
                    items: [
                        {
                            xtype: 'container',
                            title: 'Основные параметры',
                            flex: 1,
                            padding: 5,
                            layout: { type: 'vbox', align: 'stretch' },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 100
                            },
                            items:[
                                {
                                    xtype: 'textfield',
                                    name: 'Name',
                                    fieldLabel: 'Наименование',
                                    allowBlank: false,
                                    maxLength: 300
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'UnitMeasure',
                                    fieldLabel: 'Ед. измерения',
                                    store: 'B4.store.dict.UnitMeasure',
                                    editable: false,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Code',
                                    fieldLabel: 'Код',
                                    allowBlank: false,
                                    maxLength: 10
                                },
                                {
                                    xtype: 'combobox', editable: false,
                                    fieldLabel: 'Назначение работ',
                                    store: B4.enums.WorkAssignment.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    name: 'WorkAssignment'
                                },
                                {
                                    xtype: 'combobox', editable: false,
                                    fieldLabel: 'Тип работ',
                                    store: B4.enums.TypeWork.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    name: 'TypeWork'
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'Description',
                                    fieldLabel: 'Описание',
                                    flex: 1,
                                    maxLength: 500
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            title: 'Работы по содержанию и ремонту МКД',
                            flex: 1,
                            layout:
                            {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'contentrepairmkdworkgrid',
                                    flex: 1,
                                    title: ''
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
