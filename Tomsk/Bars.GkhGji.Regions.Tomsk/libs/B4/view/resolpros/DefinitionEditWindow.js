Ext.define('B4.view.resolpros.DefinitionEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.resolprosdefwin',
    itemId: 'resolutionDefinitionEditWindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    bodyPadding: 5,

    title: 'Определение',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.TypeResolProsDefinition',
        'B4.store.dict.Inspector',
        'B4.form.ComboBox',
        'B4.view.Control.GkhButtonPrint'
    ],

    initComponent: function() {
        var me = this,
            hourStore = [7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22],
            minuteStore = ["00", "15", "30", "45"];


        Ext.applyIf(me, {
            defaults: {
                labelWidth: 240,
                labelAlign: 'right'
            },
            items: [
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
                    xtype: 'datefield',
                    name: 'DocumentDate',
                    width: 120,
                    fieldLabel: 'Дата определения',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'ExecutionDate',
                            fieldLabel: 'Рассмотрение назначено на дату',
                            format: 'd.m.Y',
                            allowBlank: false,
                            labelWidth: 240,
                            minWidth: 350
                        },
                        {
                            xtype: 'timefield',
                            name: 'ExecutionTime',
                            valueField: 'time',
                            format: 'H:i', 
                            fieldLabel: 'время',
                            minValue: '07:00',
                            maxValue: '22:00',
                            labelWidth: 50,
                            flex: 1,
                            editable: false
                        }
                    ]
                },
                {
                    xtype: 'datefield',
                    name: 'DateSubmissionDocument',
                    fieldLabel: 'Срок предоставления документов',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'textfield',
                    name: 'ResolutionInitAdminViolation',
                    fieldLabel: 'Постановление о возбуждении дела об административном правонарушении'
                },
                {
                    xtype: 'textfield',
                    name: 'ReturnReason',
                    fieldLabel: 'Доводы в обоснование возврата'
                },
                {
                    xtype: 'textfield',
                    name: 'AdditionalDocuments',
                    fieldLabel: 'Дополнительные документы'
                },
                {
                    xtype: 'textfield',
                    name: 'RequestNeed',
                    fieldLabel: 'Что необходимо запросить'
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