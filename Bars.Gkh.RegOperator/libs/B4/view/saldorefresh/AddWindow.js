Ext.define('B4.view.saldorefresh.AddWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.saldorefreshaddwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 400,
    maxHeight: 100,
    bodyPadding: 5,
    title: 'Добавить группу',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.SaldoRefresh',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.SaldoRefresh.Grid'
    ],

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Group',
                    fieldLabel: 'Группа',
                    store: 'B4.store.dict.PersAccGroup',
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }],
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});