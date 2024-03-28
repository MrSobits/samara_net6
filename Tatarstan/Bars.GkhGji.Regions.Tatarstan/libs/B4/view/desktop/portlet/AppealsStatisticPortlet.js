Ext.define('B4.view.desktop.portlet.AppealsStatisticPortlet', {
    extend: 'B4.view.desktop.portal.Portlet',
    alias: 'widget.soprappealsstatisticportlet',
    ui: 'b4portlet',
    iconCls: 'wic-eye',
    requires: [
        'B4.model.rapidresponsesystem.AppealsStatistic'
    ],

    title: 'Статистика по реестру обращений ГЖИ (связанные с СОПР)',
    layout: { type: 'vbox', align: 'stretch' },
    cls: 'x-portlet orange',
    collapsible: false,
    closable: false,
    footer: true,
    column: 2,
    
    permissions: [
        {
            name: 'Widget.SoprAppealsStatistic',
            selector: 'soprappealsstatisticportlet' ,
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
            closedAppealsCount = record.get('ClosedAppealsCount');

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
                            '<td class="result-table-header result-table-text">Закрытые обращения</td>' +
                            '<td class="result-table-text">' + closedAppealsCount + '</td>' +
                        '</tr>' +
                    '</table>' +
                    '<div class="clearfix"></div>' +
                '</div>';

            me.remove(me.down('[ui=appealsStatisticPortlet]'));
            me.add({
                xtype: 'component',
                ui: 'appealsStatisticPortlet',
                renderTpl: new Ext.XTemplate(stringTpl)
            });
        }

        me.mask.hide();
    },
    
    loadData: function() {
        var me = this,
            model = Ext.ModelManager.getModel('B4.model.rapidresponsesystem.AppealsStatistic');

        model.load(0, {
            callback: function (record) {
                me.build(record);
            }
        });
    },
    
    redirectToAppealRegistry: function() {
        b4app.redirectTo('appealcits');
    }
});