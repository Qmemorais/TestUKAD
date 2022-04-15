<template>
    <HomeComponent v-if="homeComponent" @getDetails="getDetails" @createTest="createTest" />
    <TestComponent v-if="testComponent" :link="link" :id="id" @openHomeComponent="openHomeComponent" />
</template>


<script lang="ts">
    import { defineComponent } from 'vue';
    import HomeComponent from './components/HomeComponent.vue';
    import TestComponent from './components/TestComponent.vue';

    export default defineComponent({
        name: 'App',
        data() {
            return {
                link: '',
                homeComponent: true,
                testComponent: false,
                id: -1
            }
        },
        methods: {
            getDetails: function (id: number) {
                this.id = id
                this.openTestComponent()
            },
            createTest: function (link: string) {
                this.link = link
                this.openTestComponent()
            },
            openTestComponent: function () {
                this.homeComponent = false
                this.testComponent = true
            },
            openHomeComponent: function () {
                this.homeComponent = true
                this.testComponent = false
                this.id = -1
                this.link = ''
            }
        },
        components: {
            HomeComponent,
            TestComponent
        }
    })
</script>

<style>
    #app {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
        margin-top: 60px;
    }
</style>
