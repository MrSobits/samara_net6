Ext.define('B4.controller.personalaccount.PublicControl', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.PeriodFilterGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    views: ['personalAccount.PublicControlGrid'],

    mainView: 'personalAccount.PublicControlGrid',
    mainViewSelector: 'gispersaccpubliccontrolgrid',

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

    init: function () {
        this.callParent(arguments);

        this.control({
            'gispersaccpubliccontrolgrid b4updatebutton': {
                'click': function () {
                    this.getMainView().getStore().reload();
                },
                scope: this
            },
            'personalAccount_info_panel combobox[name=month]': {
                'change': this.refresh,
                scope: this
            },
            'personalAccount_info_panel combobox[name=year]': {
                'change': this.refresh,
                scope: this
            }
        });
    },

    index: function (realityObjectId, id) {
        var me = this,
            view = this.getMainView() || Ext.widget('gispersaccpubliccontrolgrid');

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
    }
});