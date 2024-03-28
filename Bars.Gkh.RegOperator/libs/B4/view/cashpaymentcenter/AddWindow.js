Ext.define('B4.view.cashpaymentcenter.AddWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.cashpaymentcenteraddwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    minHeight: 140,
    maxHeight: 140,
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
                    columns: [
                        { text: 'Наименование', dataIndex: 'ShortName', flex: 2, filter: { xtype: 'textfield' } },
                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    editable: false,
                    allowBlank: false,
                    labelAlign: 'right'
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowDecimals: true,
                    minValue: 0,
                    negativeText: 'Значение не может быть отрицательным',
                    allowBlank: false,
                    name: 'Identifier',
                    labelAlign: 'right',
                    fieldLabel: 'Идентификатор РКЦ',
                    maxLength: 250
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