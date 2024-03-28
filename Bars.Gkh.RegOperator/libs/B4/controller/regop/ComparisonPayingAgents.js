Ext.define('B4.controller.regop.ComparisonPayingAgents', {
    extend: 'B4.base.Controller',

    views: [
        'regop.comparisonpayingagents.MainPanel'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'comparisonpayingagentspanel'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'comparisonpayingagentspanel button[name="accept"]': { 'click': { fn: me.onAccept, scope: me } },
            'comparisonpayingagentspanel b4enumcombo': { 'change': { fn: me.onComboChange, scope: me } },
            'comparisonpayingagentspanel button[name="autocomp"]': { 'click': { fn: me.onAutoResolve, scope: me } },
            'comparisonpayingagentspanel [name="payagent"]': {
                'itemclick': { fn: me.onAgentRowAction, scope: me },
                'render' : function(grid) {
                    grid.getStore().on('beforeload', function(store, opts) {
                        Ext.apply(opts.params, {
                            type: me.getMainView().down('b4enumcombo').getValue()
                        });
                    });
                }
            },
            'comparisonpayingagentspanel [name="payments"]': { 'itemclick': { fn: me.onOtherRowAction, scope: me } },
            'comparisonpayingagentspanel [name="suspense"]': { 'itemclick': { fn: me.onOtherRowAction, scope: me } },
            'comparisonpayingagentspanel [name="merge"]': { 'rowaction': { fn: me.onMergeRowAction, scope: me } }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('comparisonpayingagentspanel');

        me.bindContext(view);
        me.application.deployView(view);

        me.setContextValue(view, 'record', {
            Doc: null,
            Other: null
        });
    },

    onComboChange: function () {
        var me = this,
            val = Array.prototype.slice.call(arguments, 1, 2)[0],
            view = me.getMainView(),
            agents = view.down('gridpanel[name=payagent]'),
            grid1 = view.down('gridpanel[name=payments]'),
            grid2 = view.down('gridpanel[name=suspense]'),
            btn = view.down('button[name=autocomp]'),
            merge = view.down('gridpanel[name="merge"]');

        merge.getStore().removeAll();
        agents.getStore().load({
            params: {
                type: val
            }
        });

        if (val == 10) {
            btn.setDisabled(btn.isHidden());
            grid2.hide();
            grid1.show();
            grid1.getStore().load();
        } else {
            btn.setDisabled(!btn.isHidden());
            grid1.hide();
            grid2.show();
            grid2.getStore().load();
        }
    },

    onAutoResolve: function() {
        var me = this;

        me.mask();

        B4.Ajax.request({
            url: B4.Url.action('ResolveUnacceptedPayments', 'BankDocumentResolver')
        }).next(function() {
            me.unmask();
            me.reload();
        }).error(function() {
            me.unmask();
            me.reload();
        });
    },

    onAgentRowAction: function(grid, record) {
        this.setRecord(record, null);
    },

    onOtherRowAction: function (grid, record) {
        this.setRecord(null, record);
    },

    setRecord: function(doc, other) {
        var me = this,
            view = me.getMainView(),
            record = me.getContextValue(view, 'record'),
            grid = view.down('gridpanel[name="merge"]'),
            store = grid.getStore();

        record = record || {};

        if (doc) {
            record.Doc = doc;
        }

        if (other) {
            record.Other = other;
        }

        if (record.Doc && record.Other) {
            store.add({
                DocPaymentRecordId: record.Doc.get('DocPaymentRecordId'),
                EntityId: record.Other.get('EntityId'),
                AccountNumAgent: record.Doc.get('AccountNum'),
                AccountNumOther: record.Other.get('AccountNum')
            });

            record = null;
        }

        me.setContextValue(view, 'record', record);
    },

    onMergeRowAction: function(grid, action, record) {
        if (action === 'delete') {
            grid.getStore().remove(record);
        }
    },

    onAccept: function() {
        var me = this,
            view = me.getMainView(),
            grid = view.down('gridpanel[name="merge"]'),
            store = grid.getStore(),
            type = view.down('b4enumcombo').getValue();

        if (!type) {
            B4.QuickMsg.msg('Ошибка', 'необходимо выбрать тип оплаты', 'fail');
            return false;
        }

        me.mask();

        var data = Ext.Array.map(store.getRange(), function(item) {
            return {
                DocPaymentRecordId: item.get('DocPaymentRecordId'),
                EntityId: item.get('EntityId')
            };
        });

        B4.Ajax.request({
            url: B4.Url.action('ResolveDocs', 'BankDocumentResolver'),
            params: {
                type: type,
                records: Ext.encode(data)
            }
        }).next(function() {
            me.unmask();
            me.reload();
        }).error(function() {
            me.unmask();
            me.reload();
        });
    },

    reload: function() {
        var me = this,
            view = me.getMainView(),
            grid = view.down('gridpanel[name=payments]'),
            payAgents = view.down('gridpanel[name=payagent]'),
            merge = view.down('gridpanel[name="merge"]');

        grid.getStore().load();
        payAgents.getStore().load();
        merge.getStore().removeAll();
    }
});