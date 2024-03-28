/**
* @mixin Применяется для создания страниц wizard
*/
Ext.define('B4.mixins.WizardPage', {
    isWizardPage: true,
    isAllowed: false,
    isLoaded: false,

    initWizardPage: function (selectedSection) {
        var me = this,
            canDisabled = Ext.each(selectedSection, function (section) {
                if (me.allowSection.indexOf(section) !== -1) {
                    return false;
                }
            }) === true;
        me.setDisabled(canDisabled);
        me.isAllowed = !canDisabled;
    },

    loadWizardPage: function (force) {
        var me = this;
        if (force || !me.isLoaded) {
            me.manualLoad();
            me.isLoaded = true;
        }
    },

    manualLoad: function() {
        var me = this,
            grid = me.down('grid'),
            store = {};
        if (grid && (store = grid.getStore())) {
            store.load();
        }
    },
});