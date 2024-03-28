Ext.define('B4.view.appealcits.BaseStatementAddWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
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
    width: 1000,
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
                         xtype: 'b4combobox',
                         name: 'PersonInspection',
                         fieldLabel: 'Объект проверки',
                         displayField: 'Display',
                         itemId: 'cbPersonInspection',
                         editable: false,
                         storeAutoLoad: true,
                         valueField: 'Id',
                         url: '/Inspection/ListPersonInspection'
                     },
                    {
                        xtype: 'b4combobox',
                        name: 'TypeJurPerson',
                        fieldLabel: 'Тип объекта проверки',
                        displayField: 'Display',
                        valueField: 'Id',
                        itemId: 'cbTypeJurPerson',
                        editable: false,
                        storeAutoLoad: true,
                        url: '/Inspection/ListJurPersonTypes'
                    },
                    {
                        xtype: 'b4selectfield',
                        // name: 'Contragent',
                        itemId: 'sfContragent',
                        textProperty: 'ShortName',
                        fieldLabel: 'Юридическое лицо',
                        store: 'B4.store.Contragent',
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
                        name: 'Address',
                        xtype: 'b4selectfield',
                        store: 'B4.store.RealityObject',
                        textProperty: 'Address',
                        width: 500,
                        editable: false,
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
                    xtype: 'container',
                    itemId: 'containerInfo',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Для выбора адреса проверяемого дома необходимо, чтобы был заполнен адрес во вкладке "Место возникновения проблемы" </span>'
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