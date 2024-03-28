Ext.define('B4.view.builder.DocumentEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.dict.Period',
        'B4.store.Contragent',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.YesNoNotSet',
        'B4.form.SelectField'
    ],
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {type: 'vbox', align: 'stretch'},
    width: 500,
    minWidth: 500,
    minHeight: 400,
    bodyPadding: 5,
    itemId: 'builderDocumentEditWindow',
    title: 'Документ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Contragent',
                    itemId: 'docContragent',
                    fieldLabel: 'Контрагент',
                    store: 'B4.store.Contragent',
                    columns: [{ text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } }],
                    editable: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Period',
                    fieldLabel: 'Период',
                    store: 'B4.store.dict.Period',
                    editable: false
                },
                {
                    xtype: 'combobox', 
                    fieldLabel: 'Наличие документа',
                    store: B4.enums.YesNoNotSet.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'DocumentExist',
                    editable: false
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    allowBlank: false,
                    fieldLabel: 'Тип документа',
                    name: 'BuilderDocumentType',
                    textProperty: 'Name',
                    store: 'B4.store.dict.BuilderDocumentType',
                    columns: [
                        { text: 'Тип документа', dataIndex: 'Name', flex: 1 },
                        { text: 'Код', dataIndex: 'Code', flex: 1 }]
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Наименование',
                    maxLength: 100
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentNum',
                    fieldLabel: 'Номер',
                    maxLength: 50
                },
                {
                    xtype: 'datefield',
                    name: 'DocumentDate',
                    fieldLabel: 'Дата',
                    format: 'd.m.Y',
                    width: 290
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл'
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
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