Ext.define('B4.controller.PersonalAccountAccruals', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.PeriodFilterGrid'],

    mixins: { context: 'B4.mixins.Context' },
    views: [
        'personalAccount.AccrualsGrid',
        'analysisreport.AnalysisReportForm'
    ],
    mainView: 'personalAccount.AccrualsGrid',
    mainViewSelector: 'personalAccount_accruals_grid',

    init: function () {
        this.callParent(arguments);

        this.control({
            'personalAccount_accruals_grid b4updatebutton': {
                'click': function () {
                    this.getMainView().getStore().reload();
                },
                scope: this
            },
            'personalAccount_accruals_grid b4editcolumn': {
                'click': this.openAnalysisReportForm
            },
            'personalAccount_accruals_grid': {
                'celldblclick': this.openAnalysisReportForm
            },
            'personalAccount_info_panel combobox[name=month]': {
                'change': this.refresh,
                scope: this
            },
            'personalAccount_info_panel combobox[name=year]': {
                'change': this.refresh,
                scope: this
            },
            'analysisreportform': {
                'show': this.onLoadAnalysisReportForm
            }
        });
    },

    refs: [
        {
            ref: 'comboMonth',
            selector: 'personalAccount_info_panel combobox[name=month]'
        },
        {
            ref: 'comboYear',
            selector: 'personalAccount_info_panel combobox[name=year]'
        }
    ],

    index: function (realityObjectId, id) {
        var me = this,
            view = this.getMainView() || Ext.widget('personalAccount_accruals_grid');

        me.bindContext(view);
        me.setContextValue(view, 'apartmentId', id);
        me.setContextValue(view, 'realityObjectId', realityObjectId);
        
        me.application.deployView(view, 'personalAccount_info');

        view.getStore().getProxy().setExtraParam('apartmentId', id);
        view.getStore().getProxy().setExtraParam('realityObjectId', realityObjectId);
        me.refresh();
    },

    refresh: function () {
        var view = this.getMainView(),
            comboMonth,
            comboYear;

        comboMonth = this.getComboMonth();
        comboYear = this.getComboYear();
        if (!view || !comboMonth.getValue() || !comboYear.getValue()) {
            return;
        }

        var proxy = view.getStore().getProxy();
        proxy.setExtraParam('month', comboMonth.getValue());
        proxy.setExtraParam('year', comboYear.getValue());

        view.getStore().load();
    },

    openAnalysisReportForm: function (object) {
        var sel = object.store.getRange()[object.highlightedItem.viewIndex].raw,
            comboMonth = this.getComboMonth(),
            comboYear = this.getComboYear();

        //переход по URL
        //this.application.redirectTo(Ext.String.format('#personalaccountinfo/{0}/accruals/analysisreport/{1}/{2}/{3}/{4}', sel.ApartmentId, sel.ServiceId, sel.SupplierId, comboMonth.getValue(), comboYear.getValue()));

        //открытие только вьюшки, без загрузки контроллера
        var analysisReportView = Ext.widget('analysisreportform', {
            closable: true,
            apartmentId: sel.ApartmentId,
            serviceId: sel.ServiceId,
            supplierId: sel.SupplierId,
            month: comboMonth.getValue(),
            year: comboYear.getValue()
        });

        analysisReportView.show();
    },

    onLoadAnalysisReportForm: function (win) {
        var me = this;

        win.getEl().mask('Загрузка');

        Ext.util.CSS.createStyleSheet(
            '.no-icon { background-image:none; margin-left: -5px; !important; '
        );

        var apartmentId = win.apartmentId;
        var serviceId = win.serviceId;
        var supplierId = win.supplierId;
        var month = win.month;
        var year = win.year;

        B4.Ajax.request({
            url: 'CommonParams/GetAnalysisReportData',
            method: 'POST',
            params: {
                apartmentId: apartmentId,
                serviceId: serviceId,
                supplierId: supplierId,
                month: month,
                year: year
            }
        }).next(function (jsonResp) {
                var resultData = Ext.JSON.decode(jsonResp.responseText).data;

                win.down('label[name=LS]').setText(resultData.ApartmentId);
                win.down('label[name=CalcMonth]').setText(resultData.CalcMonth);
                win.down('label[name=Address]').setText(resultData.Address);
                win.down('label[name=Supplier]').setText(resultData.Supplier);

                win.setTitle('Протокол расчета начислений по услуге \"' + resultData.Service + '\"');

                var reportTree = win.down('treepanel[name=reportTree]');
                var root = reportTree.getRootNode();

                me.__addNode(resultData.ReportTree, root);

                root.expand();

                win.getEl().unmask();
            });
    },

    __addNode: function (data, parentNode, operation) {
        var me = this;

        var text = data.Title + ' <b>' + Ext.util.Format.number(data.Charge, '0,000.00') + '</b>';

        var node = parentNode.appendChild({
            Title: text,
            clickable: false,
            //iconCls: 'no-icon',
            Note: data.Note,
            leaf: !!(data.Left == null && data.Right == null)
        });

        if (operation != null) {
            parentNode.appendChild({
                Title: ' <b>' + operation + '</b>',
                iconCls: 'no-icon',
                leaf: true
            });
        }

        if (data.Left != null) {
            me.__addNode(data.Left, node, data.ChildsOperation);
        }

        if (data.Right != null) {
            me.__addNode(data.Right, node);
        }
    }
});