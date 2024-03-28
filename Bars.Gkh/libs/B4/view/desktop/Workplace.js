Ext.define('B4.view.desktop.Workplace', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.workplace',
    tabConfig: {
        hidden: true
    },

    /**
     * @cfg{B4.view.desktop.PortalPanel/Object} portalPanel
     * Панель, содержащая экземпляры {@link B4.view.desktop.portal.Portlet}
     */

    /**
     * @cfg{Ext.panel.Panel/Object} favorites
     * Меню быстрого перехода к избранным разделам
     */

    /**
     * @cfg{Ext.Container/Ext.Toolbar/Object} statusBar
     * Нижняя панель рабочего стола, на котором отображаются информационные иконки.
     * Будет добавлен в <pre><code>this.dockedItems</code></pre> со свойством <pre><code>docked = 'bottom'</code></pre>
     */


    initComponent: function () {
        var me = this,
            items = me.items || [],
            dockedItems = me.dockedItems || [];

        me.initLicense();
        me.initVersion();
        if (me.portalPanel) {
            items.push(me.portalPanel);
        }
        if (me.statusBar) {
            dockedItems.push(Ext.apply(me.statusBar, { dock: 'bottom' }));
        }
        if (me.favorites) {
            dockedItems.push(Ext.apply(me.favorites, { dock: 'bottom' }));
        }
        me.items = items;
        me.dockedItems = dockedItems;
        me.callParent(arguments);
    },

    initLicense: function () {
        B4.Ajax.request({
            url: B4.Url.action('GetLicenseNumber', 'Lic')
        })
        .next(function (resp) {
            var data = Ext.JSON.decode(resp.responseText),
                licenseText = (data && data.licNum)
                    ? "Номер лицензии: " + data.licNum
                    : "Лицензия не установлена";

            Ext.DomHelper.insertFirst(Ext.dom.Query.select(
                '#contentPanel-body > .x-panel')[0],
                { id: 'lin-div', 'class': 'lin-body', html: '<div class="license-number">' + licenseText + '</div>' }, true);
        })
        .error(function (error) {
            console.log('Нет модуля лицензирования');
        });
    },

    initVersion: function () {
        B4.Ajax.request({
            url: B4.Url.action('GetBuildInfo', 'SystemVersionInfo')
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText);
            if (json) {
                var version = json.AppVersion,
                    build = json.BuildNumber;

                Ext.DomHelper.insertFirst(Ext.dom.Query.select('#contentPanel-body > .x-panel')[0],
                    {
                        'id': 'ver-div',
                        'class': 'ver-body',
                        'html': '<div class="license-number">'
                            + 'Версия: ' + version
                            + ' (номер сборки: ' + build + ')'
                            + '</div>'
                    }, true);
            }
        })
        .error(function (error) {
            console.log('Не удалось получить версию');
        });
    }
});