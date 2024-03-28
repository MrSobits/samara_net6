Ext.define('B4.view.service.housing.ProviderServiceEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    width: 600,
    bodyPadding: 5,
    closeAction: 'hide',
    trackResetOnLoad: true,
    title: 'Новый поставщик',
    itemId: 'housingProviderServiceEditWindow',
    layout: 'anchor',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.store.service.ContragentForProvider',
        'B4.form.SelectField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                anchor: '100%',
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 5px 5px 10px; padding: 5px 10px; line-height: 16px; width: 590px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">На форме отображается поставщик, дата заключения договора с которым самая поздняя. Посмотреть всех поставщиков можно во вкладке "Поставщики"</span>'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Provider',
                    itemId: 'sflProvider',
                    fieldLabel: 'Поставщик',
                   

                    store: 'B4.store.service.ContragentForProvider',
                    allowBlank: false,
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
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'NumberContract',
                    flex: 2,
                    fieldLabel: 'Номер договора',
                    itemId: 'tfNumberContract',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'DateStartContract',
                    allowBlank: false,
                    itemId: 'dfDateStartContract',
                    fieldLabel: 'Дата заключения договора',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'textareafield',
                    name: 'Description',
                    padding: '10 0 0 0',
                    fieldLabel: 'Примечание'
                }
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
