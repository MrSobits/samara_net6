Ext.define('B4.view.actsurvey.ConclusionEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.actSurveyConclusionEditWindow',
    
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {type: 'vbox', align: 'stretch'},
    width: 500,
    minHeight: 250,
    bodyPadding: 5,
    title: 'Форма заключения',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.dict.Inspector'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 130,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    padding: '0 5 0 0',
                    defaults: {
                        labelAlign: 'right'
                    },
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            labelWidth: 130,
                            name: 'DocNumber',
                            fieldLabel: 'Номер документа',
                            editable: false,
                            maxLength: 50
                        },
                        {
                            xtype: 'datefield',
                            labelWidth: 50,
                            name: 'DocDate',
                            fieldLabel: 'Дата',
                            editable: false,
                            format: 'd.m.Y'
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Official',
                    fieldLabel: 'ДЛ, вынесшее заключение',
                    store: 'B4.store.dict.Inspector',
                    textProperty: 'Fio',
                    blankText: 'Это поле обязательно для заполнения',
                    columns: [
                        { text: 'Код', dataIndex: 'Code', flex: 1, filter: 'textfield' },
                        { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: 'textfield' },
                        { text: 'Должность', dataIndex: 'Position', flex: 1, filter: 'textfield' }
                    ]
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
                    maxLength: 2000,
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