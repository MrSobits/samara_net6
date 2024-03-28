Ext.define('B4.view.objectcr.DocumentWorkCrEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.YesNo'
    ],

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 700,
    minWidth: 600,
    maxWidth: 800,
    bodyPadding: 5,
    
    alias: 'widget.objectcrdocumentwin',
    itemId: 'documentWorkCrEditWindow',
    title: 'Документ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 110,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Документ',
                    maxLength: 300
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Contragent',
                    fieldLabel: 'Контрагент',
                    store: 'B4.store.Contragent',
                    columns: [{ text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } }],
                    editable: false,
                    allowBlank: false
                },
		        {
		            xtype: 'container',
		            defaults: {
		                labelWidth: 110,
		                flex: 1,
		                labelAlign: 'right'
		            },
		            layout: {
		                type: 'hbox'
		            },
		            items: [
		                {
		                    xtype: 'textfield',
		                    name: 'DocumentNum',
		                    fieldLabel: 'Номер документа',
		                    maxLength: 50
		                },
		                 {
		                     xtype: 'datefield',
		                     name: 'DateFrom',
		                     fieldLabel: 'от',
		                     format: 'd.m.Y'
		                 }
		            ]
		        },
                {
                    xtype: 'b4filefield', editable: false,
                    name: 'File',
                    fieldLabel: 'Файл',
                    anchor: '100%'
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    anchor: '100%, -100',
                    maxLength: 500
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    fieldLabel: 'Выводить документ на портал',
                    name: 'UsedInExport',
                    store: B4.enums.YesNo.getStore(),
                    displayField: 'Display',
                    valueField: 'Value'
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
