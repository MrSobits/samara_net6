Ext.define('B4.aspects.ClaimWorkButtonPrintAspect', {
    extend: 'B4.base.Aspect',

    alias: 'widget.claimworkbuttonprintaspect',

    buttonSelector: null,
    codeForm: null,
    reportStore: null,
    params: {},
    displayField: 'Description',
    printController: 'ClaimWorkReport',
    printAction: 'ReportPrint',
    massPrint: false,
    getClaimWorkType: function (){},

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function (controller) {
        var actions = {};

        this.addEvents('onprintsucess');
        this.callParent(arguments);

        actions[this.buttonSelector + ' menuitem'] = { 'click': { fn: this.onMenuItemClick, scope: this } };

        this.otherActions(actions);

        this.reportStore = Ext.create('Ext.data.Store', {
            autoLoad: false,
            fields: ['Id', 'Name', 'Description'],
            proxy: {
                autoLoad: false,
                type: 'ajax',
                url: B4.Url.action('/ClaimWorkReport/GetReportList'),
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
        if (this.massPrint) {
            this.massPrintReport(itemMenu);
        } else {
            this.printReport(itemMenu);
        }
    },

    massPrintReport: function(itemMenu) {
        var me = this,
            grid = me.controller.getMainView(),
            sm = grid.getSelectionModel(),
            selected = sm.getSelection(),
            selectedIds = Ext.Array.map(selected, function(el) { return el.get('Id'); }),
            selectedAccIds = Ext.Array.map(selected, function(el) { return el.get('PersonalAccountId'); }),
            param = {
                DocumentIds: selectedIds,
                AccIds: selectedAccIds
            };       

        Ext.apply(me.params, {
            reportId: itemMenu.actionName,
            userParams: Ext.JSON.encode(param)
        });

        if (me.getClaimWorkType()) {
            Ext.apply(me.params, {
                claimWorkType: me.getClaimWorkType()
            });
        }

        if (selected.length === 0) {
            Ext.Msg.alert('Внимание', 'Необходимо выбрать записи для печати');
        } else {
            Ext.Msg.prompt({
                title: 'Формирование документов на печать',
                msg: 'Сформировать документы для выбранных записей?',
                buttons: Ext.Msg.OKCANCEL,
                fn: function (btnId, text) {
                    if (btnId === 'ok') {
                        me.mask('Формирование документов...');
                        B4.Ajax.request({
                            url: B4.Url.action('MassReportPrint', 'ClaimWorkReport'),
                            params: me.params,
                            timeout: 9999999
                        }).next(function (resp) {
                            var tryDecoded;

                            me.unmask();
                            try {
                                tryDecoded = Ext.JSON.decode(resp.responseText);
                            } catch (e) {
                                tryDecoded = {};
                            }

                            var id = resp.data ? resp.data : tryDecoded.data;

                            if (id > 0) {
                                Ext.DomHelper.append(document.body, {
                                    tag: 'iframe',
                                    id: 'downloadIframe',
                                    frameBorder: 0,
                                    width: 0,
                                    height: 0,
                                    css: 'display:none;visibility:hidden;height:0px;',
                                    src: B4.Url.action('Download', 'FileUpload', { Id: id })
                                });

                                me.fireEvent('onprintsucess', me);
                            }
                        }).error(function (err) {
                            me.unmask();
                            Ext.Msg.alert('Ошибка', err.message || err.message || err);
                        });
                    }
                }
            });
        }
    },

    printReport: function (itemMenu) {
        var me = this,
            frame = Ext.get('downloadIframe');
        if (frame != null) {
            Ext.destroy(frame);
        }

        me.getUserParams(itemMenu.actionName);
        Ext.apply(me.params, { reportId: itemMenu.actionName });

        me.mask('Обработка...');
        B4.Ajax.request({
            url: B4.Url.action('ReportPrint', 'ClaimWorkReport'),
            params: me.params,
            timeout: 9999999
        }).next(function (resp) {
            var tryDecoded;

            me.unmask();
            try {
                tryDecoded = Ext.JSON.decode(resp.responseText);
            } catch (e) {
                tryDecoded = {};
            }

            var id = resp.data ? resp.data : tryDecoded.data;

            if (id > 0) {
                Ext.DomHelper.append(document.body, {
                    tag: 'iframe',
                    id: 'downloadIframe',
                    frameBorder: 0,
                    width: 0,
                    height: 0,
                    css: 'display:none;visibility:hidden;height:0px;',
                    src: B4.Url.action('Download', 'FileUpload', { Id: id })
                });

                me.fireEvent('onprintsucess', me);
            }
        }).error(function (err) {
            me.unmask();
            Ext.Msg.alert('Ошибка', err.message || err.message || err);
        });
    },

    openInNewWindow: function () {
        return Ext.is.iPad;
    },

    getUserParams: function () {
        this.params = this.params || {};
    },

    otherActions: function () {
        //Данный метод служит для перекрытия в контроллерах где используется данный аспект
        //наслучай если потребуется к данному аспекту добавить дополнительные обработчики
    },
});