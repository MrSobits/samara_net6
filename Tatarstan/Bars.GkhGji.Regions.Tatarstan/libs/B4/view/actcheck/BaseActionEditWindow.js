Ext.define('B4.view.actcheck.BaseActionEditWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.form.FiasSelectAddress',
        'B4.form.SelectField',
        'B4.view.Control.GkhButtonPrint',
        'B4.enums.ActCheckActionType'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    width: 1000,
    modal: true,
    autoScroll: true,
    
    // Используется для добавления
    // дополнительных компонентов
    editFormItems: null,

    // Использовалось для организации
    // работы базовых элементов
    itemId: 'actCheckActionAddWindow',

    initComponent: function () {
        var me = this,
            items = [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 110,
                        padding: '0 5 5 5',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'Date',
                            fieldLabel: 'Дата документа',
                            format: 'd.m.Y',
                            labelWidth: 100,
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Номер документа',
                            name: 'Number',
                            maxLength: 50,
                            allowBlank: false
                        },
                        {
                            xtype: 'b4fiasselectaddress',
                            name: 'CreationPlace',
                            fieldLabel: 'Место составления',
                            flatIsVisible: false,
                            allowBlank: false,
                            flex: 1.75,
                            fieldsRegex: {
                                tfHousing: {
                                    regex: /^\d+$/,
                                    regexText: 'В это поле можно вводить только цифры'
                                },
                                tfBuilding: {
                                    regex: /^\d+$/,
                                    regexText: 'В это поле можно вводить только цифры'
                                }
                            }
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        padding: '0 5 5 5'
                    },
                    items: [
                        {
                            xtype: 'b4enumcombo',
                            name: 'ActionType',
                            fieldLabel: 'Вид действия',
                            enumName: 'B4.enums.ActCheckActionType',
                            readOnly: true,
                            editable: false,
                            labelWidth: 100,
                            flex: 2
                        },
                        {
                            xtype: 'component',
                            flex: 1.75
                        }
                    ]
                }
            ];

        Array.prototype.push.apply(items, me.editFormItems);
        
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    margin: 5,
                    items: items
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'b4closebutton',
                                    text: 'Отмена'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    setDisabledItems: function (mainSaveButton, allowed) {
        var me = this,
            addButtonArray = me.query('b4addbutton'),
            saveButtonArray = me.query('#saveButton'),
            editColumnArray = me.query('b4editcolumn'),
            deleteColumnArray = me.query('b4deletecolumn');

        mainSaveButton.setDisabled(!allowed);

        addButtonArray.forEach(function (addButton) {
            addButton.setDisabled(!allowed);
        });

        saveButtonArray.forEach(function (saveButton) {
            saveButton.setDisabled(!allowed);
        });

        editColumnArray.forEach(function (editColumn) {
            allowed ? editColumn.show() : editColumn.hide();
        });

        deleteColumnArray.forEach(function (deleteColumn) {
            allowed ? deleteColumn.show() : deleteColumn.hide();
        });
    }
});