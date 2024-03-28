Ext.define('B4.view.desktop.portlet.CrStatisticData', {
    extend: 'B4.view.desktop.portal.Portlet',
    alias: 'widget.crstatisticdataportlet',
    ui: 'b4portlet',
    cls: 'x-portlet purple',
    title: 'Статистические данные КР',
    layout: 'fit',
    height: 400,
    closable: false,
    isBuilt: false,
    column: 1,
    
    requires: [
        'B4.view.desktop.NotIncludedInCrHousesPanel',
        'B4.view.desktop.IncludedInCrHousesByYearsPanel',
        'B4.view.desktop.CrWorksInCeoContextPanel',
        'B4.view.desktop.HousesWithMissingDpkrParametersPanel',
        'B4.view.desktop.HousesWithNotFilledFiasPanel',
        'B4.view.desktop.BaseCrStatisticWidgetPanel',
        'B4.view.desktop.CostOfWorksInStructuralElementContextPanel'
    ],
    
    permissions: [
        {
            name: 'Widget.CrStatisticData.View',
            applyTo: 'crstatisticdataportlet',
            selector: 'portalpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'Widget.CrStatisticData.NotIncludedInCrHouses.View',
            applyTo: 'notincludedincrhousespanel',
            selector: 'crstatisticdataportlet',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.tab.show();
                    }
                    else {
                        component.tab.hide();
                    }
                }
            }
        },
        {
            name: 'Widget.CrStatisticData.WorksNotIncludedPublishProgram.View',
            applyTo: 'worksnotincludedpublishprogrampanel',
            selector: 'crstatisticdataportlet',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.tab.show();
                    }
                    else {
                        component.tab.hide();
                    }
                }
            }
        },
        {
            name: 'Widget.CrStatisticData.HousesWithNotFilledFias.View',
            applyTo: 'houseswithnotfilledfiaspanel',
            selector: 'crstatisticdataportlet',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.tab.show();
                    }
                    else {
                        component.tab.hide();
                    }
                }
            }
        },
        {
            name: 'Widget.CrStatisticData.IncludedInCrHousesByYears.View',
            applyTo: 'includedincrhousesbyyearspanel',
            selector: 'crstatisticdataportlet',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.tab.show();
                    }
                    else {
                        component.tab.hide();
                    }
                }
            }
        },
        {
            name: 'Widget.CrStatisticData.CrWorksInCeoContext.View',
            applyTo: 'crworksinceocontextpanel',
            selector: 'crstatisticdataportlet',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.tab.show();
                    }
                    else {
                        component.tab.hide();
                    }
                }
            }
        },
        {
            name: 'Widget.CrStatisticData.HousesWithMissingDpkrParameters.View',
            applyTo: 'houseswithmissingdkprparameterspanel',
            selector: 'crstatisticdataportlet',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.tab.show();
                    }
                    else {
                        component.tab.hide();
                    }
                }
            }
        },
        {
            name: 'Widget.CrStatisticData.CrBudgeting.View',
            applyTo: 'budgetingpanel',
            selector: 'crstatisticdataportlet',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.tab.show();
                    }
                    else {
                        component.tab.hide();
                    }
                }
            }
        },
        {
            name: 'Widget.CrStatisticData.CostOfWorksInStructuralElementContext.View',
            applyTo: 'costofworksinstructuralelementcontextpanel',
            selector: 'crstatisticdataportlet',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.tab.show();
                    }
                    else {
                        component.tab.hide();
                    }
                }
            }
        }
    ],

    actions: {},

    initComponent: function () {
        var me = this, 
            panels = [
            {
                panelName: 'notincludedincrhousespanel',
                reportName: 'NotIncludedInCrHousesReport',
            },
            {
                panelName: 'houseswithmissingdkprparameterspanel',
                reportName: 'HousesWithMissingParamsReport'
            },
            {
                panelName: 'budgetingpanel',
                reportName: 'CrBudgetingReport'
            },
            {
                panelName: 'worksnotincludedpublishprogrampanel',
                reportName: 'WorksNotIncludedPublishProgramReport'
            },
            {
                panelName: 'includedincrhousesbyyearspanel',
                reportName: 'IncludedInCrHousesByYearsReport'
            },
            {
                panelName: 'crworksinceocontextpanel',
                reportName: 'CrCeoWorkReport'
            },
            {
                panelName: 'costofworksinstructuralelementcontextpanel',
                reportName: 'CostOfWorksInStructuralElementContextReport'
            },
            {
                panelName: 'houseswithnotfilledfiaspanel',
                reportName: 'HousesWithNotFilledFiasReport'
            }
        ];
        
        Ext.each(panels, function (panel) {
           Ext.apply(me.actions, {
               [panel.panelName]: {
                   afterrender: {
                       fn: me.setPanelMunicipality
                   }
               },
               [panel.panelName + ' #exportBtn']: {
                click: {
                    fn: (btn) => me.onExportButtonClick(btn, panel.panelName, panel.reportName)
                }
            },
           });
        });

        Ext.apply(me.actions, {            
            /* Отображение статистики */
            'notincludedincrhousespanel #moChangePanel #showBtn': {
                click: {
                    fn: (btn) =>
                        me.onShowButtonClick(btn,
                            'notincludedincrhousespanel',
                            'GetNotIncludedInCrHousesCount',
                            function (panel, data) {
                                panel.down('highchart').updatePoint({
                                    y: data.Percentage,
                                    x: data.Count
                                });
                            })
                }
            },

            'worksnotincludedpublishprogrampanel #moChangePanel #showBtn': {
                click: {
                    fn: (btn) =>
                        me.onShowButtonClick(btn,
                            'worksnotincludedpublishprogrampanel',
                            'GetWorksNotIncludedPublishProgramCount',
                            function (panel, data) {
                                panel.down('highchart').updatePoint({
                                    y: data.Percentage,
                                    x: data.Count
                                });
                            })
                }
            },

            'houseswithnotfilledfiaspanel #moChangePanel #showBtn': {
                click: {
                    fn: (btn) =>
                        me.onShowButtonClick(btn,
                            'houseswithnotfilledfiaspanel',
                            'GetHousesWithNotFilledFias',
                            function (panel, data) {
                                panel.down('highchart').updatePoint({
                                    y: data.Percentage,
                                    x: data.Count
                                });
                            })
                }
            },

            'includedincrhousesbyyearspanel #moChangePanel #showBtn': {
                click: {
                    fn: (btn) =>
                        me.onShowButtonClick(btn,
                            'includedincrhousesbyyearspanel',
                            'GetIncludedInCrHousesByYearsCount',
                            function (panel, data) {
                                panel.down('highchart').updatePoints(data, function (rec) {
                                    return {
                                        name: Ext.String.format('{0} г.', rec.Year),
                                        y: rec.Count,
                                        z: Math.random() * 200 + 100, // Случайное число 100 -> 300
                                        t: rec.TotalCount
                                    };
                                });
                            })
                }
            },
            
            'crworksinceocontextpanel #showBtn': {
                click: {
                    fn: (btn) =>
                        me.onShowButtonClick(btn,
                            'crworksinceocontextpanel',
                            'GetCrCeoWorkCounts',
                            function (panel, data) {
                                panel.down('highchart').updatePoints(data, function (rec) {
                                    return {
                                        name: rec.CommonEstateObjectName,
                                        y: rec.WorkCount,
                                        z: Math.random() * 200 + 100 // Случайное число 100 -> 300
                                    };
                                });
                            })
                }
            },

            'houseswithmissingdkprparameterspanel #moChangePanel #showBtn': {
                click: {
                    fn: (btn) =>
                        me.onShowButtonClick(btn,
                            'houseswithmissingdkprparameterspanel',
                            'GetHousesWithMissingParamsCount',
                            function (panel, data) {
                                panel.down('highchart').updatePoint({
                                    y: data.Percentage,
                                    x: data.Count
                                });
                            })
                }
            },

            'budgetingpanel #showBtn': {
                click: {
                    fn: (btn) =>
                        me.onShowButtonClick(btn,
                            'budgetingpanel',
                            'GetCrBudgetingCount',
                            function (panel, data) {
                                var chart = panel.down('highchart'),
                                    municipalityCombo = panel.down('b4combobox[name=Municipality]'),
                                    title = {
                                        text: municipalityCombo.getValue() ? 'Год' : 'МО'
                                    };

                                chart._chart.xAxis[0].setTitle(title);

                                chart.updateSeries(data, function (data) {
                                    return data.map(function(rec){
                                        return [rec.Name + '', rec.Value];
                                    });
                                });
                            })
                }
            },

            'costofworksinstructuralelementcontextpanel #showBtn': {
                click: {
                    fn: (btn) =>
                        me.onShowButtonClick(btn,
                            'costofworksinstructuralelementcontextpanel',
                            'GetCostOfWorksInStructuralElementContext',
                            function (panel, data) {
                                panel.down('highchart').updatePoints(data, function (rec) {
                                    return {
                                        name: rec.StructuralElementName,
                                        y: rec.WorkCost,
                                        z: Math.random() * 200 + 100 // Случайное число 100 -> 300
                                    };
                                });
                            })
                }
            },
        })
        
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    layout: 'fit',
                    margin: '10 0 0 0',
                    items: [
                        {
                            xtype: 'notincludedincrhousespanel',
                            title: '1',
                            tabConfig: {
                                tooltip: 'Дома, не попавшие в версии ДПКР'
                            }
                        },
                        {
                            xtype: 'worksnotincludedpublishprogrampanel',
                            title: '2',
                            tabConfig: {
                                tooltip: 'Работы из основной версии ДПКР, не попавшие в опубликованную программу'
                            }
                        },
                        {
                            xtype: 'houseswithnotfilledfiaspanel',
                            title: '3',
                            tabConfig: {
                                tooltip: 'Дома, у которых в Реестре жилых домов не заполнен код ФИАС'
                            }
                        },
                        {
                            xtype: 'crworksinceocontextpanel',
                            title: '4',
                            tabConfig: {
                                tooltip: 'Количество работ ДПКР в разрезе ООИ'
                            }
                        },
                        {
                            xtype: 'costofworksinstructuralelementcontextpanel',
                            title: '5',
                            tabConfig: {
                                tooltip: 'Данные по стоимости работ в разрезе КЭ'
                            }
                        },
                        {
                            xtype: 'budgetingpanel',
                            title: '6',
                            tabConfig: {
                                tooltip: 'Бюджетирование'
                            }
                        },
                        {
                            xtype: 'includedincrhousesbyyearspanel',
                            title: '7',
                            tabConfig: {
                                tooltip: 'Дома, включенные в ДПКР в разрезе годов'
                            }
                        },
                        {
                            xtype: 'houseswithmissingdkprparameterspanel',
                            title: '8',
                            tabConfig: {
                                tooltip: 'Дома, с отсутсвующими параметрами для расчета ДПКР'
                            }
                        }
                    ]
                }
            ]
        });
        
        me.callParent();
    },

    setPanelMunicipality: function(panel){
        var me = this;

        me.mask('Загрузка', panel);

        B4.Ajax.request({
            method: 'GET',
            url: B4.Url.action('ListMunicipality', 'Operator'),
            timeout: 60 * 1000,
        }).next(function (response) {
            var data = Ext.JSON.decode(response.responseText).data,
                municipality,
                municipalityCombo = panel.down('b4combobox[name=Municipality]');

            if (data && data.length !== 0) {
                data.sort(function (a, b) {
                    return a.Name - b.Name;
                });

                municipality = data[0];
            }

            municipalityCombo.store.load({
                scope: municipalityCombo,
                callback: function () {
                    this.setValue(municipality || this.store.first().data);
                }
            });

            me.unmask();
        }).error(function() {
            me.unmask();
        });
    },

    onShowButtonClick: function(btn, panelSelector, method, dataApplyFn) {
        var panel = btn.up(panelSelector),
            municipalityCombo = panel.down('b4combobox[name=Municipality]'),
            portalController = b4app.controllers.get('PortalController');

        portalController.mask('Загрузка', panel);

        B4.Ajax.request({
            method: 'GET',
            url: B4.Url.action(method, 'DpkrService'),
            timeout: 60 * 1000,
            params: {
                municipalityId: municipalityCombo.getValue()
            }
        }).next(function (response) {
            var data = Ext.JSON.decode(response.responseText);

            if (data !== undefined) {
                dataApplyFn.call(this, panel, data);
            }

            portalController.unmask();
        }).error(function () {
            portalController.unmask();
        });
    },
    
    onExportButtonClick: function (btn, panelSelector, reportName) {
        var panel = btn.up(panelSelector),
            municipalityCombo = panel.down('b4combobox[name=Municipality]'),
            params = {};

        Ext.apply(params, { municipalityId: municipalityCombo.getValue(), reportName: reportName });
        var urlParams = Ext.urlEncode(params);
        
        var newUrl = Ext.urlAppend(`/DpkrService/GetExcelFileExport/?${urlParams}`);
        window.open(B4.Url.action(newUrl));
    }
});