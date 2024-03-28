Ext.define('B4.view.smevpropertytype.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.smevpropertytype.FileInfoGrid',
        'B4.view.Control.GkhButtonPrint',
        'B4.form.EnumCombo',
        'B4.enums.PublicPropertyLevel',
        'B4.enums.InnOgrn'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'smevpropertytypeEditWindow',
    title: 'Запрос',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
             items: [
             {
                 xtype: 'tabpanel',
                 border: false,
                 flex: 1,
                 defaults: {
                     border: false
                 },
                 items: [
                     {
                         layout: {
                             type: 'vbox',
                             align: 'stretch'
                         },
                         defaults: {
                             labelWidth: 100,
                             margin: '5 0 5 0',
                             align: 'stretch',
                             labelAlign: 'right'
                         },
                         bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                         title: 'Форма запроса',
                         border: false,
                         bodyPadding: 10,
                         items: [
                         {
                            xtype: 'fieldset',
                            defaults: {
                                labelWidth: 250,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            title: 'Реквизиты субъекта запроса',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        //     margin: '10 0 5 0',
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            name: 'PublicPropertyLevel',
                                            labelWidth: 160,
                                            fieldLabel: 'Тип собственности',
                                            enumName: 'B4.enums.PublicPropertyLevel',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'CadastralNumber',
                                            fieldLabel: 'Кадастровый номер',
                                            allowBlank: true,
                                            flex:1,                                       
                                            itemId: 'tfCadastralNumber'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    padding: '5 0 0 0',
                                    defaults: {
                                        labelWidth: 80,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'RealityObject',
                                            fieldLabel: 'Жилой дом',
                                            textProperty: 'Address',
                                            store: 'B4.store.RealityObject',
                                            editable: false,
                                            flex: 1,
                                            itemId: 'sfRealityObject',
                                            allowBlank: false,
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
                                                {
                                                    text: 'Адрес',
                                                    dataIndex: 'Address',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'Room',
                                            fieldLabel: 'Помещение',
                                            textProperty: 'RoomNum',
                                            store: 'B4.store.cscalculation.RoomList',
                                            editable: false,
                                            flex: 1,
                                            itemId: 'sfRoom',
                                            allowBlank: true,
                                            disabled: true,
                                            columns: [
                                                {
                                                    text: 'Номер помещения',
                                                    dataIndex: 'RoomNum',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                },
                                                {
                                                    text: 'Кадастровый номер',
                                                    dataIndex: 'CadastralNumber',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                }
                                            ]
                                        }
                                    ]
                                }
                                   
                             ]
                            },                           
                            {
                                xtype: 'tabpanel',
                                border: false,
                                flex: 1,
                                defaults: {
                                    border: false
                                },
                                items: [
                                {
                                        xtype: 'smevpropertytypefileinfogrid',
                                    flex: 1
                                }
                                ]
                            }
                         ]
                     }                    
                 ]
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