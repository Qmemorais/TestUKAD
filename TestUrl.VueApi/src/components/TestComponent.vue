<template>
    <button v-on:click="openHomeComponent">Return</button>
    <button v-if="link != ''" v-on:click="createTest">Create new Test</button>
    <button v-if="id != -1" v-on:click="getExistLinks">Get Exist Test</button>

    <p>Tested Link: {{ testedLink }}</p>
    <p>Urls(html documents) found after crawling a website {{ countExistWeb }}</p>
    <p>Urls found in sitemap {{ countExistSitemap }}</p>

    <p>Urls FOUNDED BY CRAWLING THE WEBSITE AND NOT IN SITEMAP</p>
    <table class="table table-bordered countLines">
        <thead>
            <tr>
                <th>Count</th>
                <th>Link</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(link, index) in listInWebNotSitemap" :key="index">
                <td>{{index+1}}</td>
                <td>{{link.link}}</td>
            </tr>
        </tbody>
    </table>

    <p>Urls FOUNDED IN SITEMAP AND NOT BY CRAWLING THE WEBSITE</p>
    <table class="table table-bordered countLines">
        <thead>
            <tr>
                <th>Count</th>
                <th>Link</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(link, index) in listinSitemapNotWeb" :key="index">
                <td>{{index+1}}</td>
                <td>{{link.link}}</td>
            </tr>
        </tbody>
    </table>

    <p>Urls FOUNDED BY CRAWLING THE WEBSITE AND SITEMAP.XML</p>
    <table class="table table-bordered countLines">
        <thead>
            <tr>
                <th>Count</th>
                <th>Link</th>
                <th>Time, ms</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(link, index) in listOfScan" :key="index">
                <td>{{index+1}}</td>
                <td>{{link.link}}</td>
                <td>{{link.timeOfResponse}} ms</td>
            </tr>
        </tbody>
    </table>

</template>

<script>
    import axios from 'axios';

    export default {
        props: ['link', 'id'],
        data() {
            return {
                posts: [],
                countExistWeb: 0,
                countExistSitemap: 0,
                testedLink: '',
                listOfScan: [],
                listInWebNotSitemap: [],
                listinSitemapNotWeb: []
            };
        },
        methods: {
            getExistLinks: function () {
                axios.get('https://localhost:5001/Test/' + this.id.id)
                    .then((response) => {
                        this.posts = response.data
                        this.testedLink = this.posts.testedLink
                        this.listOfScan = this.posts.listOfScan
                        this.getCounts()
                        this.getExistingLinks()
                    });
            },
            createTest: function () {
                var bodyFormData = new FormData();
                bodyFormData.append('link', this.link.link);
                axios({
                    method: "post",
                    url: "https://localhost:5001/Test/",
                    data: bodyFormData,
                    headers: { "Content-Type": "multipart/form-data" },
                })
                    .then(response => {
                        this.posts = response.data
                        this.testedLink = this.posts.testedLink
                        this.listOfScan = this.posts.listOfScan
                        this.getCounts()
                        this.getExistingLinks()
                    })
            },
            getCounts: function () {
                for (var i = 0; i < this.listOfScan.length; i++) {
                    if (this.listOfScan[i].isSitemap) {
                        this.countExistSitemap++;
                    }
                    if (this.listOfScan[i].isWeb) {
                        this.countExistWeb++;
                    }
                }
            },
            getExistingLinks: function() {
                for (var i = 0; i < this.listOfScan.length; i++) {
                    if (this.listOfScan[i].isSitemap && !this.listOfScan[i].isWeb) {
                        this.listinSitemapNotWeb.push(this.listOfScan[i]);
                    }
                    if (this.listOfScan[i].isWeb && !this.listOfScan[i].isSitemap) {
                        this.listInWebNotSitemap.push(this.listOfScan[i]);
                    }
                }
            },
            openHomeComponent: function () {
                this.$emit('openHomeComponent')
            }
        },
    }
</script>