Ext.define('B4.TextValues', {
    baseItems: {},
    moduleItems: {},
    overrideItems: {},
    getText: function(text) {
        return this.overrideItems[text.toLowerCase()]
            || this.moduleItems[text.toLowerCase()]
            || this.baseItems[text.toLowerCase()]
            || text;
    }
});