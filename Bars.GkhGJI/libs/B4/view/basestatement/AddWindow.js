﻿Ext.define('B4.view.basestatement.AddWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.baseStatementAddWindow',

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.TypeJurPerson',
        'B4.enums.PersonInspection',
        'B4.store.Contragent',
        'B4.form.ComboBox'
    ],

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    minHeight: 180,
    maxHeight: 230,
    bodyPadding: 5,
    itemId: 'baseStatementAddWindow',
    title: 'Проверка по обращению граждан',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right',
                anchor: '100%',
                allowBlank: false
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
                    fieldLabel: 'Тип объекта проверки',
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