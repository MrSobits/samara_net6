Ext.define('B4.view.builder.FeedbackEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.TypeAssessment',
        'B4.enums.TypeAuthor'
    ],
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    minHeight: 320,
    minWidth: 500,
    bodyPadding: 5,
    itemId: 'builderFeedbackEditWindow',
    title: 'Отзыв заказчика',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'combobox', editable: false,
                    fieldLabel: 'Оценка',
                    store: B4.enums.TypeAssessment.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'TypeAssessment',
                    editable: false
                },
                {
                    xtype: 'datefield',
                    name: 'DocumentDate',
                    fieldLabel: 'Дата',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Документ',
                    maxLength: 100
                },
                {
                    xtype: 'combobox', editable: false,
                    fieldLabel: 'Автор',
                    store: B4.enums.TypeAuthor.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'TypeAuthor',
                    editable: false
                },
                {
                    xtype: 'textfield',
                    name: 'OrganizationName',
                    fieldLabel: 'Наименование организации',
                    maxLength: 50
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл'
                },
                {
                    xtype: 'textarea',
                    name: 'Content',
                    fieldLabel: 'Содержание отзыва',
                    maxLength: 500,
                    flex: 1
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