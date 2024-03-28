Ext.define('B4.view.version.ActualizeByFilters', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Save',
        'B4.view.version.ActualizeByFiltersAddGrid',
        'B4.view.version.ActualizeByFiltersDeleteGrid',
    ],
    alias: 'widget.versionactualizebyfilterswindow',
    title: 'Актуализация',
    modal: false,
    maximizable: true,
    width: 1000,
    height: 600,
    minHeight: 300,
    //layout: 'form',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyPadding: 5,
    closable: true,
    listeners:
    {
        beforeshow: function () {
            var me = this,
                startyear = me.down('#dfDateStart');
            startyear.setValue((new Date()).getFullYear());
        }
    },
    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right',
                flex: 1
            },
            items: [
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    items: [   
                        {
                            xtype: 'actualizedbyfilteraddgrid'
                        },
                        {
                            xtype: 'actualizedbyfilterdeletegrid'
                        },
                    ]
                },
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                                {
                                    xtype: 'numberfield',
                                    allowBlank: true,
                                    name: 'StartYear',
                                    fieldLabel: 'Год начала:',
                                    minValue: 2014,
                                    labelWidth: 120,
                                    itemId: 'dfDateStart',
                                    hideTrigger: true,
                                    listeners: {
                                        render: function (c) {
                                            Ext.QuickTips.register({
                                                target: c.getEl(),
                                                text: 'Год капитального ремонта, начиная с которого работы попадут в списки. По умолчанию используется текущий',
                                                enabled: true,
                                                showDelay: 20,
                                                trackMouse: true,
                                                autoShow: true
                                            });
                                        }
                                    }
                                },
                                {
                                    //margin:'2 10 3 10',
                                    xtype: 'button',
                                    iconCls: 'icon-arrow-refresh',
                                    text: 'Обновить списки по критериям',
                                    textAlign: 'left',
                                    action: 'Update',
                                    tooltip: {
                                        //title: 'Заголовок',
                                        width: 200,
                                        text: 'Сформировать списки на добавление и удаление заново по критериям'
                                    }
                                },
                                {
                                    //margin:'2 10 3 10',
                                    xtype: 'button',
                                    text: 'Удалить отмеченные',
                                    textAlign: 'left',
                                    action: 'RemoveSelected',
                                    tooltip: {
                                        //title: 'Заголовок',
                                        width: 200,
                                        text: 'Удалить из списков отмеченные галочками элементы'
                                    }
                                },
                                {
                                    //margin:'2 10 3 10',
                                    xtype: 'b4savebutton',
                                    text: 'Актуализировать',
                                    textAlign: 'left',
                                    tooltip: {
                                        //title: 'Заголовок',
                                        width: 200,
                                        text: 'Актуализировать все элементы из обоих списков'
                                    }
                                },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {  
                                    //margin:'2 10 3 10',
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