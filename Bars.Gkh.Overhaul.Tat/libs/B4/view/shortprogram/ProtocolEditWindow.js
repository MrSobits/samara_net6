Ext.define('B4.view.shortprogram.ProtocolEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.shortprogramprotocolwindow',
    
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    minHeight: 450,
    width: 700,
    minWidth: 600,
    maxWidth: 800,
    bodyPadding: 5,
    itemId: 'shortprogProtocolEditWindow',
    title: 'Протокол о необходимости проведения КР',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.form.ComboBox',

        'B4.ux.button.Close',
        'B4.ux.button.Save',
        
        'B4.store.Contragent'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    flex: 1,
                    layout: { type: 'vbox', align: 'stretch' },
                    defaults: {
                        labelWidth: 120,
                        labelAlign: 'right'
                    },
                    title: 'Общие сведения',
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentName',
                            fieldLabel: 'Документ',
                            allowBlank: false,
                            maxLength: 300
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox'
                            },
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNum',
                                    fieldLabel: 'Номер',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateFrom',
                                    itemId: 'dfDateFrom',
                                    fieldLabel: 'от',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'File',
                            fieldLabel: 'Файл',
                            allowBlank: false,
                            editable: false
                        },
                        {
                            xtype: 'b4selectfield',
                            editable: false,
                            name: 'Contragent',
                            fieldLabel: 'Участник процесса',
                            store: 'B4.store.Contragent',
                            itemId: 'sfContragent',
                            columns: [
                                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'textarea',
                            name: 'Description',
                            fieldLabel: 'Описание',
                            maxLength: 2000,
                            flex: 1
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Количественные характеристики',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'gkhdecimalfield',
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    name: 'CountVote',
                                    itemId: 'nfCountVote',
                                    fieldLabel: 'Количество голосов (кв. м.)',
                                    labelWidth: 180
                                },
                                {
                                    name: 'CountVoteGeneral',
                                    itemId: 'nfCountVoteGeneral',
                                    fieldLabel: 'Общее количество голосов (кв. м.)',
                                    labelWidth: 215
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '5 0 5 0',
                            layout: 'hbox',
                            anchor: '50%',
                            items: [
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'CountAccept',
                                    itemId: 'nfCountAccept',
                                    fieldLabel: 'Доля принявших участие (%)',
                                    labelWidth: 180,
                                    labelAlign: 'right',
                                    flex: 0.5
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'gkhdecimalfield',
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    name: 'GradeClient',
                                    itemId: 'nfGradeClient',
                                    fieldLabel: 'Оценка заказчика',
                                    labelWidth: 180
                                },
                                {
                                    name: 'GradeOccupant',
                                    itemId: 'nfGradeOccupant',
                                    fieldLabel: 'Оценка жителей',
                                    labelWidth: 215
                                }
                            ]
                        }

                    ]
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