<template>
    <div>
        <label>Enter Link :</label>
        <input type="text" v-model="link">
        <button v-on:click="createTest()">Test</button>
    </div>
    <p></p>
    <div class="btn-group" v-for="value in pageInfo.pageCount" :key="value">
        <button :disabled="isActive[value-1]" v-on:click="getLinksOnPage(value)">page {{value}}</button>
    </div>
    <p></p>
    <p>Test Results</p>
    <table class="table table-bordered">
        <thead>
            <tr>
                <th>Count</th>
                <th>Link</th>
                <th>Date</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(link, index) in linksOnPage" :key="index">
                <td>{{index+1}}</td>
                <td>{{link.link}}</td>
                <td>{{link.createAt}}</td>
                <td><button v-on:click="getDetails(link.id)">View Details</button></td>
            </tr>
        </tbody>
    </table>
</template>

<script>
    import axios from 'axios';

    export default {
        data() {
            return {
                link: null,
                isActive: [],
                posts: [],
                pageInfo: [],
                testedLinks: [],
                linksOnPage: []
            };
        },
        methods: {
            getLinks: function () {
                axios.get('https://localhost:5001/api/Tests')
                    .then((response) => {
                        this.posts = response.data
                        this.pageInfo = this.posts.pageInfo
                        this.testedLinks = this.posts.testedLinks
                    });
            },
            getDetails: function (id) {
                this.$emit('getDetails', {
                    id: id
                });
            },
            createTest: function () {
                if (this.link) {
                    this.$emit('createTest', {
                        link: this.link
                    })
                }
            },
            getLinksOnPage: function (page) {
                this.getEnable(page)
                this.linksOnPage = []
                for (var i = (page - 1) * this.pageInfo.pageSize; i < page * this.pageInfo.pageSize && i < this.testedLinks.length; i++) {
                    this.linksOnPage.push(this.testedLinks[i]);
                }
            },
            getEnable: function (id) {
                this.isActive = []
                for (var i = 0; i < this.pageInfo.pageCount; i++) {
                    if (i + 1 == id) {
                        this.isActive.push(true);
                    }
                    else { this.isActive.push(false); }
                }
            }
        },
        mounted() {
            this.getLinks()
        }
    }
</script>