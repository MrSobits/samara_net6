Ext.define('B4.view.Control.MenuTreePanel', {
    extend: 'Ext.tree.Panel',  
    alias: 'widget.menutreepanel',
    region: 'west',
    rootVisible: false,
    width: 200,
    split: false,
    collapsible: false,
    //viewConfig: {
    //    loadMask: true,
    //    getRowClass: function (record) {           
    //        var options = record.get('options');
    //        if (options) {
    //            if (options.selected) {
    //                return 'x-grid-row-selected x-grid-row-focused';
    //            }
    //        }
    //        return '';
    //    }
    //},
    useArrows: true
});