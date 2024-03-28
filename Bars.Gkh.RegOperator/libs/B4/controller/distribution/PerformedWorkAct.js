Ext.define('B4.controller.distribution.PerformedWorkAct', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.regop.Distribution',
        'B4.enums.ActPaymentType',
        'B4.ux.grid.column.Enum'
    ],

    models: [],

    stores: ['objectcr.PerformedWorkActForSelect'],

    views: [
        'suspenseaccount.DistributionPanel',
        'suspenseaccount.DistributionObjectsEditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'suspenseaccount.DistributionPanel',
    mainViewSelector: 'suspaccdistribpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'suspaccdistribpanel'
        }
    ],

    getCurrentContextKey: function () {
        return "performedWorkActDistribution";
    },

    aspects: [
        {
            xtype: 'gkhregopdistributionaspect',
            name: 'trancferCrDistributionAspect',
            distribPanel: 'suspenseaccount.DistributionPanel',
            distribPanelSelector: 'suspaccdistribpanel',
            storeSelect: 'objectcr.PerformedWorkActForSelect',
            storeSelected: 'objectcr.PerformedWorkActForSelect',
            columnsGridSelect: [
                { header: 'Статус', xtype: 'gridcolumn', dataIndex: 'State', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Вид работы', xtype: 'gridcolumn', dataIndex: 'TypeWorkCr', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Сумма акта', xtype: 'gridcolumn', dataIndex: 'Sum', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Вид работы', xtype: 'gridcolumn', dataIndex: 'TypeWorkCr', flex: 1, filter: { xtype: 'textfield' } }
            ],
            distribObjEditWindowSelector: 'distributionobjectseditwindow',
            distribObjStore: Ext.create('Ext.data.Store', {
                fields: [
                    { name: 'Id' },
                    { name: 'State' },
                    { name: 'Address' },
                    { name: 'TypeWorkCr' },
                    { name: 'PaymentType' },
                    { name: 'DatePayment' },
                    { name: 'Sum' },
                    { name: 'Index' }
                ]
            }),
            distribObjColumnsGrid: [
                {
                    text: 'Статус акта',
                    dataIndex: 'State',
                    flex: 1
                },
                {
                    text: 'Адрес',
                    dataIndex: 'Address',
                    flex: 1
                },
                {
                    text: 'Вид работы',
                    dataIndex: 'TypeWorkCr',
                    flex: 1
                },
                {
                    text: 'Вид оплаты',
                    dataIndex: 'PaymentType',
                    enumName: 'B4.enums.ActPaymentType',
                    xtype: 'b4enumcolumn',
                    flex: 1
                },
                {
                    text: 'Дата оплаты',
                    dataIndex: 'DatePayment',
                    flex: 1,
                    xtype: 'datecolumn',
                    format: 'd.m.Y'
                },
                {
                    text: 'Сумма по распоряжению, руб.',
                    dataIndex: 'Sum',
                    flex: 1
                }
            ],
            getApplyUrlParams: function(win, store) {
                var params,
                    distrSumFld = win.down('[name=DistrSum]'),
                    distrSum = distrSumFld ? distrSumFld.getValue() : this.initialSum,
                    mapped = Ext.Array.map(store.proxy.data, function (item) {
                        return {
                            PerformedWorkActPaymentId: item.Id,
                            Sum: item.Sum,
                            ActPaymentType: item.PaymentType
                        };
                    });

                params = {
                    code: win.code,
                    distributionId: win.distributionId,
                    distributionIds: win.distributionIds,
                    distributionSource: win.src,
                    distribSum: distrSum,
                    records: Ext.encode(mapped)
                };

                return params;
            }
        }
    ],

    index: function(id, code, sum, src) {
        var me = this,
            view = me.getMainView(),
            freshDeploy = false;
        if (!view) {
            freshDeploy = true;
            view = Ext.widget('suspaccdistribpanel');
        }
        me.bindContext(view);
        me.application.deployView(view);

        if (id.includes(',')) {
            view.distributionIds = id;
        }
        else {
            view.distributionId = id;
        }
        view.code = code;
        view.src = src;

        if (freshDeploy) {
            me.getAspect('trancferCrDistributionAspect').reconfigure(sum.replace('dot', '.'));
        }
    }
});