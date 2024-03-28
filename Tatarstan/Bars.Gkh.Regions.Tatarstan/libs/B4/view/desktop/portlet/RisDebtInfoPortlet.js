Ext.define('B4.view.desktop.portlet.RisDebtInfoPortlet', {
    extend: 'B4.view.desktop.portal.Portlet',
    alias: 'widget.risdebtinfoportlet',
    ui: 'b4portlet',
    iconCls: 'wic-eye',
    requires: [
        'B4.model.RisDebtInfo.RisDebtInfo'
    ],

    title: 'Поступившие запросы о наличии или отсутствии<br>подтвержденной вступившим актом задолженности за ЖКУ',
    layout: { type: 'vbox', align: 'stretch' },
    cls: 'x-portlet orange',
    collapsible: false,
    closable: false,
    footer: true,
    draggable: false,
    column: 2,
    position: 1,
    
    permissions: [
        {
            name: 'Widget.Debt', selector: 'risdebtinfoportlet' ,
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
    ],
    
    dockedItems: [{
        xtype: 'toolbar',
        dock: 'bottom',
        ui: 'b4portlet-footer',
        items: [
            '->',
            {
                xtype: 'button',
                handler: function(){
                    var model = Ext.ModelManager.getModel('B4.model.RisDebtInfo.RisDebtInfo'),
                        panel = this.up('panel');

                    panel.mask.show();
                    model.load(0, {
                        callback: function(record) {
                            panel.build(record);
                        }
                    });
                },
                ui: 'searchportlet-footer-btn-rt',
                height: 30,
                text: 'Обновить'
            },
            {
                xtype: 'button',
                handler: function(){
                    B4.Ajax.request({
                        url: B4.Url.action('GetRisUrl', 'RisSettings'),
                        params: {
                            redirect: 'debtsubrequest'
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
                },
                itemId: 'debtRequestsRedirect',
                ui: 'searchportlet-footer-btn-rt',
                height: 30,
                text: 'Перейти к запросам'
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
            model = Ext.ModelManager.getModel('B4.model.RisDebtInfo.RisDebtInfo');

        me.mask.show();
        model.load(0, {
            callback: function (record) {
                me.build(record);
            }
        });

        me.callParent(arguments);
    },

    build: function (record) {
        var me = this,
            needResponse = record ? record.get("NeedResponse") : 0,
            notSentResponse = record ? record.get("NotSentResponse") : 0,
            sentResponse = record ? record.get("SentResponse") : 0;
        
        var stringTpl =
            '<div class="widget-item">' +
                '<table class="debt-table">' +
                    '<tr' + (needResponse > 0 ? ' class="result-attention-row"' : '') +
                            ' data-qtip="Количество запросов о наличии или отсутствии подтвержденной вступившим актом задолженности за ЖКУ, ' +
                            'по которым необходимо заполнить ответ и отправить его в ГИС ЖКХ до наступления крайнего срока">' +
                        '<td class="result-table-header result-table-text">Запросы по которым требуется предоставить ответ</td>' +
                        '<td class="result-table-text">' + needResponse + '</td>' +
                    '</tr>' +
                    '<tr' + (notSentResponse > 0 ? ' class="result-attention-row"' : '') +
                            ' data-qtip="Количество запросов о наличии или отсутствии подтвержденной вступившим актом задолженности за ЖКУ, ' +
                            'ответ по которым заполнен, но не отправлен в ГИС ЖКХ">' +
                        '<td class="result-table-header result-table-text">Запросы, по которым имеется ответ, не отправленный в ГИС ЖКХ</td>' +
                        '<td class="result-table-text">' + notSentResponse + '</td>' +
                    '</tr>' +
                    '<tr data-qtip="Количество запросов о наличии или отсутствии подтвержденной вступившим актом задолженности за ЖКУ, ' +
                            'ответ по которым был успешно отправлен в ГИС ЖКХ до наступления крайнего срока">' +
                        '<td class="result-table-header result-table-text">Запросы, ответ по которым отправлен в ГИС ЖКХ</td>' +
                        '<td class="result-table-text">' + sentResponse + '</td>' +
                    '</tr>' +
                '</table>' +
             '<div class="clearfix"></div>' +
            '</div>';

        me.remove(me.down('[ui=debtportlet]'));
        me.add({
            xtype: 'component',
            ui: 'debtportlet',
            renderTpl: new Ext.XTemplate(stringTpl)
        });

        me.mask.hide();
    }
});