Ext.define('B4.view.version.ActualizePeriodWindow', {
    extend: 'B4.form.Window',
    
    alias: 'widget.versionactualizeperiodwindow',
    title: 'Период актуализации',
    modal: true,
    width: 350,
    height: 250,
    layout: 'form',
    bodyPadding: 5,
    closable: true,
    initComponent: function () {
        var me = this;
        
        Ext.apply(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right',
                flex: 1
            },
            items: [
                {
                    xtype: 'container',
                    style: 'border: 0px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px 30px 0 5px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Введите период, за который необходимо актуализировать данные в долгосрочной программе.</span>'
                },
                {
                    xtype: 'numberfield',
                    fieldLabel: 'Начало периода',
                    name: 'YearStart',
                    hideTrigger: true,
                    allowBlank: false,
                    minValue: 2014,
                    maxValue: 2050
                },
                {
                    xtype: 'numberfield',
                    fieldLabel: 'Окончание периода',
                    name: 'YearEnd',
                    hideTrigger: true,
                    minValue: 2014,
                    maxValue: 2050
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
                                {
                                    xtype: 'b4savebutton',
                                    text: 'Продолжить'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    text: 'Отмена'
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