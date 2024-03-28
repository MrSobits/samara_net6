Ext.define('B4.view.actionisolated.AddWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    bodyPadding: 5,
    alias: 'widget.actionisolatedaddwindow',
    itemId: 'actionisolatedaddwindow',
    title: 'Мероприятие без взаимодействия с контролируемыми лицами',
    trackResetOnLoad: true,

    requires: [
        'B4.form.ComboBox',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.view.Control.GkhIntField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.dict.Municipality',
        'B4.store.dict.PlanActionGji',
        'B4.store.dict.Inspector',
        'B4.store.AppealCits',
        'B4.store.Contragent',
        'B4.store.dict.ControlType',
        'B4.store.dict.ZonalInspection',
        'B4.enums.TypeDocumentGji',
        'B4.enums.TypeObjectAction',
        'B4.enums.TypeBaseAction',
        'B4.enums.TypeJurPersonAction'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.Municipality',
                    name: 'Municipality',
                    fieldLabel: 'Муниципальное образование',
                    textProperty: 'Name',
                    editable: false,
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 }],
                    listeners: {
                        beforeload: function (field, options) {
                            options.params.useAuthFilter = true;
                        }
                    }
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.ZonalInspection',
                    name: 'ZonalInspection',
                    fieldLabel: 'Орган ГЖИ',
                    textProperty: 'ZoneName',
                    editable: false,
                    columns: [{ text: 'Наименование', dataIndex: 'ZoneName', flex: 1 }],
                    listeners: {
                        beforeload: function (field, options) {
                            options.params.useAuthFilter = true;
                        }
                    }
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'TypeBase',
                    fieldLabel: 'Основание мероприятия',
                    enumName: 'B4.enums.TypeBaseAction',
                    includeNull: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'AppealCits',
                    fieldLabel: 'Обращение гражданина',
                    textProperty: 'NumberGji',
                    store: 'B4.store.AppealCits',
                    editable: false,
                    allowBlank: true,
                    columns: [
                        { text: 'Номер', dataIndex: 'Number', flex: 1, filter: { xtype: 'textfield' } },
                        { xtype: 'datecolumn', text: 'Дата обращения', dataIndex: 'DateFrom', flex: 1, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                        { text: 'Номер ГЖИ', dataIndex: 'NumberGji', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Управляющая организация', dataIndex: 'ManagingOrganization', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Количество вопросов', dataIndex: 'QuestionsCount', flex: 1, filter: { xtype: 'gkhintfield' } }
                    ],
                    updateDisplayedText: function (data) {
                        var me = this,
                            date = data && data['DateFrom']
                                ? new Date(data['DateFrom'])
                                    .toLocaleDateString()
                                : '',
                            text = data && data['NumberGji']
                                ? data['NumberGji'] +
                                (date ? ' от ' + date : '')
                                : '';

                        me.setRawValue.call(me, text);
                    }
                },
                {
                    xtype: 'b4selectfield',
                    name: 'PlanAction',
                    store: 'B4.store.dict.PlanActionGji',
                    textProperty: 'Name',
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    fieldLabel: 'План',
                    allowBlank: true
                },
                {
                    xtype: 'b4enumcombo',
                    enumName: 'B4.enums.TypeObjectAction',
                    name: 'TypeObject',
                    labelAlign: 'right',
                    fieldLabel: 'Объект мероприятия',
                    includeNull: false
                },
                {
                    xtype: 'b4enumcombo',
                    padding: '0 0 5 0',
                    enumName: 'B4.enums.TypeJurPersonAction',
                    name: 'TypeJurPerson',
                    labelAlign: 'right',
                    fieldLabel: 'Тип контрагента',
                    includeNull: false,
                    allowBlank: true
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    border: false,
                    layout: 'hbox',
                    name: 'PersonInfo',
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'PersonName',
                            fieldLabel: 'ФИО',
                            labelAlign: 'right',
                            labelWidth: 120,
                            flex: 1,
                            allowBlank: true,
                            maxLength: 255
                        },
                        {
                            padding: '0 0 5 0',
                            xtype: 'textfield',
                            name: 'Inn',
                            fieldLabel: 'ИНН',
                            labelWidth: 120,
                            labelAlign: 'right',
                            flex: 1,
                            allowBlank: true,
                            maxLength: 12
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    padding: '0 0 5 0',
                    name: 'Contragent',
                    store: 'B4.store.Contragent',
                    textProperty: 'Name',
                    allowBlank: true,
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'ИНН',
                            dataIndex: 'Inn',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    fieldLabel: 'Контрагент'
                },
                {
                    xtype: 'b4enumcombo',
                    padding: '0 0 5 0',
                    enumName: 'B4.enums.KindAction',
                    name: 'KindAction',
                    fieldLabel: 'Вид мероприятия',
                    includeNull: false
                },
                {
                    xtype: 'datefield',
                    name: 'DocumentDate',
                    fieldLabel: 'Дата',
                    format: 'd.m.Y'
                }

            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
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
                            columns: 1,
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