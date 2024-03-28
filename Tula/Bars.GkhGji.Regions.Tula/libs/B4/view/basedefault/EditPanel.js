//ToDo данный js перекрыт в связи с тем что понадобилось в ННовгород добавить для всех сонований поле Ликвидацию ЮЛ в котором
//ToDo при change поля Контрагент срабатывает получение ликвидации и вывода информации 
Ext.define('B4.view.basedefault.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'baseDefaultEditPanel',
    title: 'Проверка без основания',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.view.contragent.Grid',
        'B4.view.basedefault.RealityObjectGrid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    bodyPadding: 5,
                    height: 180,
                    autoScroll: true,
                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                    layout: { type: 'vbox', align: 'stretch' },
                    split: false,
                    collapsible: false,
                    border: false,
                    defaults: {
                        labelWidth: 180,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'container',
                            flex: 1,
                            defaults: {
                                labelWidth: 180,
                                labelAlign: 'right'
                            },
                            layout: {
                                pack: 'start',
                                type: 'hbox'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'InspectionNumber',
                                    itemId: 'tfInspectionNumber',
                                    fieldLabel: 'Номер проверки',
                                    maxLength: 20
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'InspectionYear',
                                    itemId: 'tfInspectionYear',
                                    fieldLabel: 'Год проверки',
                                    labelWidth: 120,
                                    hideTrigger: true
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1
                            },
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'Contragent',
                                    textProperty: 'ShortName',
                                    fieldLabel: 'Юридическое лицо',
                                    editable: false,
                                    eadOnly: true,
                                    itemId: 'sfContragent',
                                    store: 'B4.store.Contragent',
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                        {
                                            text: 'Муниципальное образование',
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
                                                url: '/Municipality/ListWithoutPaging'
                                            }
                                        },
                                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ActivityInfo',
                                    fieldLabel: 'Ликвидация ЮЛ',
                                    readOnly: true
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'PhysicalPerson',
                            itemId: 'tfPhysPerson',
                            fieldLabel: 'Физическое лицо',
                            maxLength: 300
                        },
                        {
                            xtype: 'textarea',
                            name: 'PhysicalPersonInfo',
                            itemId: 'taPhysPersonInfo',
                            fieldLabel: 'Реквизиты физического лица',
                            maxLength: 500
                        }
                    ]
                },
                {
                    xtype: 'baseDefaultRealityObjectGrid',
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
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Отменить',
                                    textAlign: 'left',
                                    itemId: 'btnCancel'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
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