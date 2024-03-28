Ext.define('B4.view.warninginspection.AddWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.warninginspectionaddwindow',

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.Contragent',
        'B4.form.ComboBox',
        'B4.store.dict.BaseDict',
        'B4.store.appealcits.ForSelect',
        'B4.enums.TypeBaseDispHead',
        'B4.enums.TypeFormInspection',
        'B4.enums.TypeJurPerson',
        'B4.enums.PersonInspection',
        'B4.enums.InspectionCreationBasis'
    ],

    mixins: ['B4.mixins.window.ModalMask'],

    layout: 'form',
    width: 470,
    bodyPadding: 5,
    resizable: true,

    title: 'Предостережение',

    initComponent: function() {
        var me = this,
            today = new Date();

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right',
                anchor: '100%',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'InspectionBasis',
                    store: B4.enums.InspectionCreationBasis.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    fieldLabel: 'Основание предостережения',
                    editable: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'AppealCits',
                    fieldLabel: 'Обращение гражданина',
                    textProperty: 'Name',
                    store: 'B4.store.appealcits.ForSelect',
                    columns: [
                        { text: 'Номер', dataIndex: 'Number', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Номер ГЖИ', dataIndex: 'NumberGji', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Дата обращения', dataIndex: 'DateFrom', flex: 1, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                        { text: 'Управляющая организация', dataIndex: 'ManagingOrganization', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Количество вопросов', dataIndex: 'QuestionsCount', flex: 1, filter: { xtype: 'textfield', operand: CondExpr.operands.eq } }
                    ],
                    editable: false,
                    hidden: true
                },
                {
                    xtype: 'datefield',
                    name: 'Date',
                    fieldLabel: 'Дата',
                    maxValue: today,
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4combobox',
                    name: 'PersonInspection',
                    fieldLabel: 'Объект проверки',
                    displayField: 'Display',
                    editable: false,
                    storeAutoLoad: true,
                    valueField: 'Id',
                    url: '/Inspection/ListPersonInspection'
                },
                {
                    xtype: 'b4combobox',
                    name: 'TypeJurPerson',
                    fieldLabel: 'Тип контрагента',
                    displayField: 'Display',
                    valueField: 'Id',
                    editable: false,
                    storeAutoLoad: true,
                    url: '/Inspection/ListJurPersonTypes'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Contragent',
                    textProperty: 'ShortName',
                    fieldLabel: 'Юридическое лицо',
                    store: 'B4.store.Contragent',
                    columns: [
                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                        {
                            text: 'Муниципальный район',
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
                                url: '/Municipality/ListMoAreaWithoutPaging'
                            }
                        },
                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    editable: false
                },
                {
                    xtype: 'textfield',
                    name: 'PhysicalPerson',
                    fieldLabel: 'ФИО'
                },
                {
                    xtype: 'textfield',
                    name: 'Inn',
                    fieldLabel: 'ИНН',
                    maxLength: 12,
                    allowBlank: true
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});