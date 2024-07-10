/*
Данный аспект предназначен для печати отчетов
*/

Ext.define('B4.aspects.GkhButtonPrintAspect', {
    extend: 'B4.base.Aspect',

    alias: 'widget.gkhbuttonprintaspect',

    buttonSelector: null,
    codeForm: null,
    reportStore: null,
    params: {},
    displayField: 'Name',
    printController: 'GkhReport',
    printAction: 'ReportPrint',

    init: function (controller) {
        var actions = {};
        this.callParent(arguments);

        actions[this.buttonSelector + ' menuitem'] = { 'click': { fn: this.onMenuItemClick, scope: this } };

        this.reportStore = Ext.create('Ext.data.Store', {
            autoLoad: false,
            fields: ['Id', 'Name', 'Description'],
            proxy: {
                autoLoad: false,
                type: 'ajax',
                url: B4.Url.action('/GkhReport/GetReportList'),
                reader: {
                    type: 'json',
                    root: 'data'
                }
            }
        });

        this.reportStore.on('beforeload', this.onBeforeLoadReportStore, this);
        this.reportStore.on('load', this.onLoadReportStore, this);

        controller.control(actions);
    },

    loadReportStore: function () {
        this.reportStore.load();
    },

    onBeforeLoadReportStore: function (store, operation) {
        operation.params = {};
        operation.params.codeForm = this.codeForm;
    },

    onLoadReportStore: function (store) {
        var me = this,
            btn = me.componentQuery(this.buttonSelector);

        if (btn) {
            btn.menu.removeAll();

            store.each(function (rec) {
                btn.menu.add({
                    xtype: 'menuitem',
                    text: rec.get(me.displayField),
                    textAlign: 'left',
                    actionName: rec.data.Id,
                    iconCls: 'icon-report'
                });
            });
        }
    },

    onMenuItemClick: function (itemMenu) {
        this.printReport(itemMenu);
    },

    printReport: function (itemMenu) {
        var me = this,
            frame = Ext.get('downloadIframe');
        if (frame != null) {
            Ext.destroy(frame);
        }

        me.getUserParams(itemMenu.actionName);
        Ext.apply(me.params, { reportId: itemMenu.actionName });

        var urlParams = Ext.urlEncode(me.params);
        var newUrl = Ext.urlAppend(Ext.String.format('/{0}/{1}/?{2}', me.printController, me.printAction, urlParams), '_dc=' + (new Date().getTime()));
        
        newUrl = B4.Url.action(newUrl);
        if (me.openInNewWindow()) {
            window.open(newUrl);
        } else {
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

    openInNewWindow: function () {
        return Ext.is.iPad;
    },

    getUserParams: function () {
        this.params = this.params || {};
    }
});