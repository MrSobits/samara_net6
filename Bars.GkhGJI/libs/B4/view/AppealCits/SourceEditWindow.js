Ext.define('B4.view.appealcits.SourceEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    itemId: 'appealCitsSourceEditWindow',
    title: 'Форма редактирования источника поступления',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.dict.RevenueSourceGji',
        'B4.store.dict.RevenueFormGji'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    labelAlign: 'right',
                    anchor: '100%',
                   

                    store: 'B4.store.dict.RevenueSourceGji',
                    name: 'RevenueSource',
                    itemId: 'sflRevenueSource',
                    fieldLabel: 'Источник',
                    flex: 1,
                    editable: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: {xtype: 'textfield'} }
                    ]
                },
                {
                    xtype: 'textfield',
                    anchor: '100%',
                    name: 'RevenueSourceNumber',
                    itemId: 'tfRevenueSourceNumber',
                    flex: 1,
                    fieldLabel: 'Исх. № источника',
                    labelAlign: 'right',
                    maxLength: 50
                },
                {
                    xtype: 'b4selectfield',
                    labelAlign: 'right',
                    anchor: '100%',
                   

                    store: 'B4.store.dict.RevenueFormGji',
                    name: 'RevenueForm',
                    itemId: 'sflRevenueForm',
                    flex: 1,
                    editable: false,
                    fieldLabel: 'Форма поступления',
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 }]
                },
                {
                    xtype: 'datefield',
                    anchor: '100%',
                    width: 250,
                    name: 'RevenueDate',
                    fieldLabel: 'Дата поступления',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datefield',
                    anchor: '100%',
                    width: 250,
                    name: 'SSTUDate',
                    fieldLabel: 'Дата источника',
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