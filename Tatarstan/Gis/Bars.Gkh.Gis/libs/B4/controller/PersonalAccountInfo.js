Ext.define('B4.controller.PersonalAccountInfo', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.QuickMsg'
    ],
    mixins: { context: 'B4.mixins.Context' },
    views: ['personalAccount.InfoPanel', 'analysisreport.AnalysisReportForm'],
    mainView: 'personalAccount.InfoPanel',
    mainViewSelector: 'personalAccount_info_panel',

    init: function () {
        this.callParent(arguments);

        this.control({
            'personalAccount_info_panel': {
                afterrender: this.onLoad
            },

            '#personalAccountMenuTree': {
                'itemclick': this.onMenuItemClick,
                'load': this.onMenuLoad
            },
            scope: this
        });
    },

    refs: [
        {
            ref: 'infoLabel',
            selector: '#personalAccountInfoLabel'
        },
        {
            ref: 'menuTree',
            selector: '#personalAccountMenuTree'
        },
        {
            ref: 'mainTab',
            selector: '#personalAccountMainTab'
        },
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
            view = this.getMainView() || Ext.widget('personalAccount_info_panel');

        this.bindContext(view);
        this.setContextValue(view, 'realityObjectId', realityObjectId);
        this.setContextValue(view, 'accountId', id);
        this.application.deployView(view);
    },

    onLoad: function() {
        var me = this,
            comboMonth,
            comboYear;

        comboMonth = this.getComboMonth();
        comboYear = this.getComboYear();
        comboMonth.select(comboMonth.getStore().first());
        comboYear.select(comboYear.getStore().first());
    },

    onMenuLoad: function () {
        var me = this,
            menuTree = this.getMenuTree(),
            nodes = menuTree.getView().getNodes();
        if (nodes[0]) {
            var rec = menuTree.getView().getRecord(nodes[0]);
            me.__handleNode(menuTree, rec);
        }
    },

    __handleNode: function (senderView, record) {
        var accountId,
            realityObjectId;

        if (record.get('leaf')) {
            accountId = this.getContextValue(senderView, 'accountId');
            realityObjectId = this.getContextValue(senderView, 'realityObjectId');
            this.application.redirectTo(Ext.String.format(record.get('moduleScript'), realityObjectId, accountId));
        }
    },

    onMenuItemClick: function (view, record) {
        this.__handleNode(view, record);
    },

    deployViewKeys: {
        'personalAccount_info': 'deployTabs'
    },

    deployTabs: function (controller, view) {
        var container = this.getMainTab(),
            viewSelector = '#' + view.getId();

        if (!container.down(viewSelector)) {
            container.add(view);
        }
        container.setActiveTab(view);
    }
});