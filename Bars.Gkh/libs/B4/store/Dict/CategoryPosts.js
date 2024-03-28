Ext.define('B4.store.dict.CategoryPosts', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.CategoryPosts'],
    autoLoad: false,
    storeId: 'categoryPostsStore',
    model: 'B4.model.dict.CategoryPosts'
});