require.config({
    baseUrl: '/Contents',
    paths: {
        'vue': 'assets/js/vue',
        'lodash': 'assets/js/lodash'
    }
});

require(['vue', 'lodash'], function (Vue, _) {
    var data = [
        { name: '001', type: 'A' },
        { name: '002', type: 'A' },
        { name: '003', type: 'B' },
        { name: '004', type: 'B' },
        { name: '005', type: 'B' },
    ];
    var dataGroup = _.groupBy(data, 'type');

    new Vue({
        el: '#app',
        data: {
            list: dataGroup
        }
    });
});