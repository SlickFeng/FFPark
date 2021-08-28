<template>
    <div >
        <button type="button" @click="gettest()">
            测试
        </button>
    </div>
</template>
<script>
import {requestGet} from '@/utils/request'
export default {
    name:'test',
    data(){
return {}
    },
    methods:{
        gettest(){
            requestGet('https://localhost:5001/api/wx/notice/getsubscription').then((response)=>{
                console.log(response)
            })
        }
    }
}

</script>


