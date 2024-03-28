Ext.define('B4.view.deliveryagent.AddWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.deliveryagentaddwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    minHeight: 100,
    maxHeight: 100,
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