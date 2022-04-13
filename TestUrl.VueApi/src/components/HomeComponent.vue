<template>
    <div>
        <label>Enter Link :</label>
        <input type="text" v-model="link">
        <button v-on:click="createTest(link)">Test</button>
    </div>

    <p>{{ posts }}</p>
    <p>{{ posts.pageInfo }}</p>
    <button v-on:click="getLinks(value)" v-for="value in pageInfo.pageCount" :key="value">page {{value}}</button>

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
            <tr v-for="(link, index) in testedLinks" :key="index">
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

    export default ({
        data() {
            return {
                link: '',
                posts: [],
                pageInfo: [],
                testedLinks:[]
            };
        },
        methods: {
            getLinks: function (page) {
                axios.get('https://localhost:5001/' + page)
                    .then((response) => {
                        this.posts = response.data
                        this.pageInfo = this.posts.pageInfo
                        this.testedLinks = this.posts.testedLinks

                        console.log(typeof this.testedLinks.createAt)
                    });
            },
            getDetails: function (id) {
                this.$emit('getDetails', {
                    id: id
                })
            },
            createTest: function (link) {
                this.$emit('createTest', {
                    link: this.link
                })
            }
        },
        mounted() {
            this.getLinks(1)
        }
    })
</script>