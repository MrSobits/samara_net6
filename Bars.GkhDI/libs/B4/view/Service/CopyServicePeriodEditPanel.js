Ext.define('B4.view.service.CopyServicePeriodEditPanel', {
    extend: 'Ext.form.Panel',
    trackResetOnLoad: true,
    autoScroll: true,
    border: false,
    bodyStyle: Gkh.bodyStyle,
    itemId: 'copyServicePeriodEditPanel',
    layout: 'anchor',

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
                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Период копирования - период, из которого будут скопированы сведения об услугах <br/ > Период текущий – период, в который будут скопированы сведения об услугах</span>'
                },
                {
                    xtype: 'b4selectfield',
                    itemId: 'sfManagingOrg',
                    fieldLabel: 'Управляющая организация',
                    editable: false,
                    readOnly: true
                },
                {
                    xtype: 'b4selectfield',
                    itemId: 'sfPeriodDiCurrent',
                    fieldLabel: 'Период текущий',
                    editable: false,
                    readOnly: true
                },
                {
                    xtype: 'b4selectfield',
                    itemId: 'sfPeriodDiFrom',
                    fieldLabel: 'Период копирования',
                   

                    store: 'B4.store.dict.PeriodDi',
                    editable: false,
                    allowBlank: false
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
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
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});