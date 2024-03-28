/*
* Грид обращений граждан. При наличия модуля Интеграция с ЭДО перекрывается этим модулем
*/
Ext.define('B4.view.appealcits.GridFond', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.appealCitsGridFond',

    requires: [
        'B4.ux.button.Add',
        'B4.form.GridStateColumn',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'Ext.ux.grid.FilterBar',
        'B4.ux.grid.column.Enum',
        'B4.enums.QuestionStatus',
        'B4.enums.SSTUExportState',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox'
    ],

    store: 'AppealCits',
    itemId: 'appealCitsGridFond',
    closable: false,
    enableColumnHide: true,

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
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус обращения',
                    width: 160,
                    hideable: false,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gji_appeal_citizens';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    },
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.QuestionStatus',
                    dataIndex: 'QuestionStatus',
                    text: 'Статус ССТУ',
                    flex: 0.5,
                    filter: true,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 150,
                    text: 'Муниципальное образование',
                    hideable: true,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'RealObjAddresses',
                //    text: 'Адрес',
                //    flex: 1,
                //    hideable: false,
                //    filter: { xtype: 'textfield' }
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityAddresses',
                    text: 'Адрес',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'CountRealtyObj',
                //    width: 140,
                //    hidden: true,
                //    flex: 1,
                //    text: 'Количество домов',
                //    hideable: false,
                //    filter: { xtype: 'numberfield', operand: CondExpr.operands.eq }
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    width: 80,
                    text: 'Номер',
                    hideable: false,
                    filter: { xtype: 'textfield' }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'NumberGji',
                //    width: 170,
                //    flex: 1,
                //    text: 'Номер ФКР',
                //    hideable: false,
                //    filter: { xtype: 'textfield' }
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Correspondent',
                    text: 'Заявитель',
                    flex: 1,
                    hideable: false,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Executors',
                    text: 'Исполнитель',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },                
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CorrespondentAddress',
                    hidden: true,
                    text: 'Адрес корреспондента',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'Subjects',
                //    text: 'Тематика',
                //    flex: 1,
                //    filter: { xtype: 'textfield' }
                //},
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateFrom',
                    text: 'Дата регистрации обращения',
                    format: 'd.m.Y',
                    width: 100,
                    flex: 0.5,
                    hideable: false,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CheckTime',
                    text: 'Контрольный срок',
                    format: 'd.m.Y',
                    width: 100,
                    flex: 0.5,
                    hideable: false,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ExtensTime',
                    text: 'Продленный срок',
                    format: 'd.m.Y',
                    width: 100,
                    flex: 0.5,
                    hideable: false,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'RevenueSourceNames',
                //    text: 'Источники',
                //    flex: 1,
                //    hideable: false,
                //    filter: { xtype: 'textfield' }
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IncomingSourcesName',
                    text: 'Источник',
                    sortable: false,
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IncomingSources',
                    text: 'Исх. № источника',
                    sortable: false,
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'RevenueSourceNumbers',
                //    text: 'Исх. № источника',
                //    flex: 1,
                //    hideable: false,
                //    filter: { xtype: 'textfield' }
                //},
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'RevenueSourceDates',
                //    text: 'Даты поступления',
                //    width: 100,
                //    flex: 1,
                //    filter: { xtype: 'textfield' }
                //},          
                {
                    xtype: 'datecolumn',
                    dataIndex: 'AnswerDate',
                    text: 'Дата ответа',
                    format: 'd.m.Y',
                    width: 100,
                    flex: 1,
                    hideable: false,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AnswerDocNum',
                    flex: 1,
                    sortable: false,
                    text: 'Документы ответов',
                    //filter: { xtype: 'textfield' }
                    renderer: function (v, meta, record, rowIndex) {
                        var fileInfo = record.get('AnswerSugFile');
                        var fileNames = v.split(',');
                        var answer = '';
                        for (var i = 0;
                            i < fileInfo.length; i++) {
                            answer += '<a href="' +
                                B4.Url.action('/FileUpload/Download?id=' + fileInfo[i].Id) +
                                '" target="_blank" style="color: black">' +
                                fileNames[i] +
                                '</a><br>';
                        }
                        return answer;
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.SSTUExportState',
                    dataIndex: 'SSTUExportState',
                    text: 'Состояние выгрузки',
                    flex: 0.5,
                    filter: true,
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [{ ptype: 'filterbar', renderHidden: false, showShowHideButton: false, showClearAllButton: false }],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record) {
                  
                    var stateCode = record.get('StateCode');
                    var newDaTE = new Date();
                    var dfDateFrom = record.get('DateFrom');
                    var dfCaseDate = record.get('CheckTime');
                    var dateFromDate = new Date(dfDateFrom);
                    var caseDateDate = new Date(dfCaseDate);
                    var yellowDate = Ext.Date.add(newDaTE, Ext.Date.DAY, -5);
                    var redDate = Ext.Date.add(caseDateDate, Ext.Date.DAY, -1);
                    if (stateCode == '0') {
                       
                        return 'back-coralgreen';                     
                      
                    }
                    if (newDaTE > caseDateDate && stateCode != '6') {
                        return 'back-black';
                    }
                    if (newDaTE >= redDate && stateCode != '6') {
                        return 'back-red';   
                    }
                    if (newDaTE >= yellowDate && stateCode != '6') {
                        return 'back-yellow';
                    }
                 
                    return '';
                    
                }
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'b4addbutton'
                        },
                        {
                            xtype: 'button',
                            iconCls: 'icon-table-go',
                            text: 'Экспорт',
                            textAlign: 'left',
                            itemId: 'btnExport'
                        },
                        {
                            xtype: 'checkbox',
                            itemId: 'cbShowCloseAppeals',
                            boxLabel: 'Показать закрытые обращения',
                            labelAlign: 'right',
                            checked: false,
                            margin: '10px 10px 0 0'
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