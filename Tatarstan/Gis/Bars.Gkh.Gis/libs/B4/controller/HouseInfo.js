Ext.define('B4.controller.HouseInfo', {
    extend: 'B4.base.Controller',
    
    requires: ['B4.QuickMsg'],
    mixins: { context: 'B4.mixins.Context' },
    views: ['house.InfoPanel'],
    mainView: 'house.InfoPanel',
    mainViewSelector: 'house_info_panel',

    init: function () {
        this.callParent(arguments);

        this.control({
            '#houseMenuTree': {
                'itemclick': this.onMenuItemClick,
                'load': this.onMenuLoad
            },
            scope: this
        });
    },

    refs: [
        {
            ref: 'infoLabel',
            selector: '#houseInfoLabel'
        },
        {
            ref: 'menuTree',
            selector: '#houseMenuTree'
        },
        {
            ref: 'mainTab',
            selector: '#houseMainTab'
        },
        {
            ref: 'comboMonth',
            selector: 'house_info_panel combobox[name=month]'
        },
        {
            ref: 'comboYear',
            selector: 'house_info_panel combobox[name=year]'
        }
    ],

    index: function (id) {
        var me = this,
            view = this.getMainView() || Ext.widget('house_info_panel');
        
        this.bindContext(view);
        
        this.setContextValue(view, 'houseId', id);
        this.application.deployView(view);

        B4.Ajax.request({
            url: B4.Url.action('get', 'house'),
            method: 'POST',
            params: { id: id }
        }).next(function(response) {
            var data = Ext.decode(response.responseText);
            if (data.data && data.data.Address) {
                me.getInfoLabel().update({ text: data.data.Address });
            }
        });
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

    __handleNode: function(senderView, record) {
        var houseId;
        if (record.get('leaf')) {
            houseId = this.getContextValue(senderView, 'houseId');
            this.application.redirectTo(Ext.String.format(record.get('moduleScript'), houseId));
        }
    },

    onMenuItemClick: function (view, record) {
        this.__handleNode(view, record);
    },

    deployViewKeys: {
        'house_info': 'deployTabs'
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