Ext.define('B4.view.networkoperator.RoEditWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    bodyPadding: 5,
    alias: 'widget.networkoperatorroeditwindow',
    title: 'Редактирование',
    trackResetOnLoad: true,
    closeAction: 'hide',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.store.networkoperator.NetworkOperator',
        'B4.view.Control.GkhTriggerField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right',
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'NetworkOperator',
                    fieldLabel: 'Оператор связи',
                    store: 'B4.store.networkoperator.NetworkOperator',
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'КПП', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'textfield',
                    name: 'Bandwidth',
                    fieldLabel: 'Скорость ШПД (Мбит/сек)',
                    maxLength: 100,
                    flex: 1,
                    allowBlank: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'TechDecisions',
                    itemId: 'techDecisionTriggerField',
                    fieldLabel: 'Техническое решение',
                    editable: false
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
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});