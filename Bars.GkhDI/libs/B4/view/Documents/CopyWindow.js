Ext.define('B4.view.documents.CopyWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'anchor',
    width: 600,
    bodyPadding: 5,
    itemId: 'documentsCopyWindow',
    title: 'Копирование документов',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.dict.PeriodDi'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 190,
                anchor: '100%',
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    padding: 2,
                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; border: 1px solid lightblue;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">' +
                        '<span>Период копирования – период, из которого будут скопированы документы</span><br/>' +
                        '<span>Период текущий - период, в который будут скопированы документы</span><br/>' +
                        '</span>'
                },
                {
                    xtype: 'textfield',
                    itemId: 'tfManagingOrg',
                    fieldLabel: 'Управляющая организация',
                    readOnly: true
                },
                {
                    xtype: 'textfield',
                    itemId: 'tfPeriodDiCurrent',
                    fieldLabel: 'Период текущий',
                    readOnly: true
                },
                {
                    xtype: 'b4selectfield',
                    itemId: 'sfPeriodDiFrom',
                    fieldLabel: 'Период копирования',
                   

                    store: 'B4.store.dict.PeriodDi',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right',
                        labelWidth: 190
                    },
                    title: 'Выберите документы',
                    items: [
                        {
                            xtype: 'checkbox',
                            itemId: 'cbProjectContract',
                            boxLabel: 'Проект договора управления с собственником помещений',
                            padding: '0 0 0 10'
                        },
                        {
                            xtype: 'checkbox',
                            itemId: 'cbCommunalService',
                            boxLabel: 'Перечень и качество коммунальных услуг',
                            padding: '0 0 0 10'
                        },
                        {
                            xtype: 'checkbox',
                            itemId: 'cbApartmentService',
                            boxLabel: 'Базовый перечень показателей качества содержания, эксплуатации и технического обслуживания жилых зданий',
                            padding: '0 0 0 10'
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
                                {
                                    xtype: 'b4savebutton',
                                    text: 'Копировать',
                                    tooltip: 'Копировать'
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