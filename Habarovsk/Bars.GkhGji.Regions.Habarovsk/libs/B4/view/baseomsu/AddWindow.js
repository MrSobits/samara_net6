Ext.define('B4.view.baseomsu.AddWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 600,
    bodyPadding: 5,
    itemId: 'baseOMSUAddWindow',
    title: 'Плановая проверка органа местного самоуправления',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.PlanJurPersonGji',
        'B4.store.Contragent',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox',
        
        'B4.enums.TypeJurPerson'
    ],

    initComponent: function () {
        var me = this,
            currTypeJurPerson = B4.enums.TypeJurPerson.getItems(),
            newTypeJurPerson = [];
        
        Ext.iterate(currTypeJurPerson, function (val, key) {
            if (val[0] == 30)
                newTypeJurPerson.push(val);
        });
        
        Ext.applyIf(me, {
            defaults: {
                labelWidth: 140,
                allowBlank: false,
                anchor: '100%',
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'TypeJurPerson',
                    editable: false,
                    fieldLabel: 'Тип юридического лица',
                    displayField: 'Display',
                    items: newTypeJurPerson,
                    valueField: 'Value',
                    itemId: 'cbTypeJurPerson',
                    value: newTypeJurPerson[0]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Contragent',
                    textProperty: 'ShortName',
                    itemId: 'sfContragent',
                    fieldLabel: 'Юридическое лицо',
                    store: 'B4.store.Contragent',
                    editable: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Муниципальный район', dataIndex: 'Municipality', flex: 1,
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
                    name: 'OmsuPerson',
                    fieldLabel: 'Должностное лицо ОМСУ',
                    allowBlank: true,
                    editable: true,
                    readOnly: false
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.PlanJurPersonGji',
                    name: 'Plan',
                    fieldLabel: 'План',
                    editable: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                    ]
                },
                {
                    xtype: 'datefield',
                    name: 'DateStart',
                    fieldLabel: 'Дата начала проверки',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'JurPersonInspectors',
                    itemId: 'trigfInspectors',
                    fieldLabel: 'Инспекторы',
                    allowBlank: true
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'JurPersonZonalInspections',
                    itemId: 'trigfZonalInspections',
                    fieldLabel: 'Отделы',
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