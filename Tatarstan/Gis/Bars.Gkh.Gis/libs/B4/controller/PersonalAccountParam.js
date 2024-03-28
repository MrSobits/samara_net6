Ext.define('B4.controller.PersonalAccountParam', {
    extend: 'B4.base.Controller',
    
    mixins: { context: 'B4.mixins.Context' },
    views: ['personalAccount.ParamGrid'],
    mainView: 'personalAccount.ParamGrid',
    mainViewSelector: 'personalAccount_param_grid',

    init: function () {
        this.callParent(arguments);

        this.control({
            'personalAccount_param_grid': {
                afterrender: this.refresh
            },
            'personalAccount_param_grid b4updatebutton': {
                'click': function() {
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
            view = this.getMainView() || Ext.widget('personalAccount_param_grid');

        me.bindContext(view);
        me.setContextValue(view, 'apartmentId', id);
        me.setContextValue(view, 'realityObjectId', realityObjectId);
        me.application.deployView(view, 'personalAccount_info');
    },
    
    refresh: function (cmp) {
        var me = this,
            view = me.getMainView() || cmp.up('panel').down('personalAccount_param_grid'),
            apartmentId = me.getContextValue(view, 'apartmentId'),
            realityObjectId = me.getContextValue(view, 'realityObjectId'),
            comboMonth,
            comboYear;
        if (!view || !apartmentId || !realityObjectId) return;
        view.getStore().getProxy().setExtraParam('apartmentId', apartmentId);
        view.getStore().getProxy().setExtraParam('realityObjectId', realityObjectId);
        
        comboMonth = me.getComboMonth();
        comboYear = me.getComboYear();
        if (!view || !comboMonth.getValue() || !comboYear.getValue()) {
            return;
        }

        var proxy = view.getStore().getProxy();
        proxy.setExtraParam('month', comboMonth.getValue());
        proxy.setExtraParam('year', comboYear.getValue());

        view.getStore().load();
    }
});