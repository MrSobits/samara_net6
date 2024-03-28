/* 
* Контроллер "Аналитические показатели рейтинга эффективности УО"
*/
Ext.define('B4.controller.efficiencyrating.Analitics',
{
    extend: 'B4.base.Controller',

    requires: [
        'B4.enums.efficiencyrating.AnaliticsLevel',
        'B4.enums.efficiencyrating.Category',
        'B4.enums.efficiencyrating.DiagramType',
        'B4.enums.efficiencyrating.ViewParam',

        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.efficiencyrating.Analitics',

        'B4.utils.Highcharts'
    ],

    models: ['efficiencyrating.EfficiencyRatingAnaliticsGraph'],
    views: [
        'efficiencyrating.analitics.Panel',
        'efficiencyrating.analitics.GraphPanel',
        'efficiencyrating.analitics.GraphGrid',
        'efficiencyrating.analitics.GraphWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'efficiencyrating.analitics.Panel',
    mainViewSelector: 'efanaliticspanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'efanaliticspanel'
        },
        {
            ref: 'chartConstructorView',
            selector: 'highchart[name=constructorChart]'
        },
        {
            ref: 'chartContainer',
            selector: 'container[name=Highcharts]'
        },
        {
            ref: 'constructorForm',
            selector: 'efanaliticsconstructorpanel form'
        },
        {
            ref: 'graphContainer',
            selector: 'container[name=GraphContainer]'
        }
    ],

    aspects: [
        {
            /* Аспект для верхнего грида */
            xtype: 'analiticsconstructoraspect',
            name: 'AnaliticsRatingValueEditAspect',
            editFormSelector: 'efanaliticsconstructorpanel form',
            modelName: 'efficiencyrating.EfficiencyRatingAnaliticsGraph',
            gridSelector: 'efanaliticsgraphgrid[name=CategoryRatingValue]',
            category: 10, // B4.enums.efficiencyrating.Category.RatingValue
            showGraphWindowSelector: 'analiticsgraphwindow',
            showGraphWindowEditView: 'efficiencyrating.analitics.GraphWindow',
            chartSelector: 'panel[name=Highcharts]',

            getDataFromUI: function () {
                return this.controller.getDataFromUI();
            },
            listeners: {
                beforesetformdata: function (asp) {
                    asp.mask('Загрузка', this.controller.getMainView());
                },

                aftersetformdata: function (asp, rec, form) {
                    var me = this;

                    asp.unmask();

                    me.controller.getConstructorForm().key = this.category;
                    me.controller.getMainView().down('tabpanel').setActiveTab(1);

                    if (rec.get('Data')) {
                        me.controller.setPanelData.apply(me.controller, [rec.get('Data')]);
                    }
                    me.controller.drawGraph.apply(me.controller);
                },

                beforesave: function() {
                    this.controller.getGraphContainer().disable();
                },
                savesuccess: function () {
                    this.controller.getGraphContainer().enable();
                },
                savefailure: function () {
                    this.controller.getGraphContainer().enable();
                }
            }
        },
        {
            /* Аспект для нижнего грида */
            xtype: 'analiticsconstructoraspect',
            name: 'AnaliticsFactorValueEditAspect',
            editFormSelector: 'efanaliticsconstructorpanel form',
            modelName: 'efficiencyrating.EfficiencyRatingAnaliticsGraph',
            gridSelector: 'efanaliticsgraphgrid[name=CategoryFactorValue]',
            category: 20, // B4.enums.efficiencyrating.Category.FactorValue
            showGraphWindowSelector: 'analiticsgraphwindow',
            showGraphWindowEditView: 'efficiencyrating.analitics.GraphWindow',
            chartSelector: 'panel[name=Highcharts]',

            getDataFromUI: function () {
                return this.controller.getDataFromUI();
            },
            listeners: {
                beforesetformdata: function(asp) {
                    asp.mask('Загрузка...', this.controller.getMainView());
                },

                aftersetformdata: function (asp, rec, form) {
                    var me = this;

                    asp.unmask();

                    me.controller.getConstructorForm().key = this.category;
                    me.controller.getMainView().down('tabpanel').setActiveTab(1);

                    if (rec.get('Data')) {
                        me.controller.setPanelData.apply(me.controller, [rec.get('Data')]);
                    }

                    me.controller.drawGraph.apply(me.controller);
                },

                beforesave: function () {
                    this.controller.getGraphContainer().disable();
                },
                savesuccess: function () {
                    this.controller.getGraphContainer().enable();
                },
                savefailure: function () {
                    this.controller.getGraphContainer().enable();
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh.Orgs.EfficiencyRating.Analitics.Graphics.View',
                    applyTo: 'actioncolumn',
                    selector: 'efanaliticsgraphgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.EfficiencyRating.Analitics.Graphics.Edit',
                    applyTo: 'b4editcolumn',
                    selector: 'efanaliticsgraphgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.EfficiencyRating.Analitics.Graphics.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'efanaliticsgraphgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.EfficiencyRating.Analitics.Constructor.View', 
                    applyTo: 'tabpanel tab[text=Конструктор]',
                    selector: 'efanaliticspanel', 
                    applyBy: function(component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },

                { name: 'Gkh.Orgs.EfficiencyRating.Analitics.Constructor.Create', applyTo: 'button[actionName=savegraph]', selector: 'efanaliticsconstructorpanel' }
            ]
        }
    ],

    isValidForm: function () {
        var me = this,
            form = me.getConstructorForm(),
            asp = me.getAspect('AnaliticsRatingValueEditAspect'), // здесь нам не важно, какой аспект для простого сохранения
            params = form.getValues();

        if (!form.getForm().isValid()) {
            var errorMessage = asp.getFormErrorMessage(form);
            Ext.Msg.alert('Ошибка!', errorMessage);
            return false;
        }

        if (!params.ManagingOrganizations && !params.Municipalities) {
            Ext.Msg.alert('Ошибка!', 'Выберите управляющую организацию или муниципальный район');
            return false;
        }

        return true;
    },

    setPanelData: function(data) {
        this.getChartContainer().data = data;
    },

    drawGraph: function(btn) {
        var me = this,
            panel = me.getChartContainer(),
            container,
            data = panel.data;

        if (!data) {
            panel.removeAll(true);
            return;
        }
        container = Ext.widget('container', { flex: 1 });

        data.type = btn ? btn.chart.type : data.type || 'spline';
        panel.diagramType = data.type;

        panel.disable();
        panel.removeAll(true);
        panel.add(container);
        B4.utils.Highcharts.getConfiguredHighchart(container, data);
        panel.enable();
    },

    buildGraph: function() {
        var me = this,
            form = me.getConstructorForm(),
            chartContainer = me.getChartContainer(),
            params = form.getValues();

        params.DiagramType = B4.utils.Highcharts.getEnumType(chartContainer.diagramType || 'spline');
        for (var property in params) {
            if (Array.isArray(params[property])) {
                params[property] = Ext.encode(params[property]);
            }
        }

        if (!me.isValidForm()) {
            return;
        }

        me.mask('Загрузка...', chartContainer);
        Ext.Ajax.request({
            method: 'POST',
            url: B4.Url.action('GetGraph', 'EfficiencyRatingAnaliticsGraph'),
            params: params,
            success: function (response) {
                var data = Ext.JSON.decode(response.responseText);
                chartContainer.data = data;
                me.unmask();
                me.drawGraph();
            },
            failure: function (response) {
                var obj = Ext.decode(response.responseText);
                me.unmask();
                Ext.Msg.alert('Ошибка', obj.message || 'Не удалось загрузить данные');
            }
        });
    },

    getDataFromUI: function () {
        var container = this.getChartContainer();

        return {
            DiagramType: B4.utils.Highcharts.getEnumType(container.diagramType || 'spline'),
            Data: container.data
        };
    },

   saveGraph: function () {
       var me = this,
           asp = me.getAspect('AnaliticsRatingValueEditAspect'); // здесь нам не важно, какой аспект для простого сохранения

       asp.saveRequestHandler.apply(asp);
   },

    init: function () {
        this.callParent(arguments);

        this.control({
            'efanaliticspanel container[name=ChartButtons] button': { 'click': { fn: this.drawGraph, scope: this } },
            'efanaliticspanel button[actionName=buildgraph]': { 'click': { fn: this.buildGraph, scope: this } },
            'efanaliticspanel button[actionName=savegraph]': { 'click': { fn: this.saveGraph, scope: this } },

            'efanaliticsconstructorpanel form': {
                'afterrender': {
                    fn: function(form) {
                        form.getForm().isValid();
                    },
                    scope: this
                }
            },

            'efanaliticspanel b4selectfield[name=ManagingOrganizations]': {
                'beforeload': {
                    fn: function (sf, operation, store) {
                        var periodIds = this.getMainView().down('b4selectfield[name=Periods]').getValue();
                        operation.params.periodIds = Ext.encode(periodIds);
                    },
                    scope: this
                }
            },

            'efanaliticspanel b4selectfield[name=Periods]': {
                'change': {
                    fn: function (sf) {
                        var periodIds = sf.getValue(),
                            toActiveManorg = periodIds && periodIds.length,
                            sfManorg = this.getMainView().down('b4selectfield[name=ManagingOrganizations]');

                        toActiveManorg 
                            ? sfManorg.enable()
                            : sfManorg.disable();
                    },
                    scope: this
                }
            },

            'efanaliticspanel b4combobox[name=Category]': {
                'change': {
                    fn: function (cb) {
                        var factorAllowBlank = cb.getValue() !== B4.enums.efficiencyrating.Category.FactorValue,
                            cbFactorCode = this.getMainView().down('b4combobox[name=FactorCode]');

                        cbFactorCode.allowBlank = factorAllowBlank;
                        factorAllowBlank
                            ? cbFactorCode.disable()
                            : cbFactorCode.enable();

                        cbFactorCode.isValid();

                        if (factorAllowBlank) {
                            cbFactorCode.setValue(null);
                        }
                    },
                    scope: this
                }
            },

            'efanaliticspanel tabpanel[name=maintab]': {
                'tabchange' : {
                    fn: function(tabPanel, newCard, oldCard, eOpts) {
                        var me = this,
                            asp = this.getAspect('AnaliticsRatingValueEditAspect'),
                            chartConstructor = me.getConstructorForm();

                        if (newCard.itemId === 'GraphPanel') {
                            me.loadStores();
                            chartConstructor.key = null;
                        } else {
                            if (!chartConstructor.key) {
                                asp.gridAction.apply(asp, [me.getMainView().down('b4grid[name=CategoryRatingValue]'), 'add']);
                            }
                        }
                    },
                    scope: this
                }
            }
        });
    },

    loadStores: function() {
        var me = this,
           view = me.getMainView(),
           ratStore = view.down('efanaliticsgraphgrid[name=CategoryRatingValue]').getStore(),
           facStore = view.down('efanaliticsgraphgrid[name=CategoryFactorValue]').getStore();

        ratStore.load();
        facStore.load();
    },

    index: function() {
        var me = this,
           view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.application.deployView(view);

        me.loadStores();
    }
});