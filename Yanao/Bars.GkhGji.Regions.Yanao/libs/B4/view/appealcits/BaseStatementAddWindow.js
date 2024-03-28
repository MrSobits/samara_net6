Ext.define('B4.view.appealcits.BaseStatementAddWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',

        'B4.enums.TypeJurPerson',
        'B4.enums.PersonInspection',
        'B4.enums.FormCheck',

        'B4.store.Contragent',
        'B4.store.RealityObject',
        'B4.view.appealcits.AppealCitsGrid'
    ],

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start'
    },
    width: 500,
    height: 500,
    bodyPadding: 5,
    itemId: 'baseStatementAppCitsAddWindow',
    title: 'Проверка по обращению',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right',
                anchor: '100%',
                allowBlank: false
            },
            items: [
                 {
                     xtype: 'panel',
                     layout: 'form',
                     split: false,
                     collapsible: false,
                     bodyStyle: Gkh.bodyStyle,
                     margins: '0 0 5px 0',
                     border: false,
                     defaults: {
                         labelWidth: 150,
                         anchor: '100%',
                         labelAlign: 'right',
                         allowBlank: false
                     },
                     items: [
                     {
                         xtype: 'combobox',
                         name: 'PersonInspection',
                         fieldLabel: 'Объект проверки',
                         displayField: 'Display',
                         store: B4.enums.PersonInspection.getStore(),
                         valueField: 'Value',
                         itemId: 'cbPersonInspection',
                         editable: false
                     },
                    {
                        xtype: 'combobox',
                        name: 'TypeJurPerson',
                        fieldLabel: 'Тип контрагента',
                        displayField: 'Display',
                        store: B4.enums.TypeJurPerson.getStore(),
                        valueField: 'Value',
                        itemId: 'cbTypeJurPerson',
                        editable: false
                    },
                    {
                        xtype: 'b4selectfield',
                        // name: 'Contragent',
                        itemId: 'sfContragent',
                        fieldLabel: 'Юридическое лицо',
                        store: 'B4.store.Contragent',
                        //listView: 'B4.view.contragent.Grid',
                        columns: [
                            { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
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
                            { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                        ],
                        editable: false
                    },
                    {
                        xtype: 'textfield',
                        name: 'PhysicalPerson',
                        fieldLabel: 'Физическое лицо',
                        itemId: 'tfPhysicalPerson',
                        maxLength: 300,
                        disabled: true
                    },
                    {
                        xtype: 'combobox',
                        name: 'TypeForm',
                        itemId: 'cbFormCheck',
                        store: B4.enums.FormCheck.getStore(),
                        displayField: 'Display',
                        valueField: 'Value',
                        fieldLabel: 'Форма проверки',
                        editable: false
                    },
                    {
                        itemId: 'sfRealityObject',
                        xtype: 'b4selectfield',
                        listView: 'B4.view.realityobj.Grid',
                        store: 'B4.store.RealityObject',
                        textProperty: 'Address',
                        width: 500,
                        hidden: true,
                        allowBlank: true,
                        columns: [
                                {
                                    text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
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
                                { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
                        ],
                        fieldLabel: 'Адрес'
                    }
                     ]
                 },
                {
                    xtype: 'appealCitsBaseStatGrid',
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