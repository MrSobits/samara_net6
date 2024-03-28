Ext.define('B4.view.version.ActualizeSubProgramByFilters', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Save',
        'B4.view.version.ActualizeSubProgramByFiltersAddGrid',
        'B4.view.version.ActualizeSubProgramByFiltersDeleteGrid',
    ],
    alias: 'widget.versionactualizesubprogrambyfilterswindow',
    title: 'Выбор домов для актуализации подпрограммы',
    modal: true,
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
                            xtype: 'actualizesubprogrambyfilteraddgrid'
                        },
                        {
                            xtype: 'actualizesubprogrambyfilterdeletegrid'
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
                            xtype: 'buttongroup',
                            items: [
                                {
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
                                    xtype: 'b4savebutton',
                                    text: 'Актуализировать',
                                    tooltip: {
                                        //title: 'Заголовок',
                                        width: 200,
                                        text: 'Актуализировать все элементы из обоих списков'
                                    }
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