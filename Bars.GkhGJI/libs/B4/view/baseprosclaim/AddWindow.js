Ext.define('B4.view.baseprosclaim.AddWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    minHeight: 253,
    maxHeight: 253,
    itemId: 'baseProsClaimAddWindow',
    title: 'Проверка по требованию прокуратуры',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.enums.TypeJurPerson',
        'B4.enums.PersonInspection',
        'B4.form.ComboBox'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                allowBlank: false,
                anchor: '100%',
                labelAlign: 'right',
                labelWidth: 170
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
                    fieldLabel: 'Тип контрагента',
                    displayField: 'Display',
                    valueField: 'Id',
                    itemId: 'cbTypeJurPerson',
                    editable: false,
                    storeAutoLoad: true,
                    url: '/Inspection/ListJurPersonTypes'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Contragent',
                    textProperty: 'ShortName',
                    itemId: 'sfContragent',
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
                    fieldLabel: 'ФИО',
                    itemId: 'tfPhysicalPerson',
                    maxLength: 300,
                    disabled: true
                },
                {
                    xtype: 'datefield',
                    name: 'ProsClaimDate',
                    fieldLabel: 'Дата',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datefield',
                    name: 'ProsClaimDateCheck',
                    fieldLabel: 'Дата проверки',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentNumber',
                    fieldLabel: 'Номер документа',
                    maxLength: 50
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