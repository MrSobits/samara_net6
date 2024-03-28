Ext.define('B4.view.Import.loadidis.Panel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.form.FileField'
    ],

    title: 'Загрузка идентификатора ИС',
    alias: 'widget.loadIdIsPanel',
    layout: 'anchor',
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    closable: true,
    
    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Загрузка идентификаторов ИС фонда',
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                pack: 'start'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 100
                            },
                            items: [
                                {
                                    xtype: 'b4filefield',
                                    fieldLabel: 'Файл',
                                    allowBlank: false,
                                    anchor: '100%',
                                    flex: 1
                                },
                                {
                                    xtype: 'button',
                                    text: 'Загрузить',
                                    tooltip: 'Загрузить',
                                    iconCls: 'icon-accept',
                                    itemId: 'loadIdIsButton'
                                }
                            ]
                        },
                        {
                            xtype: 'checkbox',
                            itemId: 'cbIsOfficialSiteLoadIds',
                            checked: false,
                            boxLabel: 'Загрузка выполняется с официального источника (Идентификаторы ИС Фонда соответствуют федеральному номеру)'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Загрузка технических паспортов',
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'container',
                            padding: 2,
                            style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; border: 1px solid lightblue;',
                            html: '<span class="im-info""></span>  Для обновления существующих домов требуется заполненный федеральный номер. В случае, если он не заполнен, то первоначально необходимо загрузить идентификаторы ИС'
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                pack: 'start'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 100
                            },
                            items: [
                                {
                                    xtype: 'b4filefield',
                                    fieldLabel: 'Файл',
                                    allowBlank: false,
                                    anchor: '100%',
                                    flex: 1
                                },
                                {
                                    xtype: 'button',
                                    text: 'Загрузить',
                                    tooltip: 'Загрузить',
                                    iconCls: 'icon-accept',
                                    itemId: 'loadIdIsButton'
                                }
                            ]
                        },
                        {
                            xtype: 'checkbox',
                            itemId: 'cbIsReplaceCurrentData',
                            checked: false,
                            boxLabel: 'Заменить текущие данные'
                        },
                        {
                            xtype: 'checkbox',
                            itemId: 'cbIsOfficialSiteLoadTps',
                            checked: false,
                            boxLabel: 'Загрузка выполняется с официального источника (Идентификаторы ИС Фонда соответствуют федеральному номеру)'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});