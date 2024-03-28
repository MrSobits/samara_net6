Ext.define('B4.view.program.CorrectionActualizeYearsWindow', {
    extend: 'B4.form.Window',
    
    alias: 'widget.correctionactualizeyearswindow',
    
    requires: [
        'B4.form.ComboBox'
    ],
    
    title: 'Выбор года',
    modal: true,
    width: 450,
    height: 200,
    layout: 'form',
    bodyPadding: 5,
    closable: true,
    initComponent: function () {
        var me = this;
        
        Ext.apply(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right',
                flex: 1
            },
            items: [
                {
                    xtype: 'container',
                    style: 'border: 0px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px 30px 0 5px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Для создания краткосрочной программы выберите год. Те записи, у которых скорректированный год равен выбранному году, перейдут в краткосрочную программу.</span>'
                },
                {
                    xtype: 'b4combobox',
                    editable: false,
                    floating: false,
                    allowBlank: false,
                    displayField: 'Year',
                    valueField: 'Year',
                    name: 'Year',
                    fieldLabel: 'Период краткосрочной программы',
                    url: '/DpkrCorrectionStage2/GetActualizeYears'
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