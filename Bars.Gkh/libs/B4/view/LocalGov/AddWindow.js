Ext.define('B4.view.localgov.AddWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 600,
    bodyPadding: 5,
    itemId: 'localGovAddWindow',
    title: 'Орган местного самоуправления',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.Contragent',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 190,
                anchor: '100%',
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    defaults: {
                        labelWidth: 190,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items: [{
                        xtype: 'b4selectfield',
                        width: 565,
                        name: 'Contragent',
                        fieldLabel: 'Контрагент',
                        store: 'B4.store.Contragent',
                        editable: false,
                        allowBlank: false,
                        columns: [
                            { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                            {
                                text: 'Муниципальный район', dataIndex: 'Municipality', flex: 1,
                                filter: {
                                    xtype: 'b4combobox',
                                    operand: CondExpr.operands.eq,
                                    storeAutoLoad: false,
                                    hideLabel: true,
                                    editable: false,
                                    valueField: 'Name',
                                    emptyItem: { Name: '-' },
                                    url: '/Municipality/ListMoAreaWithoutPaging'
                                }
                            },
                            { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                        ]
                    },
                      {
                          xtype: 'textfield',
                          width: 565,
                          name: 'NameDepartamentGkh',
                          fieldLabel: 'Наименование подразделения, ответсвенного за ЖКХ',
                          allowBlank: false,
                          maxLength: 300
                      },
                       {
                           xtype: 'fieldset',
                           defaults: {
                               labelWidth: 180,
                               anchor: '100%',
                               labelAlign: 'right'
                           },
                           title: 'Контакты',
                           items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Phone',
                                    fieldLabel: 'Телефон',
                                    allowBlank: false,
                                    maxLength: 50
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'OfficialSite',
                                    fieldLabel: 'Официальный сайт',
                                    allowBlank: false,
                                    maxLength: 50
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