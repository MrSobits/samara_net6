Ext.define('B4.aspects.DebtorClaimWorkEditAspect', {
    extend: 'B4.aspects.GkhEditPanel',

    alias: 'widget.debtorclaimworkeditaspect',

    claimWorkId: 0,
    claimWorkType: null,

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    init: function (controller) {
        var me = this,
            actions = {};
        this.callParent(arguments);

        actions[me.editPanelSelector +' button[actionName=updState]'] = { 'click': { fn: me.updState, scope: me } };      
        actions[me.editPanelSelector + ' claimworkaccountdetailgrid'] = {
            'store.beforeload': { fn: me.onBeforeLoadDetail, scope: me },
            'rowaction': { fn: me.onDetailRowAction, scope: me }
        }
        controller.control(actions);
    },

    updState: function () {
        var me = this,
            id = me.getRecord().getId();
        me.controller.mask('Обновление');
        B4.Ajax.request({
            url: B4.Url.action('UpdateStates', me.claimWorkType, { id: me.getRecord().getId(), inTask: false }),
            timeout: 1000 * 60 * 3
        }).next(function (response) {
            me.controller.unmask();
            var json = Ext.JSON.decode(response.responseText);
            if (json.success !== true) {
                B4.QuickMsg.msg('Внимание!', json.message, 'error');
            } else {
                me.setData(id);
            }
        }).error(function (response) {
            me.controller.unmask();
            var resp = Ext.decode(response.responseText);
            Ext.Msg.alert('Ошибка!', resp.message || 'Ошибка обновления статусов');
        });
    },

    onBeforeLoadDetail: function (store, operation) {
        operation.params.claimWorkId = this.claimWorkId;
    },

    setData: function (id, type) {
        var me = this,
            panel = me.getPanel();

        this.callParent(arguments);

        me.claimWorkId = id;
        me.claimWorkType = type;

        panel.down('claimworkaccountdetailgrid').getStore().load();
    },

    onDetailRowAction: function (grid, action, record) {
        switch (action.toLowerCase()) {
            case 'gotoaccount':
                this.goToAccount(record);
                break;
            case 'printaccountreport':
                this.printAccountReport(record);
                break;
            case 'printaccountclaimworkreport':
                this.printAccountClaimworkReport(record);
                break;;
        }
    },

    goToAccount: function (record) {
        var me = this,
            accountId = record.get('AccountId');

        if (accountId) {
            me.controller.application.redirectTo(Ext.String.format('personal_acc_details/{0}', accountId));
        }
    },

    printAccountClaimworkReport: function (record) {
        var me = this,
            accountId = record.get('AccountId'),
            params = {},
            frame = Ext.get('downloadIframe');

        if (frame != null) {
            Ext.destroy(frame);
        }

        Ext.apply(params, {
            accId: accountId,
            reportId: 'PersonalAccountClaimworkReport'
        });

        me.mask('Обработка...');
        B4.Ajax.request({
            url: B4.Url.action('ReportPrint', 'ClaimWorkReport'),
            params: params,
            timeout: 1000 * 60 * 3
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

    printAccountReport: function (record) {
        var me = this,
            accountId = record.get('AccountId'),
            params = {};

        frame = Ext.get('downloadIframe');

        if (frame != null) {
            Ext.destroy(frame);
        }

        Ext.apply(params, {
            accId: accountId,
            reportId: 'PersonalAccountReport'
        });

        me.mask('Обработка...');
        B4.Ajax.request({
            url: B4.Url.action('ReportPrint', 'ClaimWorkReport'),
            params: params,
            timeout: 1000 * 60 * 3
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
});