Ext.define('B4.controller.AnalysisReport', {
    extend: 'B4.base.Controller',

    mixins: { context: 'B4.mixins.Context' },
    views: ['analysisreport.AnalysisReportForm'],
    stores: ['AnalysisReport'],
    mainView: 'analysisreport.AnalysisReportForm',
    mainViewSelector: 'analysisreportform',

    init: function () {
        var me = this;
        me.control({
            scope: me,
            'analysisreportform': {
                'show': me.onLoad
            }
        });

        me.callParent(arguments);
    },

    index: function (apartmentId, serviceId, supplierId, month, year) {
        var me = this,
            view = me.getMainView() || Ext.widget('analysisreportform');

        me.bindContext(view);

        me.setContextValue(view, 'apartmentId', apartmentId);
        me.setContextValue(view, 'serviceId', serviceId);
        me.setContextValue(view, 'supplierId', supplierId);
        me.setContextValue(view, 'month', month);
        me.setContextValue(view, 'year', year);

        me.application.deployView(view);
        view.show();
    },

    onLoad: function () {
        var me = this,
            win = me.getMainView();

        Ext.util.CSS.createStyleSheet(
            '.no-icon { margin-left: -5px; !important; ' 
        );

        var apartmentId = me.getContextValue(win, 'apartmentId');
        var serviceId = me.getContextValue(win, 'serviceId');
        var supplierId = me.getContextValue(win, 'supplierId');
        var month = me.getContextValue(win, 'month');
        var year = me.getContextValue(win, 'year');

        B4.Ajax.request({
            url: 'AnalysisReport/GetAnalysisReportData',
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

            me.addNode(resultData.ReportTree, root);

            root.expand();
        });
    },

    addNode: function (data, parentNode, operation) {
        var me = this;

        var text = data.Title + ' <b>' + data.Charge + '</b>';

        var node = parentNode.appendChild({
           Title: text,
           //iconCls: 'no-icon',
           Note: data.Note
        });

        if (operation != null) {
            parentNode.appendChild({
                Title: '<b>' + operation + '</b>',
                iconCls: 'no-icon',
                leaf: true
            });
        }

        if (data.Left != null) {
            me.addNode(data.Left, node, data.ChildsOperation);
        }

        if (data.Right != null) {
            me.addNode(data.Right, node);
        }
    }
});