Ext.define('B4.view.licensereissuance.RequestProvDocEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.licensereissuanceprovdoceditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 380,
    minWidth: 380,
    minHeight: 180,
    maxHeight: 180,
    bodyPadding: 5,
    title: 'Документ',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'LicProvidedDoc',
                    fieldLabel: 'Наименование',
                    store: 'B4.store.dict.LicenseProvidedDoc',
                    allowBlank: false,
                    editable: false,
                    textProperty: 'Name',
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'Number',
                    fieldLabel: 'Номер',
                    allowBlank: false,
                    flex: 1
                },
                {
                    xtype: 'datefield',
                    name: 'Date',
                    fieldLabel: 'Дата',
                    allowBlank: false,
                    flex: 1,
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
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