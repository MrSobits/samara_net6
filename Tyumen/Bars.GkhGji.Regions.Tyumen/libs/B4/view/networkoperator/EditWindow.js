Ext.define('B4.view.networkoperator.EditWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    bodyPadding: 5,
    alias: 'widget.networkoperatoreditwindow',
    title: 'Редактирование',
    trackResetOnLoad: true,
    closeAction: 'hide',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.store.contragent.ContragentForSelect'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
            {
                xtype: 'tabpanel',
                border: false,
                margins: -1,
                items: [
                    {
                        layout: 'anchor',
                        title: 'Общие сведения',
                        border: false,
                        bodyPadding: 5,
                        margins: -1,
                        items: [
                        {
                            xtype: 'fieldset',
                            title: 'Общие сведения',
                            defaults: {
                                labelWidth: 100,
                                labelAlign: 'right',
                                anchor: '100%'
                            },
                            items: [
                            {
                                xtype: 'b4selectfield',
                                name: 'Contragent',
                                fieldLabel: 'Контрагент',
                                store: 'B4.store.contragent.ContragentForSelect',
                                columns: [
                                    { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                    { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                    { text: 'КПП', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                                ],
                                editable: false,
                                itemId: 'sfContragent',
                                allowBlank: false
                            },
                            {
                                xtype: 'textarea',
                                name: 'Description',
                                fieldLabel: 'Описание',
                                maxLength: 3000,
                                flex: 1
                            }]
                        }],
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
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }]
        });

        me.callParent(arguments);
    }
});