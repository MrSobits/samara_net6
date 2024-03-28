Ext.define('B4.view.actcheck.ActionAddWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.form.FiasSelectAddress',
        'B4.enums.ActCheckActionType',
        'B4.store.actcheck.Action',
        'B4.view.actaction.ActActionSelectionGrid',
        'B4.form.SelectField'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 550,
    itemId: 'actCheckActionAddWindow',
    title: 'Добавление действия',
    closable: false,
    modal: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right',
                padding: '0 10 0 0'
            },
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'ActionType',
                    padding: '5 10 0 0',
                    fieldLabel: 'Вид действия',
                    url: '/ActCheckAction/GetActionTypes',
                    displayField: 'Display',
                    valueField: 'Value',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'FillBasedOnAnotherAction',
                    store: 'B4.store.actcheck.Action',
                    listView: 'B4.view.actaction.ActActionSelectionGrid',
                    fieldLabel: 'Заполнить на</br>основании действия',
                    hidden: true,
                    updateDisplayedText: function (data) {
                        var text;
                        
                        if(data && data['ActionType'] && data['Date'])
                        {
                            text = B4.enums.ActCheckActionType.displayRenderer(data['ActionType']) + ' ' + new Date(data['Date']).toLocaleDateString();
                        }
                        
                        this.setRawValue(text);
                    }
                },
                {
                    xtype: 'datefield',
                    name: 'Date',
                    fieldLabel: 'Дата',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'b4fiasselectaddress',
                    name: 'CreationPlace',
                    fieldLabel: 'Место составления',
                    flatIsVisible: false,
                    allowBlank: false,
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
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [ { xtype: 'b4savebutton' } ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            items: [ { xtype: 'b4closebutton' } ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});