Ext.define('B4.aspects.efficiencyrating.Analitics', {
    extend: 'B4.aspects.GridEditForm',
    requires: [
        'B4.enums.efficiencyrating.Category',
        'B4.utils.Highcharts'
    ],

    alias: 'widget.analiticsconstructoraspect',
    category: null,

    showGraphWindowSelector: null,
    showGraphWindowEditView: null,
    chartSelector: null,

    init: function(config) {
        var me = this;

        me.callParent(arguments);
        me.on('validate', me.onValidate);
        me.on('getdata', me.onGetData);
    },

    onBeforeStoreLoad: function(store, operation) {
        operation.params.category = this.category;
    },

    rowAction: function (grid, action, record) {
        if (!grid || grid.isDestroyed) return;
        if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
            switch (action.toLowerCase()) {
                case 'edit':
                    this.editRecord(record);
                    break;
                case 'delete':
                    this.deleteRecord(record);
                    break;
                case 'showgraph':
                    this.showGraph(record);
                    break;
            }
        }
    },

    otherActions: function(actions) {
        this.callParent(actions);

        actions[this.gridSelector] = Ext.apply(actions[this.gridSelector] || {}, {
            'afterrender': {
                fn: function(view) {
                    view.key = this.category;
                    view.getStore().on('beforeload', this.onBeforeStoreLoad, this);
                },
                scope: this
            }
        });
    },

    onValidate: function(asp) {
        var rec = asp.getRecordBeforeSave(asp.getForm().getRecord()),
           municipalities = rec.get('Municipalities'),
           manorgs = rec.get('ManagingOrganizations');

        if (!rec.get('DiagramType')) {
            Ext.Msg.alert('Ошибка', 'Сформируйте график перед сохранением!');
            return false;
        }

        if (!municipalities.length && !manorgs.length) {
            Ext.Msg.alert('Ошибка', 'Выберите управляющую организацию или муниципальный район!');
            return false;
        }

        return true;
    },

    onGetData: function (asp, rec) {
        Ext.apply(rec.data, asp.getDataFromUI() || {});
    },

    showGraph: function (record) {
        var me = this,
            window,
            model = this.getModel(record);

        model.load(record.getId(),
        {
            success: function (rec) {
                window = me.getGraphWindow(rec.get('Name'));
                window.show();
                me.drawGraph(window, rec.get('Data'));
            }
        });

    },

    getGraphWindow: function(title) {
        var me = this,
            window = null;

        if (me.showGraphWindowSelector) {
            window = me.componentQuery(me.showGraphWindowSelector);

            if (!window || window.isDestroyed) {
                if (me.showGraphWindowEditView) {
                    window = me.controller.getView(me.showGraphWindowEditView).create();
                }
            }
        }
        if (window) {
            window.setTitle(title);
        }

        return window;
    },

    drawGraph: function (window, data) {
        var me = this,
            panel = window.down(me.chartSelector),
            container;

        if (!data) {
            panel.removeAll(true);
            return;
        }
        container = Ext.widget('container', { flex: 1 });

        data.type = data.type || 'spline';

        panel.removeAll(true);
        panel.add(container);
        B4.utils.Highcharts.getConfiguredHighchart(container, data);
    },

    getDataFromUI: Ext.emptyFn
});