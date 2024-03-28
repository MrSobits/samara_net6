Ext.define('B4.view.admincase.DocEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.admincasedoceditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    bodyPadding: 5,
    itemId: 'admincasedoceditwindow',
    title: 'Документ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.ComboBox',
        'B4.view.Control.GkhButtonPrint',
        'B4.enums.TypeAdminCaseDoc',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 125
            },
            items: [
                {
                    xtype: 'b4combobox',
                    store: B4.enums.TypeAdminCaseDoc.getStore(),
                    name: 'TypeAdminCaseDoc',
                    fieldLabel: 'Тип документа',
                    displayField: 'Display',
                    valueField: 'Value',
                    editable: false
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 125,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Номер документа',
                            name: 'DocumentNumber',
                            readOnly: true
                        },
                        {
                            xtype: 'datefield',
                            format: 'd.m.Y',
                            name: 'DocumentDate',
                            fieldLabel: 'Дата документа',
                            allowBlank: false
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'EntitiedInspector',
                    fieldLabel: 'ДЛ, рассмотревшее ходатайство',
                    store: 'B4.store.dict.Inspector',
                    textProperty: 'Fio',
                    allowBlank: false,
                    columns: [
                        { dataIndex: 'Fio', text: 'ФИО', flex: 1, filter: {xtype: 'textfield'} }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        labelAlign: 'right',
                        labelWidth: 125,
                        allowBlank: false,
                        flex: 1
                    },
                    items: [
                        {
                            name: 'NeedTerm',
                            fieldLabel: 'Необходимый срок'
                        },
                        {
                            name: 'RenewalTerm',
                            fieldLabel: 'Срок продлен до'
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'DescriptionSet',
                    fieldLabel: 'Установил',
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
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
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