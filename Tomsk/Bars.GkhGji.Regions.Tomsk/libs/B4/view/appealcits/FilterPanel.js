Ext.define('B4.view.appealcits.FilterPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.appealcitsFilterPanel',

    closable: false,
    header: false,
    layout: 'hbox',
    bodyPadding: 5,
    itemId: 'appealcitsFilterPanel',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.ux.button.Update',
        'B4.form.SelectField',
        'B4.view.realityobj.Grid',
        'B4.store.dict.Inspector',
        'B4.store.RealityObject'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 130,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    border: false,
                    width: 500,
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'container',
                            border: false,
                            width: 500,
                            padding: '5 0 0 0',
                            layout: 'hbox',
                            defaults: {
                                format: 'd.m.Y',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    labelWidth: 125,
                                    fieldLabel: 'Дата обращения с',
                                    width: 290,
                                    itemId: 'dfDateFromStart'
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 50,
                                    fieldLabel: 'по',
                                    width: 210,
                                    itemId: 'dfDateFromEnd'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            border: false,
                            width: 500,
                            padding: '5 0 0 0',
                            layout: 'hbox',
                            defaults: {
                                format: 'd.m.Y',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    labelWidth: 125,
                                    fieldLabel: 'Контрольный срок с',
                                    width: 290,
                                    itemId: 'dfCheckTimeStart'
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 50,
                                    fieldLabel: 'по',
                                    width: 210,
                                    itemId: 'dfCheckTimeEnd'
                                }
                            ]
                        },
                        {
                            itemId: 'sfRealityObject',
                            xtype: 'b4selectfield',
                            store: 'B4.store.RealityObject',
                            textProperty: 'Address',
                            width: 500,
                            editable: false,
                            labelWidth: 125,
                            padding: '5 0 0 0',
                            columns: [
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
                                { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            fieldLabel: 'Жилой дом'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    border: false,
                    width: 650,
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            padding: '5 0 0 0',
                            itemId: 'sfAuthor',
                            labelWidth: 125,
                            width: 500,
                            editable: false,
                            fieldLabel: 'Поручитель',
                            textProperty: 'Fio',
                            store: 'B4.store.dict.Inspector',
                            columns: [
                                { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Должность', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            padding: '5 0 0 0',
                            itemId: 'sfExecutant',
                            labelWidth: 125,
                            width: 500,
                            editable: false,
                            fieldLabel: 'Исполнитель',
                            textProperty: 'Fio',
                            store: 'B4.store.dict.Inspector',
                            columns: [
                                { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Должность', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'container',
                            border: false,
                            width: 650,
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    itemId: 'sfController',
                                    labelWidth: 125,
                                    width: 500,
                                    editable: false,
                                    fieldLabel: 'Проверяющий',
                                    textProperty: 'Fio',
                                    store: 'B4.store.dict.Inspector',
                                    columns: [
                                        { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'Должность', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    width: 10,
                                    xtype: 'component'
                                },
                                {
                                    width: 100,
                                    itemId: 'updateGrid',
                                    xtype: 'b4updatebutton'
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