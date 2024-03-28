Ext.define('B4.controller.PersonalAccountService', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.PeriodFilterGrid'],

    mixins: { context: 'B4.mixins.Context' },
    views: ['personalAccount.ServiceGrid'],
    mainView: 'personalAccount.ServiceGrid',
    mainViewSelector: 'personalAccount_service_grid',

    init: function () {
        this.callParent(arguments);

        this.control({
            'personalAccount_service_grid b4updatebutton': {
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
            view = this.getMainView() || Ext.widget('personalAccount_service_grid');

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