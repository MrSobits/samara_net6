Ext.define('B4.view.appealcits.DefinitionEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    minHeight: 300,
    bodyPadding: 5,
    itemId: 'appealcitsDefinitionEditWindow',
    title: 'Форма редактирования определения',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.store.dict.Inspector',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.view.Control.GkhButtonPrint',
        'B4.form.ComboBox',
        'B4.enums.TypeDefinitionResolution'
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
                        labelWidth: 180,
                        labelAlign: 'right',
                        allowBlank: false,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNum',
                            fieldLabel: 'Номер',
                            maxLength: 50
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
                    url: '/ResolutionDefinition/ListTypeDefinition'
                },
                {
                    xtype: 'b4filefield',
                    name: 'FileInfo',
                    fieldLabel: 'Файл'
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.Inspector',
                    textProperty: 'Fio',
                    name: 'IssuedDefinition',
                    fieldLabel: 'ДЛ, вынесшее определение',
                    columns: [
                        { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'ExecutionDate',
                    fieldLabel: 'Дата исполнения',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'textarea',
                    fieldLabel: 'Установил',
                    flex: 1,
                    width: 350,
                    name: 'Established'
                },
                {
                    xtype: 'textarea',
                    fieldLabel: 'Решил',
                    flex: 1,
                    width: 350,
                    name: 'Decided'
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
                        { xtype: 'tbfill' },
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