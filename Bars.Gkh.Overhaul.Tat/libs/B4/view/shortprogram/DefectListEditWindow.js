Ext.define('B4.view.shortprogram.DefectListEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.shortprogramdefectlistwindow',
    
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'anchor',
    width: 500,
    minWidth: 400,
    maxWidth: 600,
    maxHeight: 220,
    minHeight: 220,
    bodyPadding: 5,
    itemId: 'shortprogramdefectlistwindow',
    title: 'Дефектная ведомость',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.shortprogram.DefectListWork',
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 110,
                labelAlign: 'right',
                anchor: '100%',
                allowBlank: false,
                layout: {
                    type: 'anchor'
                }
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Work',
                    fieldLabel: 'Вид работы',
                    store: 'B4.store.shortprogram.DefectListWork',
                    editable: false
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Документ',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'datefield',
                    name: 'DocumentDate',
                    fieldLabel: 'Дата документа',
                    format: 'd.m.Y',
                    width: 250,
                    anchor: null
                },
                {
                    xtype: 'b4filefield',
                    editable: false,
                    name: 'File',
                    fieldLabel: 'Файл',
                    itemId: 'ffFile'
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    name: 'Sum',
                    fieldLabel: 'Сумма по ведомости, руб',
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    decimalSeparator: ','
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