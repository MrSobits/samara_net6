Ext.define('B4.view.competition.LotBidEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 700,
    minWidth: 700,
    minHeight: 220,
    maxHeight: 220,
    bodyPadding: 5,
    alias: 'widget.competitionlotbideditwindow',
    title: 'Протокол',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.Builder'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Builder',
                    textProperty: 'ContragentName',
                    fieldLabel: 'Участник',
                    flex: 1,
                    store: 'B4.store.Builder',
                    columns: [
                        { text: 'Наименование', dataIndex: 'ContragentName', filter: 'textfield', flex: 1 },
                        { text: 'ИНН', dataIndex: 'Inn', filter: 'textfield', flex: 1 },
                        { text: 'КПП', dataIndex: 'Kpp', filter: 'textfield', flex: 1 },
                        { text: 'ОГРН', dataIndex: 'Ogrn', filter: 'textfield', flex: 1 }
                    ],
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    anchor: '100%',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'IncomeDate',
                            fieldLabel: 'Дата поступления',
                            format: 'd.m.Y',
                            allowBlank: false
                        },
                        {
                            xtype: 'numberfield',
                            name: 'Points',
                            fieldLabel: 'Количество баллов',
                            hideTrigger: true,
                            allowDecimals: true,
                            minValue: 0,
                            negativeText: 'Значение не может быть отрицательным'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    anchor: '100%',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'Price',
                            fieldLabel: 'Цена заявки, руб. (без НДС) ',
                            hideTrigger: true,
                            allowDecimals: true,
                            minValue: 0,
                            negativeText: 'Значение не может быть отрицательным'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'PriceNds',
                            fieldLabel: 'Цена заявки, руб. (с НДС)',
                            hideTrigger: true,
                            allowDecimals: true,
                            minValue: 0,
                            negativeText: 'Значение не может быть отрицательным'
                        }
                    ]
                },
                {
                    xtype: 'checkbox',
                    name: 'IsWinner',
                    fieldLabel: 'Является победителем'
                }
                
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4savebutton' }]
                        }, '->', {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});