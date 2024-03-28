Ext.define('B4.view.contragentclw.AddWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.contragentclwaddwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    minHeight: 150,
    maxHeight: 150,
    width: 450,
    minWidth: 450,
    bodyPadding: 5,
    title: 'Форма добавления контрагента',

    requires: [
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.contragent.Grid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Contragent',
                    fieldLabel: 'Контрагент',
                    store: 'B4.store.Contragent',
                    columns: [{ text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } }],
                    editable: false,
                    allowBlank: false,
                    labelAlign: 'right'
                },
                {
                    xtype: 'datefield',
                    name: 'DateFrom',
                    allowBlank: false,
                    fieldLabel: 'Дела с даты',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datefield',
                    name: 'DateTo',
                    allowBlank: false,
                    fieldLabel: 'Дела по дату',
                    format: 'd.m.Y'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
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
                            columns: 1,
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