Ext.define('B4.view.desktop.portlet.SendingDataResultPortlet', {
    extend: 'B4.view.desktop.portal.Portlet',
    alias: 'widget.sendingdataresultoportlet',
    ui: 'b4portlet',
    iconCls: 'wic-eye',
    requires: [
        'B4.model.sendingdataresult.SendingDataResult'
    ],

    title: 'Результат отправки данных с ГИС ЖКХ',
    layout: { type: 'vbox', align: 'stretch' },
    collapsible: false,
    closable: false,
    header: true,
    footer: true,
    contragentId: 0,
    column: 2,
    
    permissions: [
        {
            name: 'Widget.SendingDataResult',
            applyTo: 'sendingdataresultoportlet',
            selector: 'portalpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        }
    ],

    dockedItems: [{
        xtype: 'toolbar',
        dock: 'bottom',
        ui: 'b4portlet-footer',
        items: [
            '->',
            {
                xtype: 'button',
                itemId: 'refreshBtn',
                ui: 'searchportlet-footer-btn-rt',
                height: 30,
                text: 'Обновить',
                handler: function(){
                    var model = Ext.ModelManager.getModel('B4.model.sendingdataresult.SendingDataResult'),
                        panel = this.up('panel');

                    panel.mask.show();
                    model.load(0, {
                        callback: function(record) {
                            panel.build(record);
                        }
                    });
                },
            },
            {
                xtype: 'button',
                id: 'gisDownload',
                height: 30,
                ui: 'searchportlet-footer-btn-rt',
                text: 'Загрузка данных для ГИС ЖКХ',
                handler: function() {
                    this.up('panel').redirectToUrl('importdatakernel');
                },
            },
            {
                xtype: 'button',
                id: 'gisSend',
                height: 30,
                ui: 'searchportlet-footer-btn-rt',
                text: 'Отправить данные в ГИС ЖКХ',
                handler: function(){
                    var contragentId = this.up('panel').contragentId;
                    if(contragentId==0){
                        Ext.Msg.alert('Ошибка!', ('Нет информации о контрагенте'));
                    }
                    else {
                    this.up('panel').redirectToUrl('gisintegration?contragentId=' + contragentId);
                    }
                },
            }
        ]
    }],

    initComponent: function () {
        var me = this;

        me.mask = Ext.create('Ext.LoadMask', me, {
            msg: "Загрузка.."
        });

        me.callParent();
    },

    afterRender: function () {
        var me = this,
            model = Ext.ModelManager.getModel('B4.model.sendingdataresult.SendingDataResult');

        me.mask.show();
        model.load(0, {
            callback: function (record) {
                me.build(record);
            }
        });

        me.callParent(arguments);
    },

    build: function (record) {
        var me = this;

        if (record) {
            me.contragentId = record.get("ContragentId");
            var stringTpl = 
                '<div class="widget-item">' +
                '<h1 class="result-table-org-text" >' + "Организация: "+ record.get("ContragentName") + '</h1>' +
                '<hr>' +
                '<table class="result-table">' +
                '<tr' + (record.get("FormatFileLoaded") != true ? ' class="result-attention-row"' : ' class="result-success-row"') + '>' +
                '<td class="result-table-header result-table-text">Загрузка файла с данными для ГИС ЖКХ (Формат 4.0)</td>' +
                '<td class="result-table-text">' + (record.get("FormatFileLoaded") ? "Загружен" : "Не загружен") + '</td>' +
                '</tr>';

            if(record.get("MethodsInfo") != null) {
                Ext.each(record.get("MethodsInfo"), function (method, i) {
                    stringTpl +=
                        '<tr' + (((method ? method.TotalCount : 0) != (method ? method.SentCount : 0)) || ((method ? method.TotalCount : 0) == 0) ? ' class="result-attention-row"' : ' class="result-success-row"') + '>' +
                        '<td class="result-table-header result-table-text">' + method.MethodName + '</td>' +
                        '<td class="result-table-text" ' +
                        (method.UrlRegistry ? 'style="cursor: pointer;"' : '') +
                        ' methodName = "'+method.MethodName+'"' +
                        ' urlRegistry = "'+method.UrlRegistry+'"' +
                        ' data-setListeners =  "{[this.setListeners("'+method.MethodName+'")]}">' + 
                        method.SentCount + ' из ' + method.TotalCount +
                        '</td>' +
                        '</tr>';
                });
            }
            stringTpl +=
                '</table>' +
                '<div class="clearfix"></div>' +
                '</div>';

            me.remove(me.down('[ui=sendingdataresultoportlet]'));
            me.add({
                xtype: 'component',
                ui: 'sendingdataresultoportlet',
                renderTpl: new Ext.XTemplate(stringTpl,
                    {
                        setListeners: function (value) {
                            Ext.Function.defer(this.addListener, 1, this, [value]);
                            
                        }, 
                        addListener: function(value) {
                            Ext.select('td[methodName="'+value+'"]').on('click',
                                function () {
                                    var url = this.getAttribute('urlRegistry');
                                    if(url){
                                        me.redirectToUrl(url);
                                    }
                                })
                        }
                    })
            });
        }

        me.mask.hide();
    },

    redirectToUrl: function (redirect) {
        B4.Ajax.request({
            url: B4.Url.action('GetRisUrl', 'RisSettings'),
            params: {
                redirect: redirect
            }
        }).next(function (response) {
            var data = Ext.decode(response.responseText);
            if (!data) {
                alert('Пользователь не был аутентифицирован.');
                return;
            }

            var win = window.open(data, '_blank');

            if (win) {
                //Browser has allowed it to be opened
                win.focus();
            } else {
                //Browser has blocked it
                alert('Всплывающее окно было заблокировано. Разрешите показ всплывающих окон.');
            }
        }).error(function (e) {
            Ext.Msg.alert('Ошибка!', (e.message || 'Не найден путь до РИС'));
        });
    }
});