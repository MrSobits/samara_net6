Ext.define('B4.view.program.CorrectionResultPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.correctionresultpanel',

    requires: [
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.view.program.CorrectionResultGrid',
        'B4.form.ComboBox'
    ],

    minWidth: 750,
    width: 750,
    autoScroll: true,
    bodyPadding: 5,
    closable: true,
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'border',
        padding: 3
    },
    defaults: {
        split: true
    },
    title: 'Результат корректировки',
    trackResetOnLoad: true,
    frame: true,

    initComponent: function() {
        var me = this;
        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    layout: { type: 'vbox', align: 'stretch' },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            flex: 1,
                            padding: '0 0 5 0',
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    name: 'Municipality',
                                    fieldLabel: 'Муниципальное образование',
                                    labelWidth: 152,
                                    editable: false,
                                    storeAutoLoad: false,
                                    url: '/Municipality/ListWithoutPaging'
                                },
                                {
                                    xtype: 'buttongroup',
                                    columns: 1,
                                    defaults: {
                                        xtype: 'button',
                                        enableToggle: true,
                                        toggleGroup: 'filterResultType',
                                        pressed: false,
                                        iconCls: 'icon-page-white-magnify'
                                    },
                                    items: [
                                        {
                                            actionName: 'AllRecords',
                                            text: 'Все записи: ',
                                            filterValue: 0
                                        },
                                        {
                                            actionName: 'InShortTerm',
                                            text: 'Записи краткосрочной программы: ',
                                            filterValue: 20
                                        },
                                        /*
                                        {
                                            actionName: 'InLongTerm',
                                            text: 'Запись существует в долгосрочке: ',
                                            filterValue: 30
                                        },
                                        */
                                        {
                                            actionName: 'RemoveFromShortTerm',
                                            text: 'Записи удаляемые из краткосрочной программы: ',
                                            filterValue: 40
                                        },
                                        {
                                            actionName: 'AddInShortTerm',
                                            text: 'Новые записи в краткосрочной программе: ',
                                            filterValue: 50
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px 30px 0 5px; padding: 5px 10px; line-height: 16px;',
                            html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">У записей, выделенных оранжевым цветом, Скорректированный год превышает год в опубликованной региональной программе. </br>После формирования краткосрочной программы записи, подсвеченные желтым, будут добавлены в краткосрочную программу, а записи, подсвеченные красным, удалятся из нее.</span>'
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'hidden',
                    name: 'MunicipalityId'
                },                
                {
                    region: 'center',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'correctionresultgrid',
                            closable: false,
                            title: '',
                            flex: 1,
                            border: 0
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});