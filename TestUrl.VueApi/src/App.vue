<template>
    <HomeComponent @getDetails="getDetails" @createTest="createTest"/>
    <TestComponent :link="link" :id="id"/>
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
                id: -1
            }
        },
        methods: {
            getDetails: function (id: number) {
                this.id = id
                console.log('child component said login', id)
            },
            createTest: function (link: string) {
                this.link = link
                console.log('child component said login', link)
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
