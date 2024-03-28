Ext.define('B4.view.appealcits.AnswerGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.appealcitsAnswerGrid',

    requires: [
        'B4.Url',
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Ответы',
    store: 'appealcits.Answer',
    itemId: 'appealCitsAnswerGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'actioncolumn',
                    width: 20,
                    itemId: 'transferRfRecPrintColumn',
                    icon: 'content/img/icons/printer.png',
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var param = { AppealCitsAnswerId: rec.get('Id') };
                        this.params = {};
                        this.params.userParams = Ext.JSON.encode(param);

                        Ext.apply(this.params, { reportId: 'AppealCitsAnswer' });
                        var urlParams = Ext.urlEncode(this.params);

                        var newUrl = Ext.urlAppend('/GkhReport/ReportPrint/?' + urlParams, '_dc=' + (new Date().getTime()));
                        newUrl = B4.Url.action(newUrl);

                        Ext.DomHelper.append(document.body, {
                            tag: 'iframe',
                            id: 'downloadIframe',
                            frameBorder: 0,
                            width: 0,
                            height: 0,
                            css: 'display:none;visibility:hidden;height:0px;',
                            src: newUrl
                        });
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentName',
                    flex: 1,
                    text: 'Документ'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    flex: 1,
                    text: 'Дата документа',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    flex: 1,
                    text: 'Номер документа'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
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
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                                /*{
                                    xtype: 'button',
                                    itemId: 'btnPrint',
                                    text: 'Печать шаблона ответа',
                                    iconCls: 'icon-printer',
                                    menu: [
                                            {
                                                text: 'Ответ',
                                                textAlign: 'left',
                                                actionName: 'AppealCitsAnswer'
                                                
                                            }]
                                }*/
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});