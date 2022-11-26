<template>
    <TestsComponent v-if="testsComponent" @getDetails="getDetails" @createTest="createTest" />
    <TestResultComponent v-if="testResultComponent" :link="link" :id="id" @openTestsComponent="openTestsComponent" />
</template>


<script lang="ts">
    import { defineComponent } from 'vue';
    import TestsComponent from './components/TestsComponent.vue';
    import TestResultComponent from './components/TestResultComponent.vue';

    export default defineComponent({
        name: 'App',
        data() {
            return {
                link: '',
                testsComponent: true,
                testResultComponent: false,
                id: -1
            }
        },
        methods: {
            getDetails: function (id: number) {
                this.id = id
                this.openTestResultComponent()
            },
            createTest: function (link: string) {
                this.link = link
                this.openTestResultComponent()
            },
            openTestResultComponent: function () {
                this.testsComponent = false
                this.testResultComponent = true
            },
            openTestsComponent: function () {
                this.testsComponent = true
                this.testResultComponent = false
                this.id = -1
                this.link = ''
            }
        },
        components: {
            TestsComponent,
            TestResultComponent
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
