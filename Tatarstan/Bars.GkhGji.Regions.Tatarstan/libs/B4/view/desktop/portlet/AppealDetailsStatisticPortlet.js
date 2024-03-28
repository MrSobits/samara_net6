Ext.define('B4.view.desktop.portlet.AppealDetailsStatisticPortlet', {
    extend: 'B4.view.desktop.portal.Portlet',
    alias: 'widget.soprappealdetailsstatisticportlet',
    ui: 'b4portlet',
    iconCls: 'wic-eye',
    requires: [
        'B4.model.rapidresponsesystem.AppealDetailsStatistic'
    ],

    title: 'Поступившие обращения в СОПР',
    layout: { type: 'vbox', align: 'stretch' },
    cls: 'x-portlet orange',
    collapsible: false,
    closable: false,
    footer: true,
    column: 2,
    
    permissions: [
        {
            name: 'Widget.SoprAppealDetailsStatistic',
            selector: 'soprappealdetailsstatisticportlet',
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
                action: 'refresh',
                ui: 'searchportlet-footer-btn-rt',
                height: 30,
                text: 'Обновить',
                handler: function(){
                    var portlet = this.up('panel');

                    portlet.mask.show();
                    portlet.loadData();
                },
            },
            {
                xtype: 'button',
                action: 'redirectToAppealRegistry',
                height: 30,
                ui: 'searchportlet-footer-btn-rt',
                text: 'Перейти к обращениям',
                handler: function() {
                    var portlet = this.up('panel');

                    portlet.redirectToAppealRegistry();
                }
            },
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
        var me = this;

        me.mask.show();
        me.loadData();

        me.callParent(arguments);
    },

    build: function (record) {
        var me = this;
            
        if(!record){
            me.mask.hide();
            return;
        }
        
        var newAppealsCount = record.get('NewAppealsCount'),
            appealsInWorkCount = record.get('AppealsInWorkCount'),
            processedAppealsCount = record.get('ProcessedAppealsCount'),
            notProcessedAppealsCount = record.get('NotProcessedAppealsCount');

        if (record) {
            var stringTpl =
                '<div class="widget-item">' +
                    '<table class="debt-table">' +
                        '<tr>' +
                            '<td class="result-table-header result-table-text">Новые обращения</td>' +
                            '<td class="result-table-text">' + newAppealsCount + '</td>' +
                        '</tr>' +
                        '<tr>' +
                            '<td class="result-table-header result-table-text">Обращения в работе</td>' +
                            '<td class="result-table-text">' + appealsInWorkCount + '</td>' +
                        '</tr>' +
                        '<tr>' +
                            '<td class="result-table-header result-table-text">Обработанные в срок</td>' +
                            '<td class="result-table-text">' + processedAppealsCount + '</td>' +
                        '</tr>' +
                        '<tr>' +
                            '<td class="result-table-header result-table-text">Необработанные в срок</td>' +
                            '<td class="result-table-text">' + notProcessedAppealsCount + '</td>' +
                        '</tr>' +
                    '</table>' +
                    '<div class="clearfix"></div>' +
                '</div>';

            me.remove(me.down('[ui=appealDetailsStatisticPortlet]'));
            me.add({
                xtype: 'component',
                ui: 'appealDetailsStatisticPortlet',
                renderTpl: new Ext.XTemplate(stringTpl)
            });
        }

        me.mask.hide();
    },
    
    loadData: function() {
        var me = this,
            model = Ext.ModelManager.getModel('B4.model.rapidresponsesystem.AppealDetailsStatistic');

        model.load(0, {
            callback: function (record) {
                me.build(record);
            }
        });
    },
    
    redirectToAppealRegistry: function() {
        var me = this,
            items = B4.getBody().items;

        var index = items.findIndexBy(function (tab) {
            return tab.urlToken != null && tab.urlToken.indexOf('rapidresponsesystemappeal') === 0;
        });

        if (index !== -1) {
            B4.getBody().remove(items.items[index], true);
        }

        b4app.redirectTo('rapidresponsesystemappeal');
    }
});