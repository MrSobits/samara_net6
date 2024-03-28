Ext.define('B4.view.repairobject.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    itemId: 'repairObjEditPanel',
    title: 'Паспорт объекта',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.store.RealityObject',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me,
            {
                items: [
                    {
                        xtype: 'fieldset',
                        defaults: {
                            labelWidth: 230,
                            anchor: '100%',
                            labelAlign: 'right'
                        },
                        title: 'Общие сведения',
                        items: [
                            {
                                xtype: 'b4selectfield',
                                name: 'RealityObject',
                                itemId: 'sfRealityObject',
                                textProperty: 'Address',
                                fieldLabel: 'Объект недвижимости',
                                flex: 1,
                                store: 'B4.store.RealityObject',
                                columns: [
                                    {
                                        text: 'Муниципальное образование',
                                        dataIndex: 'Municipality',
                                        flex: 1,
                                        filter: {
                                            xtype: 'b4combobox',
                                            operand: CondExpr.operands.eq,
                                            storeAutoLoad: false,
                                            hideLabel: true,
                                            editable: false,
                                            valueField: 'Name',
                                            emptyItem: { Name: '-' },
                                            url: '/Municipality/ListWithoutPaging'
                                        }
                                    },
                                    {
                                        text: 'Адрес',
                                        dataIndex: 'Address',
                                        flex: 1,
                                        filter: { xtype: 'textfield' }
                                    }
                                ],
                                editable: false,
                                allowBlank: false
                            },
                            {
                                xtype: 'b4selectfield',
                                name: 'RepairProgram',
                                itemId: 'sfRepairProgram',
                                textProperty: 'Name',
                                fieldLabel: 'Программа',
                                flex: 1,
                                store: 'B4.store.dict.RepairProgram',
                                columns: [
                                    { text: 'Программа текущего ремонта', dataIndex: 'Name', flex: 1 }
                                ],
                                editable: false,
                                allowBlank: false
                            },
                            {
                                xtype: 'b4filefield',
                                name: 'ReasonDocument',
                                itemId: 'documentfield',
                                fieldLabel: 'Документ-основание для проведения работ'
                            },
                            {
                                xtype: 'textfield',
                                name: 'Comment',
                                itemId: 'commentfield',
                                fieldLabel: 'Примечание'
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
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
                                }
                            ]
                        }
                    ]
                }]
        });

        me.callParent(arguments);
    }
});