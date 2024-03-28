Ext.define('B4.view.appealcits.ExecutantEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    minWidth: 520,
    minHeight: 310,
    height: 310,
    bodyPadding: 5,
    itemId: 'appealCitsExecutantEditWindow',
    title: 'Форма редактирования',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    margin: '0 0 5 0',
                    layout: { type: 'hbox', align: 'stretch' },
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Author',
                            itemId: 'sflAuthor',
                            allowBlank: false,
                            fieldLabel: 'Ответственный руководитель',
                            flex: 1,
                            textProperty: 'Fio',
                            store: 'B4.store.dict.Inspector',
                            columns: [
                                { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Должность', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            listeners: {
                                beforeload: function (cmp, options) {
                                    options.params.headOnly = true;
                                }
                            }
                        },
                        {
                            xtype: 'datefield',
                            name: 'OrderDate',
                            allowBlank: false,
                            width: 250,                           
                            fieldLabel: 'Дата поручения',
                            format: 'd.m.Y',
                            labelWidth: 150
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: { type: 'hbox', align: 'stretch' },
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Executant',
                            itemId: 'sflExecutant',
                            allowBlank: false,
                            fieldLabel: 'Исполнитель',
                            flex: 1,
                            textProperty: 'Fio',
                            store: 'B4.store.dict.Inspector',
                            columns: [
                                { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Должность', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'datefield',
                            name: 'PerformanceDate',
                            allowBlank: false,
                            width: 250,
                            fieldLabel: 'Срок исполнения',
                            format: 'd.m.Y',
                            labelWidth: 150
                        }
                    ]
                },
                {
                    xtype: 'checkbox',
                    name: 'IsResponsible',
                    fieldLabel: 'Ответственный'
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Резолюция',
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-application-go',
                                    itemId: 'btnRedirect',
                                    text: 'Перенаправить'
                                },
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
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
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