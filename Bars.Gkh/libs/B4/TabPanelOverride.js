Ext.override(Ext.tab.Panel, {
    hideTab: function(component) {
        var me = this,
            tab = component.tab || component;
        tab.hide();

        if (this.isAllTabsHidden()) {
            me.hide();
        } else if (tab.active) {
            me.setActiveTab(me.getVisibleTabs()[0]);
        }
    },

    showTab: function(component) {
        var me = this,
            tab = component.tab || component;

        if (me.hidden) {
            me.show();
        }

        tab.show();
    },

    getAllTabs: function() {
        return this.items.items;
    },

    isAllTabsHidden: function() {
        var notHiddenTabs = this.getVisibleTabs();

        return !notHiddenTabs || !notHiddenTabs.length;
    },

    getVisibleTabs: function() {
        return Ext.Array.filter(this.getAllTabs(), function(cmp) { return !(cmp.tab || cmp).hidden });
    },

    hideAllTabs: function() {
        var me = this;
        Ext.Array.forEach(this.getAllTabs(), function(cmp) { me.hideTab(cmp) });
    },

    showAllTabs: function() {
        var me = this;
        Ext.Array.forEach(this.getAllTabs(), function(cmp) { me.showTab(cmp); });
    }
});