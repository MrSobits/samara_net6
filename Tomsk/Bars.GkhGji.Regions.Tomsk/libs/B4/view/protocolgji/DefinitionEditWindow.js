Ext.define('B4.view.protocolgji.DefinitionEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {type: 'vbox', align: 'stretch'},
    width: 600,
    bodyPadding: 5,
    itemId: 'protocolgjiDefinitionEditWindow',
    title: 'Форма определения',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.store.dict.Inspector',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhButtonPrint',
        'B4.form.ComboBox',
        'B4.enums.TypeDefinitionProtocol'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 180,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        allowBlank: false,
                        flex: 1,
                        labelWidth: 180
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNum',
                            fieldLabel: 'Номер',
                            maxLength: 300
                        },
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            fieldLabel: 'Дата',
                            format: 'd.m.Y'
                        }
                    ]
                },
                {
                    xtype: 'b4combobox',
                    operand: CondExpr.operands.eq,
                    editable: false,
                    floating: false,
                    valueField: 'Id',
                    displayField: 'Display',
                    name: 'TypeDefinition',
                    fieldLabel: 'Тип определения',
                    url: '/ProtocolDefinition/ListTypeDefinition'
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 180,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'DateOfProceedings',
                            fieldLabel: 'Дата расмотрения дела:',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'timefield',
                            fieldLabel: 'время',
                            name: 'TimeDefinition',
                            format: 'H:i',
                            submitFormat: 'Y-m-d H:i:s',
                            minValue: '8:00',
                            maxValue: '22:00'
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.Inspector',
                    textProperty: 'Fio',
                    name: 'IssuedDefinition',
                    fieldLabel: 'ДЛ, вынесшее определение',
                    editable: false,
                    columns: [
                        { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'ExecutionDate',
                    fieldLabel: 'Дата исполнения',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'textfield',
                    name: 'PlaceReview',
                    fieldLabel: 'Место рассмотрения',
                    maxLength: 2000,
                    flex: 1
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    maxLength: 2000,
                    flex: 1,
                    hidden: true
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